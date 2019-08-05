using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AzureSearchQueryBuilder.Builders
{
    public abstract class ParametersBuilder<TModel, TOptions> : IParametersBuilder<TModel, TOptions>
    {
        private IList<string> _searchFields;

        public string Filter { get; private set; }

        public string HighlightPostTag { get; private set; }

        public string HighlightPreTag { get; private set; }

        public double? MinimumCoverage { get; private set; }

        public IEnumerable<string> SearchFields { get => _searchFields; }

        public JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings();

        public int? Top { get; private set; }

        public abstract TOptions Build();

        public IParametersBuilder<TModel, TOptions> Where<TProperty>(BooleanLambdaDelegate<TModel> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            throw new NotImplementedException();
        }

        public IParametersBuilder<TModel, TOptions> WithHighlightPostTag(string highlightPostTag)
        {
            this.HighlightPostTag = highlightPostTag;
            return this;
        }

        public IParametersBuilder<TModel, TOptions> WithHighlightPreTag(string highlightPreTag)
        {
            this.HighlightPreTag = highlightPreTag;
            return this;
        }

        public IParametersBuilder<TModel, TOptions> WithMinimumCoverage(double? minimumCoverage)
        {
            this.MinimumCoverage = minimumCoverage;
            return this;
        }

        public IParametersBuilder<TModel, TOptions> WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._searchFields == null)
            {
                this._searchFields = new List<string>();
            }

            throw new NotImplementedException();
        }

        public IParametersBuilder<TModel, TOptions> WithTop(int? top)
        {
            this.Top = top;
            return this;
        }
    }
}
