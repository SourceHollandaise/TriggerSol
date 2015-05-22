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
using System.Collections.Generic;
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

namespace XConsole
{
    class Programm
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
                Console.Write("Anmeldung... ");
                SpinAnimation.Start();

                var authServivce = new AuthenticateService(new Transaction(), user, passHash, ServiceEnvironment.StagingUrl);
                User currentUser = null;

                Task.Run(async () =>
                {
                    currentUser = await authServivce.AuthenticateAsync();
                }).Wait();
    
                if (currentUser != null)
                {
                    SpinAnimation.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Anmeldung erfolgreich!");
                    Console.WriteLine("Angemeldet als " + currentUser.UserName);
                    Console.Write("Abfragezeitraum (in Tagen): ");
                    var daysBack = Console.ReadLine();
                    Console.Write("Anhänge herunterladen (J für Downloads): ");
                    bool downloadFiles = Console.ReadLine().ToLowerInvariant() == "j";


                    Console.Write("Daten werden geladen... ");
                    SpinAnimation.Start();

                    DataStoreProvider.ExplicitCaching<ErvRueckverkehr>();
                    DataStoreProvider.ExplicitCaching<ErvAnhang>();

                    SpinAnimation.Stop();

                    Console.WriteLine("Daten aktualisiert!");
                    Console.WriteLine();

                    Console.WriteLine("Taste drücken für Aktualisierung...");
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
                        Console.Write("Posteingang für ERV-Code " + code.Code + " wird abgerufen... ");
             
                        SpinAnimation.Start();

                        Console.ResetColor();

                        Console.WriteLine();
   
                        var ervReceiveService = new ErvReceiveService(code, TypeProvider.Current.GetSingle<IParaDataHttpClient>());
    
                        int count = 0;
                        Task.Run(async () =>
                        {
                            count = await ervReceiveService.GetPaperboxRecentCountAsync(days);
                        }).Wait();
    
    
                        Para.Data.UstOut[] serviceResultSet = null;
    
                        Task.Run(async () =>
                        {
                            serviceResultSet = await ervReceiveService.GetPaperboxRecentAsync(days, count);
                        }).Wait();
    
                        SpinAnimation.Stop();
    
                        var transaction = new Transaction();
    
                        var mapper = new ErvRueckverkehrMapper(transaction);
    
                        List<ErvRueckverkehr> currentErvResultSet = new List<ErvRueckverkehr>();

                        foreach (var data in serviceResultSet.OrderByDescending(p => p.AusgangBereitgestelltUebermittlungsStelle).ToList())
                        {
                            var erv = mapper.Map(data);
                            currentErvResultSet.Add(erv);
    
                            WriteErvRueckverkehrResult(data);
                        }

                        transaction.Commit();
    
                        WriteTotalResult(code);
                        Console.WriteLine();

                        int totalDownloads = 0;

                        if (downloadFiles)
                        {
                            var fileDataService = TypeProvider.Current.GetObject<IFileDataService>();
                            var ervList = currentErvResultSet.OrderByDescending(p => p.AusgangBereitgestelltUebermittlungsStelle).ToList();
    
                            foreach (var erv in ervList)
                            {
                                if (erv.NumberOfDocuments == 0)
                                    continue;
                                
                                transaction = new Transaction();

                                Console.WriteLine();
                                Console.WriteLine("Lade " + erv.NumberOfDocuments + " Anhänge für " + erv.ZustellungTyp + " " + erv.GerichtsAktenzeichen + " " + erv.RCode + "\r\n" + erv.MessageId);
                                Console.WriteLine();

                                var ervAnhangDownloadService = new ErvAnhangDownloadService(transaction, TypeProvider.Current.GetSingle<IParaDataHttpClient>());
    
                                var currentAnhang = 1;
                                foreach (var anhang in erv.ErvAnhangList.OrderBy(p => p.TransactionId).ToList())
                                {
                                    if (fileDataService.Exists(anhang))
                                    {
                                        Console.WriteLine("Anhang " + currentAnhang + "/" + erv.NumberOfDocuments + " " + anhang.DocumentType + " bereits vorhanden... ");
                                    }
                                    else
                                    {
                                        ErvAnhang downloaded = null;
                                        Console.Write("Lade Anhang " + currentAnhang + "/" + erv.NumberOfDocuments + " " + anhang.DocumentType + "... ");
                                        SpinAnimation.Start();

                                        Task.Run(async () =>
                                        {
                                            downloaded = await ervAnhangDownloadService.DownloadSingleAsync(anhang, anhang.TransactionId);
                                        }).Wait();
    
                                        SpinAnimation.Stop();
    
                                        WriterErvAnhangDownloadResult(downloaded);
                                        if (fileDataService.Exists(anhang))
                                            totalDownloads++;
                                    }
                                    currentAnhang++;
                                }

                                transaction.Commit();
    
                                Console.WriteLine();
                            }
                        }

                        Console.WriteLine("Downloads abgeschlossen für " + code.Code + ":");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Anzahl der Anhänge:\t\t" + totalDownloads);
                        Console.WriteLine();
                        Console.ResetColor();
                    }
    
    
                    foreach (var code in currentUser.ErvCodes)
                    {
                        WriteTotalResult(code);
                    }
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

        static void WriteErvRueckverkehrResult(Para.Data.UstOut data)
        {
            Console.WriteLine(data.AusgangBereitgestelltUebermittlungsStelle.Value + "\t" + data.ZustellungTyp + "\r\n\t\t\t" + data.GerichtsAktenzeichen + "\r\n\t\t\t" + data.Partei1 + " " + data.Partei2 + "\r\n\t\t\t" + data.MessageId);
            Console.ForegroundColor = data.AusgangBestaetigtUebermittlungsstelle.HasValue ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine("\t\t\t" + (data.AusgangBestaetigtUebermittlungsstelle.HasValue ? "BESTÄTIGT" : "OFFEN"));
            Console.WriteLine();
            Console.ResetColor();
        }

        static void WriterErvAnhangDownloadResult(ErvAnhang anhang)
        {
            if (!string.IsNullOrEmpty(anhang.Error))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("FEHLER: " + anhang.Error);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Download abgeschlossen (" + anhang.Size + " Bytes)");
            }
           
            Console.ResetColor();
        }

        static void WriteTotalResult(ErvCode code)
        {
            Console.WriteLine("Abrufen des Rückverkehrs abgeschlossen für " + code.Code + ":");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Posteingang gesamt:\t\t" + DataStoreProvider.DataStore.LoadAll<ErvRueckverkehr>(p => p.RCode == code.Code).Count());
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Bestätigte Elemente:\t\t" + DataStoreProvider.DataStore.LoadAll<ErvRueckverkehr>(p => p.RCode == code.Code && p.AusgangAbgeholtUebermittlungsstelle.HasValue).Count());
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Unbestätigte Elemente:\t\t" + DataStoreProvider.DataStore.LoadAll<ErvRueckverkehr>(p => p.RCode == code.Code && !p.AusgangAbgeholtUebermittlungsstelle.HasValue).Count());
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
