using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAppExample.Models.ValueProviders
{
    public class CustomValueProvider : BindingSourceValueProvider, IEnumerableValueProvider
    {
        private readonly IDictionary<string, string> _values = new Dictionary<string, string>()
        {
            {"height", "12cm"},
            {"Lenght", "35m"},
            {"width", "35m"},
        };

        private PrefixContainer PrefixContainer { get; }

        // Binding source helps set the binding context for the model binder?
        public CustomValueProvider(BindingSource bindingSource) : base(bindingSource)
        {
            PrefixContainer = new PrefixContainer(_values.Keys);
        }

        // interface used by the model binder to determine if modelname should be treated as a prefix.
        public override bool ContainsPrefix(string prefix)
        {
            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            return PrefixContainer.ContainsPrefix(prefix);
        }

        // Interfaces used by the model binder to get all property names associated with the modelname.
        public IDictionary<string, string> GetKeysFromPrefix(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            return PrefixContainer.GetKeysFromPrefix(prefix);
        }

        // Interface used by the model binder.
        public override ValueProviderResult GetValue(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }

            if (key.Length == 0)
            {
                return ValueProviderResult.None;
            }

            if (!_values.ContainsKey(key))
            {
                return ValueProviderResult.None;
            }

            return new ValueProviderResult(_values[key]);
        }
    }
}