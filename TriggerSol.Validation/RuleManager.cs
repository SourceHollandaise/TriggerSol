//
// RuleManager.cs
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

namespace TriggerSol.Validation
{
    public static class RuleManager
    {
        static Dictionary<Type, IList<IRule>> _rules = new Dictionary<Type, IList<IRule>>();

        public static void AddRules(Type type, IEnumerable<IRule> rules)
        {
            if (!_rules.ContainsKey(type))
                _rules.Add(type, new List<IRule>());

            foreach (var rule in rules)
            {
                if (rule.TargetType == null)
                    rule.TargetType = type;
                
                var exists = _rules[type].FirstOrDefault(p => p.RuleId == rule.RuleId);

                if (exists == null)
                    _rules[type].Add(rule);
            }
        }

        public static void CleanRules(Type type)
        {
            if (_rules.ContainsKey(type))
                _rules.Remove(type);
        }

        public static IList<IRule> GetRulesForType(Type type)
        {
            return !_rules.ContainsKey(type) ? new List<IRule>() : _rules[type];
        }
    }
}
