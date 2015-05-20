﻿//
// User.cs
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
    [PersistentName("USER")]
    public class User : PersistentBase
    {
        string _UserName;

        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                SetPropertyValue(ref _UserName, value);
            }
        }

        string _Password;

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                SetPropertyValue(ref _Password, value);
            }
        }

        int _UserId;

        public int UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                SetPropertyValue(ref _UserId, value);
            }
        }

        string _ApiKey;

        public string ApiKey
        {
            get
            {
                return _ApiKey;
            }
            set
            {
                SetPropertyValue(ref _ApiKey, value);
            }
        }

        bool _IsAuthenticated;

        public bool IsAuthenticated
        {
            get
            {
                return _IsAuthenticated;
            }
            set
            {
                SetPropertyValue(ref _IsAuthenticated, value);
            }
        }

        public IList<ErvCode> ErvCodes
        {
            get
            {
                return GetAssociatedCollection<ErvCode>(Fields<ErvCode>.GetName(p => p.User));
            }
        }
    }
}

