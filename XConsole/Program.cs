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
using System.IO;
using System.Linq;
using TriggerSol.Boost;
using TriggerSol.Dependency;
using TriggerSol.Game.Model;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace XConsole
{
    class Programm
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                if (e.ExceptionObject is Exception)
                    DependencyResolverProvider.Current.ResolveSingle<ILogger>().LogException(e.ExceptionObject as Exception);
            };

            Console.WriteLine("Init Booster...");
            TriggerSol.Console.Spinner.Start(150);
            System.Threading.Thread.Sleep(1000);
            TriggerSol.Console.Spinner.Stop();

            InitBooster();

            Console.WriteLine("Datastore is ready!");

            string name = null;
            using (var session = new Session())
            {
                Console.Write("Name the game: ");
                name = Console.ReadLine();

                Console.Write("Rounds: ");
                var rounds = int.Parse(Console.ReadLine());

                Console.Write("Max. points: ");
                var points = int.Parse(Console.ReadLine());

                Console.Write("Players: ");
                var players = Console.ReadLine().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToArray();

                new TestData().Create(session, name, rounds, points, players);
                session.Commit();
            }

            Console.ReadKey();

            Game game;
            using (var session = new Session())
            {
                game = session.Load<Game>(p => p.Name == name);
                game.Start();
                Console.WriteLine($"{game.Name} starts now!");
                Console.WriteLine(game.Description);
                Console.WriteLine("Players:");
                ShowTableau(game);
                Console.WriteLine();

                Random r = new Random(1);

                while (game.CurrentRound <= game.TotalRounds)
                {
                    ShowCurrentPlayer(game, r.Next(2, 12));
                    game.NextPlayer();
                }

                Console.WriteLine($"Winner: {game.Tableau.First().Name}");

                ShowTableau(game);

                session.Rollback();

                Console.ReadKey();
            }
        }

        static void ShowCurrentPlayer(Game game, int value)
        {
            Console.WriteLine($"Round {game.CurrentRound} / {game.TotalRounds}");
            Console.WriteLine($"Active player: {game.ActivePlayer.Name}");
            Console.WriteLine("Roll the dice now!");
            Console.ReadKey();

            TriggerSol.Console.Spinner.Start(100);
            System.Threading.Thread.Sleep(350);
            TriggerSol.Console.Spinner.Stop();

            game.UpdatePlayerScore(value);

            Console.WriteLine($"Points for {game.ActivePlayer.Name} {value}");
            Console.WriteLine($"Total points: {game.ActivePlayer.Score}");

            Console.WriteLine();
            Console.WriteLine($"Tableau after {game.CurrentRound} / {game.TotalRounds} rounds:");

            ShowTableau(game);

            Console.WriteLine();
        }

        static void InitBooster()
        {
            var booster = new Booster(LogLevel.OnlyException);
            booster.RegisterLogger<FileLogger>();
            booster.InitDataStore<CachedFileDataStore>(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TriggerSol Database"));
        }

        static void ShowTableau(Game game)
        {
            foreach (var item in game.Tableau.Select(p => new { Name = p.Name, Points = p.Score }).OrderByDescending(p => p.Points))
                Console.WriteLine($"{item.Name} {item.Points}");
        }
    }

    class TestData
    {
        public Game Create(ISession session, string name, int rounds, int points, params string[] players)
        {
            var template = session.Load<GameTemplate>(p => p.Name == name);
            if (template == null)
            {
                template = session.CreateObject<GameTemplate>();
                template.Name = name;
                template.PointsPerRound = points;
                template.TotalRounds = rounds;
                template.Description = $"{template.Name} Rounds: {template.TotalRounds} Max. points per round: {template.PointsPerRound}";
            }

            return template.Create(players);
        }
    }
}
