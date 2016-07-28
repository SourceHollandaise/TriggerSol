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

using System;
using System.Collections.Generic;
using System.Linq;
using TriggerSol.JStore;

namespace TriggerSol.Game.Model
{
    [PersistentName("GAME")]
    public class Game : PersistentBase
    {
        string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue(ref _Name, value); }
        }

        int _Rounds;
        public int Rounds
        {
            get { return _Rounds; }
            set { SetPropertyValue(ref _Rounds, value); }
        }

        int _CurrentRound;
        public int CurrentRound
        {
            get { return _CurrentRound; }
            set { SetPropertyValue(ref _CurrentRound, value); }
        }

        int _PointsPerRound;
        public int PointsPerRound
        {
            get { return _PointsPerRound; }
            set { SetPropertyValue(ref _PointsPerRound, value); }
        }

        Player _ActivePlayer;
        [Reference]
        public Player ActivePlayer
        {
            get { return _ActivePlayer; }
            set { SetPropertyValue(ref _ActivePlayer, value); }
        }

        public IList<Player> Players => GetAssociatedCollection<Player>(nameof(Player.Game));

        public IList<Player> Tableau => Players.OrderByDescending(p => p.Points).ToList();

        public void SetPoints()
        {
            ActivePlayer.SetPoints(PointsPerRound);
            ActivePlayer.Save();

            ChangePlayer();
        }

        void ChangePlayer()
        {
            var player = Players.NextIf(ActivePlayer);
            if (player == null)
            {
                ActivePlayer = Players.First();
                CurrentRound++;
            }
            else
                ActivePlayer = player;
        }
    }
}
