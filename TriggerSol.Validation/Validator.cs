//
// Validator.cs
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

namespace TriggerSol.Validation
{
    public class Validator
    {
        public IList<RuleResult> GetResult(object obj)
        {
            var rules = RuleManager.GetRulesForType(obj.GetType());
            var validationResult = new List<RuleResult>();

            if (!rules.Any())
                return validationResult;
            
            foreach (var rule in rules)
            {
                var result = new RuleResult
                {
                    Valid = true
                };

                var propInfo = obj.GetType().GetRuntimeProperty(rule.Property);

                var value = propInfo.GetValue(obj);

                if (rule is RuleRequired)
                {
                    result.Valid = value != null;
                }

                if (rule is RuleRange)
                {
                    var range = rule as RuleRange;

                    if (range.Min.GetType() != range.Max.GetType())
                        throw new ArgumentException("Min and Max are not of equal type!");

                    /*
                    dynamic cMin = Convert.ChangeType(range.Min, propInfo.PropertyType);

                    dynamic cMax = Convert.ChangeType(range.Max, propInfo.PropertyType);
                    */

                    result.Valid = value >= (dynamic)range.Min && value <= (dynamic)range.Max;
                }

                validationResult.Add(result);
            }

            return validationResult;
        }

        public bool IsValid(object obj)
        {
            return GetResult(obj).All(p => p.Valid);
        }
    }
}
