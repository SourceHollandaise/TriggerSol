//
// ErvRueckverkehrMapper.cs
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
using Para.Data;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using Uvst.Model;

namespace Uvst.Domain
{
    public class ErvRueckverkehrMapper : DependencyObject
    {
        IDataTransaction _transaction;

        ErvReceiveLog _log;

        public ErvRueckverkehrMapper(IDataTransaction transaction)
        {
            this._transaction = transaction;

            SetLog();
        }

        public ErvRueckverkehr Map(UstOut paperbox)
        {
            var erv = _transaction.LoadObject<ErvRueckverkehr>(p => p.MessageId == paperbox.MessageId) ?? _transaction.CreateObject<ErvRueckverkehr>();

            AutoMapper.Mapper.CreateMap<UstOut, ErvRueckverkehr>();
            AutoMapper.Mapper.Map<UstOut, ErvRueckverkehr>(paperbox, erv);

            erv.IsNew = erv.IsNewObject;

            if (!erv.AusgangAbeholtLokal.HasValue)
                erv.AusgangAbeholtLokal = DateTime.Now;

            if (erv.IsNewObject)
                AddMetataData(paperbox, erv);

            if (erv.ReceiveLog == null)
            {
                _log.ErvCode = _transaction.LoadObject<ErvCode>(p => p.Code == erv.RCode);
                erv.ReceiveLog = _log;
            }

            Logger.Log(string.Format("Erv {0} mapped!", erv.MessageId));

            return erv;
        }

        void AddMetataData(UstOut paperbox, ErvRueckverkehr erv)
        {
            if (erv.NumberOfDocuments == 0)
                return;

            if (paperbox.DocumentMetadata == null)
                return;

            var files = paperbox.DocumentMetadata.Items;

            for (int i = 0; i < files.Length; i++)
            {
                var anhang = _transaction.CreateObject<ErvAnhang>();
                anhang.Rueckverkehr = erv;
                anhang.IsDownloaded = false;
                anhang.Size = files[i].Size;
                anhang.DocumentType = files[i].DocumentType;
                anhang.AnhangId = files[i].AnhangId;
                anhang.MimeType = files[i].MimeType;
                anhang.ReferenzId = files[i].ReferenzId;
                anhang.TransactionId = i;
            }

            Logger.Log(string.Format("Metadata for {0} mapped!", erv.MessageId));
        }

        void SetLog()
        {
            if (_log == null)
            {
                _log = _transaction.CreateObject<ErvReceiveLog>();
                _log.Date = DateTime.Now;
            }
        }
    }
}

