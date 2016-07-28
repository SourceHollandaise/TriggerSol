//
// SessionExtenions.cs
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
using System.Runtime.CompilerServices;

namespace TriggerSol.JStore
{
    public static class SessionExtenions
    {
        public static void AddToTransaction(this ISession session, IEnumerable<IPersistentBase> persistents)
        {
            GuardSessionIsNull(session: session);

            foreach (var persistent in persistents)
                session.AddTo(persistent);
        }

        public static T FindObject<T>(this ISession session, Func<T, bool> criteria) where T : IPersistentBase
        {
            GuardSessionIsNull(session: session);

            return session.GetObjects().OfType<T>().FirstOrDefault(criteria);
        }

        public static IEnumerable<T> FindObjects<T>(this ISession session, Func<T, bool> criteria) where T : IPersistentBase
        {
            GuardSessionIsNull(session: session);

            return session.GetObjects().OfType<T>().Where(criteria);
        }

        static void GuardSessionIsNull([CallerMemberName] string method = null, ISession session = null)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session), $"Session must not be null by calling {method}");
        }
    }
}