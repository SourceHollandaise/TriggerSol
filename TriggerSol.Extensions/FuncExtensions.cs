//
// FuncExtensions.cs
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

using System.Collections.Generic;
using System.Reflection;

namespace System
{
    public static class FuncExtensions
    {
        public enum GroupType
        {
            And,
            Or
        }

        public static Func<T, bool> AndAlso<T>(this Func<T, bool> func1, Func<T, bool> func2)
        {
            if (func1 == null)
                throw new ArgumentNullException("func1");

            if (func2 == null)
                throw new ArgumentNullException("func2");

            return a => func1(a) && func2(a);
        }

        public static Func<T, bool> OrElse<T>(this Func<T, bool> func1, Func<T, bool> func2)
        {
            if (func1 == null)
                throw new ArgumentNullException("func1");

            if (func2 == null)
                throw new ArgumentNullException("func2");

            return a => func1(a) || func2(a);
        }

        public static Func<T, bool> Grouped<T>(Func<T, bool>[] functions, GroupType groupType = GroupType.Or)
        {
            if (functions == null)
                throw new ArgumentNullException("functions");

            if (functions.Length == 0)
                throw new ArgumentOutOfRangeException("functions");

            Func<T, bool> result = a => groupType == GroupType.And;

            if (groupType == GroupType.And)
            {
                foreach (var func in functions)
                {
                    result = result.AndAlso(func);
                }
            }

            if (groupType == GroupType.Or)
            {
                foreach (var func in functions)
                {
                    result = result.OrElse(func);
                }
            }

            return result;
        }
    }
}
   
