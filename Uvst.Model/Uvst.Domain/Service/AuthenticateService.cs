//
// AuthenticateService.cs
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
using System.Threading.Tasks;
using Para.Data;
using Para.Data.Client;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using Uvst.Model;

namespace Uvst.Domain
{
    public class AuthenticateService : DependencyObject
    {
        string _username, _password, _serviceUrl;

        ITransaction _transaction;

        IParaDataHttpClient _client;

        public AuthenticateService(ITransaction transaction, string username, string password, string serviceUrl)
        {
            this._transaction = transaction;
            this._username = username;
            this._password = password;
            this._serviceUrl = serviceUrl;
        }

        public async Task<User> AuthenticateAsync()
        {
            var user = GetUser();

            var login = CreateLoginParameter(user);

            ApiKey result = null;

            try
            {
                _client = new ParaDataHttpClient(_serviceUrl);

                result = await _client.PostLoginAsync(login);

                _client.SetApiKeyHeaderAndUserOidToken(result.Apikey, result.UserId);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            user.ApiKey = result.Apikey;
            user.UserId = result.UserId;

            foreach (var item in result.ErvCodes)
            {
                ErvCode code = _transaction.LoadObject<ErvCode>(p => p.Code == item.Key) ?? _transaction.CreateObject<ErvCode>();
                code.Code = item.Key;
                code.CodeId = item.Value;
                code.User = user;
                code.IsActive = true;
            }

            _transaction.Commit();

            TypeResolver.RegisterSingle<IParaDataHttpClient>(_client);

            return user;
        }

        User GetUser()
        {
            var user = _transaction.LoadObject<User>(p => p.UserName == _username && p.Password == _password);

            if (user == null)
            {
                user = _transaction.CreateObject<User>();
                user.UserName = _username;
                user.Password = _password;
                user.IsAuthenticated = false;
            }

            return user;
        }

        LoginParameter CreateLoginParameter(User user)
        {
            return new LoginParameter
            {
                UserName = user.UserName,
                PasswordHash = user.Password,
                SoftwareName = "UVST_ALPHA",
                DeviceName = "DeviceName",
                OperatingSystem = "OS",
                SoftwareApiKey = ServiceEnvironment.ApiKeyIOS,
            };
        }
    }
}

