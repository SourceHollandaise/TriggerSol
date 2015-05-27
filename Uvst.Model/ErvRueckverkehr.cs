//
// ErvRueckverkehr.cs
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
using TriggerSol.JStore;

namespace Uvst.Model
{
    [PersistentName("ERV_RUECKVERKEHR")]
    public class ErvRueckverkehr : PersistentBase
    {
        string _Aktenzeichen;

        public string Aktenzeichen
        {
            get
            {
                return _Aktenzeichen;
            }
                
            set
            {
                SetPropertyValue(ref _Aktenzeichen, value);
            }
        }

        DateTime _AngelegtAm;

        public DateTime AngelegtAm
        {
            get
            {
                return _AngelegtAm;
            }
            set
            {
                SetPropertyValue(ref _AngelegtAm, value);
            }
        }

        string _Anwendung;

        public string Anwendung
        {
            get
            {
                return _Anwendung;
            }
            set
            {
                SetPropertyValue(ref _Anwendung, value);
            }
        }

        DateTime? _AusgangAbeholtRemote;

        public DateTime? AusgangAbeholtRemote
        {
            get
            {
                return _AusgangAbeholtRemote;
            }
            set
            {
                SetPropertyValue(ref _AusgangAbeholtRemote, value);
            }
        }

        DateTime? _AusgangAbeholtLokal;

        public DateTime? AusgangAbeholtLokal
        {
            get
            {
                return _AusgangAbeholtLokal;
            }
            set
            {
                SetPropertyValue(ref _AusgangAbeholtLokal, value);
            }
        }

        DateTime? _AusgangAbgeholtUebermittlungsstelle;

        public DateTime? AusgangAbgeholtUebermittlungsstelle
        {
            get
            {
                return _AusgangAbgeholtUebermittlungsstelle;
            }
            set
            {
                SetPropertyValue(ref _AusgangAbgeholtUebermittlungsstelle, value);
            }
        }

        DateTime? _AusgangBereitgestelltUebermittlungsStelle;

        public DateTime? AusgangBereitgestelltUebermittlungsStelle
        {
            get
            {
                return _AusgangBereitgestelltUebermittlungsStelle;
            }
            set
            {
                SetPropertyValue(ref _AusgangBereitgestelltUebermittlungsStelle, value);
            }
        }

        DateTime? _AusgangBestaetigtRemote;

        public DateTime? AusgangBestaetigtRemote
        {
            get
            {
                return _AusgangBestaetigtRemote;
            }
            set
            {
                SetPropertyValue(ref _AusgangBestaetigtRemote, value);
            }
        }

        DateTime? _AusgangBestaetigtUebermittlungsstelle;

        public DateTime? AusgangBestaetigtUebermittlungsstelle
        {
            get
            {
                return _AusgangBestaetigtUebermittlungsstelle;
            }
            set
            {
                SetPropertyValue(ref _AusgangBestaetigtUebermittlungsstelle, value);
            }
        }

        string _Dienststelle;

        public string Dienststelle
        {
            get
            {
                return _Dienststelle;
            }
            set
            {
                SetPropertyValue(ref _Dienststelle, value);
            }
        }

        string _ErrorCode;

        public string ErrorCode
        {
            get
            {
                return _ErrorCode;
            }
            set
            {
                SetPropertyValue(ref _ErrorCode, value);
            }
        }

        string _ErrorMessage;

        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
            set
            {
                SetPropertyValue(ref _ErrorMessage, value);
            }
        }

        string _GerichtsAktenzeichen;

        public string GerichtsAktenzeichen
        {
            get
            {
                return _GerichtsAktenzeichen;
            }
            set
            {
                SetPropertyValue(ref _GerichtsAktenzeichen, value);
            }
        }

        int _Id;

        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                SetPropertyValue(ref _Id, value);
            }
        }

        bool _IsNew;

        public bool IsNew
        {
            get
            {
                return _IsNew;
            }
            set
            {
                SetPropertyValue(ref _IsNew, value);
            }
        }

        string _MessageId;

        public string MessageId
        {
            get
            {
                return _MessageId;
            }
            set
            {
                SetPropertyValue(ref _MessageId, value);
            }
        }

        DateTime? _NachweisGesendetRemote;

        public DateTime? NachweisGesendetRemote
        {
            get
            {
                return _NachweisGesendetRemote;
            }
            set
            {
                SetPropertyValue(ref _NachweisGesendetRemote, value);
            }
        }

        string _NachweisMessageIdRemote;

        public string NachweisMessageIdRemote
        {
            get
            {
                return _NachweisMessageIdRemote;
            }
            set
            {
                SetPropertyValue(ref _NachweisMessageIdRemote, value);
            }
        }

        bool _NachweisZusendenRemote;

        public bool NachweisZusendenRemote
        {
            get
            {
                return _NachweisZusendenRemote;
            }
            set
            {
                SetPropertyValue(ref _NachweisZusendenRemote, value);
            }
        }

        int _NumberOfDocuments;

        public int NumberOfDocuments
        {
            get
            {
                return _NumberOfDocuments;
            }
            set
            {
                SetPropertyValue(ref _NumberOfDocuments, value);
            }
        }

        string _Partei1;

        public string Partei1
        {
            get
            {
                return _Partei1;
            }
            set
            {
                SetPropertyValue(ref _Partei1, value);
            }
        }

        string _Partei2;

        public string Partei2
        {
            get
            {
                return _Partei2;
            }
            set
            {
                SetPropertyValue(ref _Partei2, value);
            }
        }

        string _RCode;

        public string RCode
        {
            get
            {
                return _RCode;
            }
            set
            {
                SetPropertyValue(ref _RCode, value);
            }
        }

        string _TransactionFile;

        public string TransactionFile
        {
            get
            {
                return _TransactionFile;
            }
            set
            {
                SetPropertyValue(ref _TransactionFile, value);
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

        string _ZustellungTyp;

        public string ZustellungTyp
        {
            get
            {
                return _ZustellungTyp;
            }
            set
            {
                SetPropertyValue(ref _ZustellungTyp, value);
            }
        }

        ErvReceiveLog _ReceiveLog;

        public ErvReceiveLog ReceiveLog
        {
            get
            {
                return _ReceiveLog;
            }
            set
            {
                SetPropertyValue(ref _ReceiveLog, value);
            }
        }

        public IList<ErvAnhang> ErvAnhangList
        {
            get
            {
                return GetAssociatedCollection<ErvAnhang>(Fields<ErvAnhang>.GetName(p => p.Rueckverkehr));
            }
        }
    }
}
