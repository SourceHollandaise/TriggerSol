//
// Player.cs
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
using TriggerSol.Extensions.Types;
using TriggerSol.JStore;

namespace TriggerSol.Game.Model
{
    [PersistentName("PLAYER")]
    public class Player : PersistentBase
    {
        public Player()
        {

        }

        public Player(Session session) : base(session)
        {
        }

        string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue(ref _Name, value); }
        }

        int _Position;
        public int Position
        {
            get { return _Position; }
            set { SetPropertyValue(ref _Position, value); }
        }

        int _Score;
        public int Score
        {
            get { return _Score; }
            set { SetPropertyValue(ref _Score, value); }
        }

        GenericColor _Color;
        public GenericColor Color
        {
            get { return _Color; }
            set { SetPropertyValue(ref _Color, value); }
        }

        Game _Game;
        [Reference]
        public Game Game
        {
            get { return _Game; }
            set { SetPropertyValue(ref _Game, value); }
        }

        public void UpdateScore(int points) => Score += points;
    }
}
