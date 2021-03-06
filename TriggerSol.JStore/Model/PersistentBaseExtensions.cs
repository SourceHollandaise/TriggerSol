//
// PersistentBaseExtensions.cs
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
using System.Reflection;

namespace TriggerSol.JStore
{
    public static class PersistentBaseExtensions
    {
        public static Dictionary<IPersistentBase, PersistentDeleteBehaviour> GetReferenceObjects(this IPersistentBase persistent)
        {
            var dict = new Dictionary<IPersistentBase, PersistentDeleteBehaviour>();

            foreach (var pInfo in persistent.GetType().GetRuntimeProperties())
            {
                var refAttr = pInfo.FindAttribute<ReferenceAttribute>();
                if (refAttr != null)
                {
                    var refObj = pInfo.GetValue(persistent) as IPersistentBase;
                    if (refObj != null)
                        dict.Add(refObj, refAttr.DeleteBehaviour);
                }
            }

            return dict;
        }

        public static void DeleteReferenceObject(this IPersistentBase persistent, IPersistentBase persistentToDelete, PersistentDeleteBehaviour deleteBehaviour)
        {
            if (deleteBehaviour == PersistentDeleteBehaviour.Delete)
                persistentToDelete.Delete();

            if (deleteBehaviour == PersistentDeleteBehaviour.SetNull)
            {
                var props = persistentToDelete.GetType().GetRuntimeProperties().Where(p => p.GetType() == persistent.GetType());
                foreach (var prop in props)
                {
                    prop.SetValue(persistentToDelete, null);
                    persistentToDelete.Save();
                }
            }

            if (deleteBehaviour == PersistentDeleteBehaviour.Deny)
                throw new InvalidOperationException($"Deletion denied! You can't delete {persistent.ToString()} while reference {persistentToDelete.GetType().FullName} is set to {deleteBehaviour}");
        }
    }
}
