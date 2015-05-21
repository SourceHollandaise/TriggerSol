//
// ErvAnhang.cs
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
using TriggerSol.JStore;

namespace Uvst.Model
{
    [PersistentName("ERV_ANHANG")]
    public class ErvAnhang : PersistentBase, IFileData
    {
        public override void Initialize()
        {
            base.Initialize();

            CreationDate = DateTime.Now;
        }

        string _Error;

        public string Error
        {
            get
            {
                return _Error;
            }
            set
            {
                SetPropertyValue(ref _Error, value);
            }
        }

        ErvRueckverkehr _Rueckverkehr;

        public ErvRueckverkehr Rueckverkehr
        {
            get
            {
                return _Rueckverkehr;
            }
            set
            {
                SetPropertyValue(ref _Rueckverkehr, value);
            }
        }

        bool _IsDownloaded;

        public bool IsDownloaded
        {
            get
            {
                return _IsDownloaded;
            }
            set
            {
                SetPropertyValue(ref _IsDownloaded, value);

            }
        }

        string _AnhangId;

        public string AnhangId
        {
            get
            {
                return _AnhangId;
            }
            set
            {
                SetPropertyValue(ref _AnhangId, value);
            }
        }

        int _TransactionId;

        public int TransactionId
        {
            get
            {
                return _TransactionId;
            }
            set
            {
                SetPropertyValue(ref _TransactionId, value);
            }
        }

        string _ReferenzId;

        public string ReferenzId
        {
            get
            {
                return _ReferenzId;
            }
            set
            {
                SetPropertyValue(ref _ReferenzId, value);
            }
        }

        string _Subject;

        public string Subject
        {
            get
            {
                return _Subject;
            }
            set
            {
                SetPropertyValue(ref _Subject, value);
            }
        }

        public string Information
        {
            get
            {
                return null;
            }
        }

        string _DocumentType;

        public string DocumentType
        {
            get
            {
                return _DocumentType;
            }
            set
            {
                SetPropertyValue(ref _DocumentType, value);
            }
        }

        string _FileName;

        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                SetPropertyValue(ref _FileName, value);
            }
        }

        long _Size;

        public long Size
        {
            get
            {
                return _Size;
            }
            set
            {
                SetPropertyValue(ref _Size, value);
            }
        }

        string _MimeType;

        public string MimeType
        {
            get
            {
                return _MimeType;
            }
            set
            {
                SetPropertyValue(ref _MimeType, value);
            }
        }

        DateTime _CreationDate;

        public DateTime CreationDate
        {
            get
            {
                return _CreationDate;
            }
            set
            {
                SetPropertyValue(ref _CreationDate, value);
            }
        }
    }
}

