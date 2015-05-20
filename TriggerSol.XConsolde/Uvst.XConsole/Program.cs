//
// Program.cs
//
// Author:
//       Jörg Egger <joerg.egger@outlook.de>
//
// Copyright (c) 2015 Jörg Egger
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Para.Data.Client;
using TriggerSol.Boost;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using Uvst.Domain;
using Uvst.Model;

namespace Uvst.XConsole
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new Booster().InitDataStore<CachedJsonFileDataStore>("/Users/trigger/Uvst");

            string user = "S600157";
            var password = new MD5HashCalculator().CalculateHash("upnq-v4su");

            try
            {
                Console.WriteLine("Anmeldung...");
                var result = new AuthenticateService(new Transaction(), user, password, ServiceEnvironment.StagingUrl).TryAuthenticate().Result;

                if (result != null)
                { 
                    Console.WriteLine("Anmeldung erfolgreich!");
                    Console.WriteLine(result.UserName);

                    var stored = DataStoreProvider.DataStore.LoadAll<ErvRueckverkehr>().OrderByDescending(p => p.AusgangAbeholtLokal).ToList();

                    foreach (var recent in stored)
                    {
                        Console.WriteLine(recent.AusgangAbeholtLokal + "\t" + "Empfangsdatum");

                        Console.WriteLine(recent.AusgangBereitgestelltUebermittlungsStelle
                        + "\t" + recent.ZustellungTyp
                        + "\r\n\t\t\t" + recent.GerichtsAktenzeichen
                        + "\r\n\t\t\t" + recent.Partei1 + " " + recent.Partei2
                        + "\r\n\t\t\t" + recent.MessageId
                        + "\r\n\t\t\t" + (recent.AusgangBestaetigtUebermittlungsstelle.HasValue ? "BESTÄTIGT" : "OFFEN")
                        + "\r\n");
                    }
                        
                    Console.WriteLine("Taste drücken für Abruf...");
                    Console.ReadKey();

                    foreach (var item in result.ErvCodes)
                    {
                        Console.WriteLine(item.Code + " " + item.CodeId);
                    }

                    int days = -30;

                    foreach (var code in result.ErvCodes.OrderBy(p => p.Code).ToList())
                    {
                        var receive = new ErvReceiveService(code, TypeProvider.Current.GetSingle<IParaDataHttpClient>());

                        var count = Task.Run(() => receive.GetPaperboxRecentCount(days)).Result;

                        Console.WriteLine("Posteingang für ERV-Code " + code.Code + " wird abgerufen...");

                        var recentItems = Task.Run(() => receive.GetPaperboxRecent(days, count)).Result.OrderBy(p => p.AusgangBereitgestelltUebermittlungsStelle).ToList();

                        var transaction = new Transaction();
                        var mapper = new ErvRueckverkehrMapper(transaction);

                        foreach (var recent in recentItems)
                        {
                            mapper.Map(recent);

                            Console.WriteLine(recent.AusgangBereitgestelltUebermittlungsStelle.Value
                            + "\t" + recent.ZustellungTyp
                            + "\r\n\t\t\t" + recent.GerichtsAktenzeichen
                            + "\r\n\t\t\t" + recent.Partei1 + " " + recent.Partei2
                            + "\r\n\t\t\t" + recent.MessageId
                            + "\r\n\t\t\t" + (recent.AusgangBestaetigtUebermittlungsstelle.HasValue ? "BESTÄTIGT" : "OFFEN")
                            + "\r\n");
                        }

                        transaction.Commit();

                        Console.WriteLine("Anzahl abgerufene Elemente: " + count);
                        Console.WriteLine("Taste drücken für nächsten Code...");
                        Console.ReadKey();
                    }
                }
                else
                    Console.WriteLine("Anmeldung fehlgeschlagen!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();

        }
    }

    public class MD5HashCalculator
    {
        private const string Formatter = "X2";

        public string CalculateHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(input);

            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString(Formatter));

            return sb.ToString();
        }
    }

}
