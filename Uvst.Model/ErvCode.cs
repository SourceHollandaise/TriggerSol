//
// ErvCode.cs
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
using System.Linq;
using TriggerSol.JStore;
using System.Collections.Generic;

namespace Uvst.Model
{
    [PersistentName("ERV_CODE")]
    public class ErvCode : PersistentBase
    {
        string _Code;

        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetPropertyValue(ref _Code, value);
            }
        }

        int _CodeId;

        public int CodeId
        {
            get
            {
                return _CodeId;
            }
            set
            {
                SetPropertyValue(ref _CodeId, value);
            }
        }

        bool _IsActive;

        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                SetPropertyValue(ref _IsActive, value);
            }
        }

        User _User;

        public User User
        {
            get
            {
                return _User;
            }
            set
            {
                SetPropertyValue(ref _User, value);
            }
        }
    }
}
