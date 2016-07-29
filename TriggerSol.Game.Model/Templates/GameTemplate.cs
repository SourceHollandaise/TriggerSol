//
// GameTemplate.cs
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
using System.Linq;
using TriggerSol.JStore;

namespace TriggerSol.Game.Model
{

    [PersistentName("game_template")]
    public class GameTemplate : PersistentBase, IGame
    {
        public GameTemplate()
        {

        }

        public GameTemplate(Session session) : base(session)
        {
        }

        GameType _GameType;
        public GameType GameType
        {
            get { return _GameType; }
            set { SetPropertyValue(ref _GameType, value); }
        }

        string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue(ref _Name, value); }
        }

        string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(ref _Description, value); }
        }

        string _Author;
        public string Author
        {
            get { return _Author; }
            set { SetPropertyValue(ref _Author, value); }
        }

        string _Copyright;
        public string Copyright
        {
            get { return _Copyright; }
            set { SetPropertyValue(ref _Copyright, value); }
        }

        int _TotalRounds;
        public int TotalRounds
        {
            get { return _TotalRounds; }
            set { SetPropertyValue(ref _TotalRounds, value); }
        }

        int _MinScorePerRound;
        public int MinScorePerRound
        {
            get { return _MinScorePerRound; }
            set { SetPropertyValue(ref _MinScorePerRound, value); }
        }

        int _MaxScorePerRound;
        public int MaxScorePerRound
        {
            get { return _MaxScorePerRound; }
            set { SetPropertyValue(ref _MaxScorePerRound, value); }
        }

        int _MaxScoreTotal;
        public int MaxScoreTotal
        {
            get { return _MaxScoreTotal; }
            set { SetPropertyValue(ref _MaxScoreTotal, value); }
        }

        public Game Create(params string[] players) => new GameFactory(Session, this).Create(players);
    }
}
