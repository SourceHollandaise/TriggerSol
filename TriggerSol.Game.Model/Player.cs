﻿//
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
using TriggerSol.JStore;

namespace TriggerSol.Game.Model
{
    [PersistentName("PLAYER")]
    public class Player : PersistentBase
    {
        string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue(ref _Name, value); }
        }

        Game _Game;
        [Reference]
        public Game Game
        {
            get { return _Game; }
            set { SetPropertyValue(ref _Game, value); }
        }

        int _Position;
        public int Position
        {
            get { return _Position; }
            set { SetPropertyValue(ref _Position, value); }
        }

        int _Round;
        public int Round
        {
            get { return _Round; }
            set { SetPropertyValue(ref _Round, value); }
        }

        int _Points;
        public int Points
        {
            get { return _Points; }
            set { SetPropertyValue(ref _Points, value); }
        }

        public void SetPoints(int points)
        {
            Points += points;
            Round++;
        }
    }
}