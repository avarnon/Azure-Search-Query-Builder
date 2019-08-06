using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AzureSearchQueryBuilder.Helpers;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("AzureSearchQueryBuilder.Tests")]
namespace AzureSearchQueryBuilder.Builders
{
    public abstract class ParametersBuilder<TModel, TParameters> : IParametersBuilder<TModel, TParameters>
    {
        private IList<string> _searchFields;

        public string Filter { get; private set; }

        public string HighlightPostTag { get; private set; }

        public string HighlightPreTag { get; private set; }

        public double? MinimumCoverage { get; private set; }

        public IEnumerable<string> SearchFields { get => _searchFields; }

        public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings()
        {
            ConstructorHandling = ConstructorHandling.Default,
            ContractResolver = new ReadOnlyJsonContractResolver(),
            Converters =
            {
                new Iso8601TimeSpanConverter(),
            },
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK",
            DateParseHandling = DateParseHandling.DateTime,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DefaultValueHandling = DefaultValueHandling.Include,
            FloatFormatHandling = FloatFormatHandling.String,
            FloatParseHandling = FloatParseHandling.Double,
            Formatting = Formatting.Indented,
            MetadataPropertyHandling = MetadataPropertyHandling.Default,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            StringEscapeHandling = StringEscapeHandling.Default,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            TypeNameHandling = TypeNameHandling.None,
        };

        public int? Top { get; private set; }

        public abstract TParameters Build();

        public IParametersBuilder<TModel, TParameters> Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            string newFilter = FilterExpressionUtility.GetFilterExpression(lambdaExpression);
            if (string.IsNullOrWhiteSpace(this.Filter))
            {
                this.Filter = newFilter;
            }
            else
            {
                this.Filter = $"({this.Filter}) and ({newFilter})";
            }

            return this;
        }

        public IParametersBuilder<TModel, TParameters> WithHighlightPostTag(string highlightPostTag)
        {
            this.HighlightPostTag = highlightPostTag;
            return this;
        }

        public IParametersBuilder<TModel, TParameters> WithHighlightPreTag(string highlightPreTag)
        {
            this.HighlightPreTag = highlightPreTag;
            return this;
        }

        public IParametersBuilder<TModel, TParameters> WithMinimumCoverage(double? minimumCoverage)
        {
            this.MinimumCoverage = minimumCoverage;
            return this;
        }

        public IParametersBuilder<TModel, TParameters> WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._searchFields == null)
            {
                this._searchFields = new List<string>();
            }

            string field = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._searchFields.Add(field);

            return this;
        }

        public IParametersBuilder<TModel, TParameters> WithTop(int? top)
        {
            this.Top = top;
            return this;
        }
    }
}
