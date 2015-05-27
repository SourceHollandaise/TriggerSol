//
// HttpClientExtensions.cs
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

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<Stream> DownloadFileAsync(this HttpClient client, string uri, CancellationToken token, IProgress<double> progress = null)
        {
            var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
            }

            var length = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                MemoryStream memoryStream = null;

                bool nolength = (length == -1);

                int size = ((nolength) ? 8192 : (int)length);

                if (nolength)
                    memoryStream = new MemoryStream();

                long total = 0;
                int nread = 0;
                int offset = 0;

                byte[] buffer = new byte [size];

                token.ThrowIfCancellationRequested();

                while ((nread = await stream.ReadAsync(buffer, offset, size, token)) != 0)
                {
                    if (nolength)
                    {
                        memoryStream.Write(buffer, 0, nread);
                    }
                    else
                    {
                        offset += nread;
                        size -= nread;
                    }
                    total += nread;

                    if (progress != null)
                        progress.Report((double)offset);

                    token.ThrowIfCancellationRequested();
                }

                return nolength ? memoryStream : new MemoryStream(buffer);
            }
        }
    }
}
