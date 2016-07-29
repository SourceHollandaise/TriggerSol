//
// GameFactory.cs
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
    public class GameFactory
    {
        readonly ISession _Session;
        readonly GameTemplate _Template;

        public GameFactory(ISession session, GameTemplate template)
        {
            _Session = session;
            _Template = template;
        }

        public Game Create(params string[] players)
        {
            if (players == null || players.Length == 0)
                throw new ArgumentNullException(nameof(players), "Players must not be null!");

            var game = _Session.CreateObject<Game>();

            AutoMapper.Mapper.Initialize((c) =>
            {
                c.CreateMap<IGame, IGame>();
            });
            AutoMapper.Mapper.Map<IGame, IGame>(_Template, game);

            for (int i = 0; i < players.Length; i++)
            {
                var player = _Session.CreateObject<Player>();
                player.Position = i + 1;
                player.Name = players[i];
                player.Game = game;
            }

            return game;
        }
    }
}
