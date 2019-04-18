//
// Game.cs
//
// Author:
//       Jörg Egger <joerg.egger@outlook.de>
//
// Copyright (c) 2016 Jörg Egger
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

using System.Collections.Generic;
using System.Linq;
using TriggerSol.JStore;

namespace TriggerSol.Game.Model
{
    [PersistentName("game")]
    public class Game : PersistentBase, IGameSettings
    {
        public Game()
        {

        }

        public Game(Session session) : base(session)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            CurrentRound = 1;
            GameType = GameType.Round;
        }

        GameType _GameType;
        public GameType GameType
        {
            get => _GameType;
            set => SetPropertyValue(ref _GameType, value);
        }

        string _Name;
        public string Name
        {
            get => _Name;
            set => SetPropertyValue(ref _Name, value);
        }

        string _Description;
        public string Description
        {
            get => _Description;
            set => SetPropertyValue(ref _Description, value);
        }

        int _TotalRounds;
        public int TotalRounds
        {
            get => _TotalRounds;
            set => SetPropertyValue(ref _TotalRounds, value);
        }

        int _CurrentRound;
        public int CurrentRound
        {
            get => _CurrentRound;
            set => SetPropertyValue(ref _CurrentRound, value);
        }

        int _MinScorePerRound;
        public int MinScorePerRound
        {
            get => _MinScorePerRound;
            set => SetPropertyValue(ref _MinScorePerRound, value);
        }

        int _MaxScorePerRound;
        public int MaxScorePerRound
        {
            get => _MaxScorePerRound;
            set => SetPropertyValue(ref _MaxScorePerRound, value);
        }

        int _MaxScoreTotal;
        public int MaxScoreTotal
        {
            get => _MaxScoreTotal;
            set => SetPropertyValue(ref _MaxScoreTotal, value);
        }

        Player _ActivePlayer;
        [Reference]
        public Player ActivePlayer
        {
            get => _ActivePlayer;
            set => SetPropertyValue(ref _ActivePlayer, value);
        }

        public IList<Player> Players => GetAssociatedCollection<Player>(nameof(Player.Game));

        public IList<Player> Tableau => Players?.OrderByDescending(p => p.Score).ToList();

        public virtual void Start() => ActivePlayer = Players?.OrderBy(p => p.Position).FirstOrDefault();

        public virtual void Stop() => CurrentRound = TotalRounds;

        public bool IsLastPlayerInRound => Players.Select(p => p.Position).Max() == ActivePlayer.Position;

        public void NextPlayer()
        {
            if (CurrentRound > TotalRounds)
                return;
            
            var player = Players?.FirstOrDefault(p => p.Position == ActivePlayer.Position + 1);
            if (player == null)
            {
                ActivePlayer = Players?.FirstOrDefault();
                CurrentRound++;
            }
            else
                ActivePlayer = player;
        }

        public virtual void UpdatePlayerScore(int points) => ActivePlayer?.UpdateScore(points);
    }
}
