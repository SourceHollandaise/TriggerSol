﻿//
// ErvReceiveService.cs
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
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Para.Data;
using Para.Data.Client;
using TriggerSol.Dependency;
using Uvst.Model;

namespace Uvst.Domain
{
    public class ErvReceiveService : DependencyObject
    {
        ErvCode _code;
        ParaDataHttpClient _client;

        public ErvReceiveService(ErvCode code, IParaDataHttpClient client)
        {
            this._code = code;
            this._client = client as ParaDataHttpClient;
        }

        #region Paperbox recent

        public async Task<int> GetPaperboxRecentCount(int days)
        {
            return await _client.GetPaperboxRecentCountAsync(_code.Code, DateTime.Today.AddDays(days), _code.CodeId, new string[] { "ALL" });
        }

        public async Task<UstOut[]> GetPaperboxRecent(int days, int count)
        {
            return await _client.GetPaperboxRecentAsync(_code.Code, DateTime.Today.AddDays(days), _code.CodeId, new string[] { "ALL" }, count);
        }

        #endregion

        #region Paperbox unconfirmed

        public async Task<int> GetPaperboxUconfirmedCount(int days)
        {
            return await _client.GetPaperboxUnconfirmedCountAsync(_code.Code, DateTime.Today.AddDays(days), _code.CodeId, new string[] { "ALL" });
        }

        public async Task<UstOut[]> GetPaperboxUnconfirmed(int days, int count)
        {
            return await _client.GetPaperboxUnconfirmedAsync(_code.Code, DateTime.Today.AddDays(days), _code.CodeId, new string[] { "ALL" }, count);
        }

        #endregion

        #region Paperbox confirmed

        public async Task<UstIsConfirmed[]> GetPaperboxIsConfirmed(int[] ids)
        {
            return await _client.GetPaperboxIsConfirmedAsync(_code.Code, _code.CodeId, ids);
        }

        public async Task<UstOut> GetPaperboxLastConfirmed()
        {
            return await _client.GetPaperboxLastConfirmedAsync(_code.Code, _code.CodeId);
        }

        #endregion

        #region Download documents

        public async Task<UstDocumentMetadata> GetDocumentMetadata(int transactionId)
        {
            return await _client.GetCourtDocumentMetadataAsync(_code.Code, _code.CodeId, transactionId);
        }

        public async Task<Stream> GetDocument(int transactionId, int index, IProgress<double> progress = null)
        {
            var url = _client.BaseAddress.AbsoluteUri + "api/CourtDocument?ervCode=" + _code.Code + "&subUserOid=" + _code.CodeId + "&transactionId=" + transactionId + "&index=" + index;

            return await _client.DownloadFileAsync(url, new CancellationToken(), progress);
        }

        #endregion
    }
}
