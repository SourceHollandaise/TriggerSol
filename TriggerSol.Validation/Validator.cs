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
    public class Validator : IValidator
    {
        public bool IsValid(object obj) => Result(obj).All(p => p.Valid);
        
        public bool IsTargetValidForRule(IRule rule, object obj) => ResultForRule(rule, obj).Valid;
        
        public IList<ValidationResult> Result(object obj)
        {
            var registeredRules = RuleManager.GetRulesForType(obj.GetType());
            var resultList = new List<ValidationResult>();

            if (!registeredRules.Any())
                return resultList;
            
            foreach (var rule in registeredRules)
                resultList.Add(ResultForRule(rule, obj));

            return resultList;
        }

        public ValidationResult ResultForRule(IRule rule, object obj)
        {
            var result = new ValidationResult{ Valid = true };

            var propertyBag = new RulePropertyBag(rule, obj);

            if (rule is RuleRequired)
                result.Valid = IsValidRuleRequired(propertyBag);

            if (rule is RuleRange)
                result.Valid = IsValidRuleRange(propertyBag);

            if (rule is RuleContainsText)
                result.Valid = IsValidRuleContainsText(propertyBag);

            return result;
        }

        bool IsValidRuleRequired(RulePropertyBag propertyBag)
        {
            if (propertyBag.Value is string)
                return !string.IsNullOrEmpty((string)propertyBag.Value);

            return propertyBag.Value != null;
        }

        bool IsValidRuleRange(RulePropertyBag propertyBag)
        {
            var rule = propertyBag.Rule as RuleRange;

            if (rule.Min == null || rule.Max == null)
                throw new ArgumentException("Min and Max must not be null!");

            if (rule.Min.GetType() != rule.Max.GetType())
                throw new ArgumentException("Min and Max are not of equal type!");

            return propertyBag.Value >= (dynamic)rule.Min && propertyBag.Value <= (dynamic)rule.Max;
        }

        bool IsValidRuleContainsText(RulePropertyBag propertyBag)
        {
            var rule = propertyBag.Rule as RuleContainsText;

            if (propertyBag.PInfo.PropertyType != typeof(string))
                throw new ArgumentException("Property is not type of string!");

            return propertyBag.Value == null ? false : ((string)propertyBag.Value).Contains(rule.Text);
        }

        class RulePropertyBag
        {
            public RulePropertyBag(IRule rule, object obj)
            {
                Rule = rule;
                PInfo = obj.GetType().GetRuntimeProperty(rule.TargetProperty);
                Value = PInfo.GetValue(obj);
            }

            public IRule Rule { get; }

            public PropertyInfo PInfo { get; }
        
            public object Value { get; }
        }
    }
}
