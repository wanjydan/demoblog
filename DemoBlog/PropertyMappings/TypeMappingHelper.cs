using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DemoBlog.Mappings.Interfaces;

namespace DemoBlog.Mappings
{
    public class TypeHelperService : ITypeHelperService
    {
        public Tuple<bool, string[]> TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
                return Tuple.Create(true, Array.Empty<string>());

            var fieldsAfterSplit = fields.Split(',');

            var invalidFields = new List<string>();

            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(T).GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                    invalidFields.Add(propertyName);
            }

            if (invalidFields.Any())
                return Tuple.Create(false, invalidFields.ToArray());

            return Tuple.Create(true, Array.Empty<string>());
        }
    }
}