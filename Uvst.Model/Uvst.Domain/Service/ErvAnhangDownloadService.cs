//
// ErvAnhangDownloadService.cs
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
using System.Threading.Tasks;
using Para.Data.Client;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using Uvst.Model;

namespace Uvst.Domain
{
    public class ErvAnhangDownloadService : DependencyObject
    {
        ITransaction _transaction;
        ParaDataHttpClient _client;
        ErvReceiveService _service;
        IFileDataService _fileDataService;

        public ErvAnhangDownloadService(ITransaction transaction, IParaDataHttpClient client)
        {
            this._transaction = transaction;
            this._client = client as ParaDataHttpClient;
            this._fileDataService = TypeResolver.GetObject<IFileDataService>();
        }

        public async Task<IList<ErvAnhang>> DownloadAllAsync(ErvRueckverkehr erv)
        {
            var list = new List<ErvAnhang>();

            foreach (var anhang in erv.ErvAnhangList.OrderBy(p => p.TransactionId).ToList())
            {
                var result = await DownloadSingleAsync(anhang, anhang.TransactionId);

                list.Add(result);
            }
            return list;
        }

        public async Task<ErvAnhang> DownloadSingleAsync(ErvAnhang anhang, int index)
        {
            InitializReceiveService(anhang.Rueckverkehr);

            if (anhang.IsDownloaded)
                return anhang;

            _transaction.AddTo(anhang);

            anhang.FileName = await GetFilePath(anhang.Rueckverkehr, anhang);

            return anhang;
        }

        async Task<string> GetFilePath(ErvRueckverkehr erv, ErvAnhang anhang)
        {
            try
            {
                anhang.Error = null;
                anhang.IsDownloaded = true;

                var stream = await _service.GetDocumentStreamAsync(erv.Id, anhang.TransactionId);

                var fileName = erv.MessageId.Replace("mid://", "");

                return await _fileDataService.GetAsync(stream, ".pdf", fileName + "_" + anhang.TransactionId);
            }
            catch (Exception ex)
            {
                anhang.Error = ex.Message;
                anhang.FileName = null;
                anhang.IsDownloaded = false;
                return null;
            }
        }

        void InitializReceiveService(ErvRueckverkehr erv)
        {
            var ervCode = DataStoreProvider.DataStore.Load<ErvCode>(p => p.Code == erv.RCode);
            _service = new ErvReceiveService(ervCode, _client);
        }
    }
}
