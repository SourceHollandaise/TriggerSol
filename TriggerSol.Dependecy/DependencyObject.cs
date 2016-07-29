//
// DependencyObject.cs
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
using TriggerSol.Logging;


namespace TriggerSol.Dependency
{
    public abstract class DependencyObject : IDependencyObject, ILoggable
    {
        IDependencyResolver _DependencyResolver;
        public IDependencyResolver DependencyResolver
        {
            get
            {
                if (_DependencyResolver == null)
                    _DependencyResolver = DependencyResolverProvider.Current;
                return _DependencyResolver;
            }
        }

        ILogger _Logger;
        public ILogger Logger
        {
            get
            {
                if (_Logger == null)
                    _Logger = DependencyResolver.ResolveSingle<ILogger>();
                return _Logger;
            }
        }
    }
}
