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
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("UVST RÜCKVERKEHRABFRAGE-CLIENT 0.1");
            Console.WriteLine();
            Console.ResetColor();

            Console.Write("Benutzer: ");
            var user = Console.ReadLine();
            Console.Write("Passwort: ");
            var pass = Console.ReadLine();

            var passHash = new MD5HashCalculator().CalculateHash(pass);

            try
            {
                Console.WriteLine("Anmeldung...");
                SpinAnimation.Start();
                User currentUser = null;

                Task.Run(async () =>
                {
                    currentUser = await new AuthenticateService(new Transaction(), user, passHash, ServiceEnvironment.StagingUrl).TryAuthenticate();
                }).Wait();

                if (currentUser != null)
                { 
                    SpinAnimation.Stop();
                    Console.WriteLine("Anmeldung erfolgreich!");
                    Console.WriteLine("Angemeldet als " + currentUser.UserName);
                    Console.Write("Abfragezeitraum (in Tagen): ");
                    var daysBack = Console.ReadLine();
                    Console.Write("Anhänge herunterladen J/N: ");
                    bool downloadFiles = Console.ReadLine() == "J";
                    Console.WriteLine("Taste drücken für Abruf...");
                    Console.ReadKey();

                    Console.WriteLine("Aktive Codes:");
                    foreach (var item in currentUser.ErvCodes)
                    {
                        Console.WriteLine(item.Code + " - " + item.CodeId);
                    }
                    Console.WriteLine();

                    int days = int.Parse(daysBack) * -1;

                    foreach (var code in currentUser.ErvCodes.OrderBy(p => p.Code).ToList())
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Posteingang für ERV-Code " + code.Code + " wird abgerufen...");
                        Console.ResetColor();
                        Console.WriteLine();
                        SpinAnimation.Start();

                       

                        var receive = new ErvReceiveService(code, TypeProvider.Current.GetSingle<IParaDataHttpClient>());

                        int count = 0;
                        Task.Run(async () =>
                        {
                            count = await receive.GetPaperboxRecentCount(days);
                        }).Wait();

                       
                        Para.Data.UstOut[] recentItems = null;

                        Task.Run(async () =>
                        {
                            recentItems = await receive.GetPaperboxRecent(days, count);
                        }).Wait();

                        SpinAnimation.Stop();

                        var transaction = new Transaction();

                        var mapper = new ErvRueckverkehrMapper(transaction);

                        foreach (var recent in recentItems.OrderByDescending(p => p.AusgangBereitgestelltUebermittlungsStelle).ToList())
                        {
                            mapper.Map(recent);

                            Console.WriteLine(recent.AusgangBereitgestelltUebermittlungsStelle.Value
                            + "\t" + recent.ZustellungTyp
                            + "\r\n\t\t\t" + recent.GerichtsAktenzeichen
                            + "\r\n\t\t\t" + recent.Partei1 + " " + recent.Partei2
                            + "\r\n\t\t\t" + recent.MessageId);

                            Console.ForegroundColor = recent.AusgangBestaetigtUebermittlungsstelle.HasValue ? ConsoleColor.Green : ConsoleColor.Red;

                            Console.WriteLine("\t\t\t" + (recent.AusgangBestaetigtUebermittlungsstelle.HasValue ? "BESTÄTIGT" : "OFFEN"));
                            Console.WriteLine();
                            Console.ResetColor();
                            transaction.Commit();

//                            if (downloadFiles && erv.NumberOfDocuments > 0 && erv != null)
//                            {
//                                var loaderTransaction = new Transaction();
//                                Console.WriteLine();
//                                Console.WriteLine("Lade " + erv.NumberOfDocuments + " Anhänge für " + erv.ZustellungTyp + " " + erv.GerichtsAktenzeichen + " " + erv.RCode + "\r\n" + erv.MessageId);
//                                Console.WriteLine();
//
//                                var loader = new ErvAnhangDownloader(loaderTransaction, TypeProvider.Current.GetSingle<IParaDataHttpClient>());
//
//                                var x = 1;
//                                foreach (var anhang in erv.ErvAnhangList.OrderBy(p => p.TransactionId).ToList())
//                                {
//                                   
//                                    ErvAnhang resultAnhang = null;
//                                    Console.Write("Lade Anhang " + x + "/" + erv.NumberOfDocuments + " " + anhang.DocumentType + "... ");
//                                    SpinAnimation.Start();
//                                    Task.Run(async () =>
//                                    {
//                                        resultAnhang = await loader.DownloadSingle(anhang, anhang.TransactionId);
//                                    }).Wait();
//
//                                    SpinAnimation.Stop();
//
//                                    if (!string.IsNullOrEmpty(resultAnhang.Error))
//                                    {
//                                        Console.ForegroundColor = ConsoleColor.Red;
//                                        Console.WriteLine("FEHLER: " + resultAnhang.Error);
//                                    }
//                                    else
//                                    {
//                                        Console.ForegroundColor = ConsoleColor.Green;
//                                        Console.WriteLine("Download abgeschlossen (" + resultAnhang.Size + " Bytes)");
//                                    }
//
//                                    Console.ResetColor();
//
//                                    loaderTransaction.Commit();
//                                    x++;
//                                }
//
//                                Console.WriteLine();
//   
//                            }
                        }

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Anzahl Elemente: " + count);
                        Console.ResetColor();
                        Console.WriteLine();
                        if (downloadFiles)
                        {
                            var ervList = DataStoreProvider.DataStore.LoadAll<ErvRueckverkehr>(p => p.RCode == code.Code).OrderByDescending(p => p.AusgangBereitgestelltUebermittlungsStelle).ToList();

                            foreach (var erv in ervList)
                            {
                                if (erv.NumberOfDocuments == 0)
                                    continue;
                            
                                transaction = new Transaction();
                                Console.WriteLine();
                                Console.WriteLine("Lade " + erv.NumberOfDocuments + " Anhänge für " + erv.ZustellungTyp + " " + erv.GerichtsAktenzeichen + " " + erv.RCode + "\r\n" + erv.MessageId);
                                Console.WriteLine();
                                var downloader = new ErvAnhangDownloader(transaction, TypeProvider.Current.GetSingle<IParaDataHttpClient>());

                                var x = 1;
                                foreach (var anhang in erv.ErvAnhangList.OrderBy(p => p.TransactionId).ToList())
                                {
                                    ErvAnhang downloaded = null;
                                    Console.Write("Lade Anhang " + x + "/" + erv.NumberOfDocuments + " " + anhang.DocumentType + "... ");
                                    SpinAnimation.Start();
                                    Task.Run(async () =>
                                    {
                                        downloaded = await downloader.DownloadSingle(anhang, anhang.TransactionId);
                                    }).Wait();

                                    SpinAnimation.Stop();

                                    if (!string.IsNullOrEmpty(anhang.Error))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("FEHLER: " + downloaded.Error);
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Download abgeschlossen (" + downloaded.Size + " Bytes)");
                                    }

                                    Console.ResetColor();

                                    transaction.Commit();
                                    x++;
                                }

                                Console.WriteLine();
                            }
                        }
                    }


                    Console.WriteLine("Abrufen des Rückverkehrs abgeschlossen...");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Abgerufene Elemente:\t\t" + DataStoreProvider.DataStore.LoadAll<ErvRueckverkehr>().Count());
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Bestätigte Elemente:\t\t" + DataStoreProvider.DataStore.LoadAll<ErvRueckverkehr>(p => p.AusgangAbgeholtUebermittlungsstelle.HasValue).Count());
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unbestätigte Elemente:\t\t" + DataStoreProvider.DataStore.LoadAll<ErvRueckverkehr>(p => !p.AusgangAbgeholtUebermittlungsstelle.HasValue).Count());
                    Console.ResetColor();
                }
                else
                    Console.WriteLine("Anmeldung fehlgeschlagen!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Zum Beenden beliebige Taste drücken");

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
