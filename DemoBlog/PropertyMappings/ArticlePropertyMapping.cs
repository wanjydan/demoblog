using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using DemoBlog.PropertyMappings.Interfaces;
using DemoBlog.ViewModels.ArticleViewModels;

namespace DemoBlog.PropertyMappings
{
    public class ArticlePropertyMapping : IArticlePropertyMapping
    {
        private Dictionary<string, PropertyMappingValue> _productPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string> {"Id"})},
                {"Title", new PropertyMappingValue(new List<string> {"Title"})},
                {"Date", new PropertyMappingValue(new List<string> {"CreatedDate", "UpdatedDate"})},
                {"Likes", new PropertyMappingValue(new List<string> {"Likes"})}
            };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public ArticlePropertyMapping()
        {
            propertyMappings.Add(new PropertyMapping<ArticleListViewModel, Article>(_productPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for {typeof(TSource)}");
        }

        public Tuple<bool, string[]> ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return Tuple.Create(true, Array.Empty<string>());
            }

            var fieldAfterSplit = fields.Split(",");

            var invalidFields = new List<string>();

            foreach (var field in fieldAfterSplit)
            {
                var trimmedField = field.Trim();

                var indexOfFirstSpace = trimmedField.IndexOf(" ");

                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                    invalidFields.Add(trimmedField);
            }
            
            if(invalidFields.Any())
                return Tuple.Create(false, invalidFields.ToArray());

            return Tuple.Create(true, Array.Empty<string>());
        }
    }
}