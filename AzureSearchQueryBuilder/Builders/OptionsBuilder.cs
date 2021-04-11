using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AzureSearchQueryBuilder.Helpers;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("AzureSearchQueryBuilder.Tests")]

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// The <see cref="OptionsBuilder"/>`2[<typeparamref name="TModel"/>, <typeparamref name="TOptions"/>] builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    /// <typeparam name="TOptions">The type of the Options object to be built.</typeparam>
    public abstract class OptionsBuilder<TModel, TOptions> : IOptionsBuilder<TModel, TOptions>
    {
        private IList<string> _searchFields;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="jsonSerializerSettings">The JSON Serialzer Settings</param>
        protected OptionsBuilder(JsonSerializerSettings jsonSerializerSettings)
        {
            this.JsonSerializerSettings = jsonSerializerSettings;
        }

        /// <summary>
        /// Gets the expression that filters the documents considered for producing the completed term suggestions.
        /// </summary>
        public string Filter { get; private set; }

        /// <summary>
        /// Gets the string tag that appends to search hits.
        /// </summary>
        public string HighlightPostTag { get; private set; }

        /// <summary>
        /// Gets the string tag that prepends to search hits.
        /// </summary>
        public string HighlightPreTag { get; private set; }

        /// <summary>
        /// Gets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
        /// </summary>
        public double? MinimumCoverage { get; private set; }

        /// <summary>
        /// Gets a list of field names to search for the specified search text.
        /// </summary>
        public IEnumerable<string> SearchFields { get => _searchFields; }

        /// <summary>
        /// Gets the number of items to retrieve.
        /// </summary>
        public int? Size { get; private set; }

        /// <summary>
        /// The JSON Serializer settings.
        /// </summary>
        protected JsonSerializerSettings JsonSerializerSettings { get; }

        /// <summary>
        /// Build a <typeparamref name="TOptions"/> object.
        /// </summary>
        /// <returns>the <typeparamref name="TOptions"/> object.</returns>
        public abstract TOptions Build();

        /// <summary>
        /// Adds a where clause to the filter expression.
        /// </summary>
        /// <param name="lambdaExpression">The lambda expression used to generate a filter expression.</param>
        /// <returns>the updated builder.</returns>
        public IOptionsBuilder<TModel, TOptions> Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            string newFilter = FilterExpressionUtility.GetFilterExpression(lambdaExpression, this.JsonSerializerSettings);
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

        /// <summary>
        /// Sets the string tag that appends to search hits.
        /// </summary>
        /// <param name="highlightPostTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        public IOptionsBuilder<TModel, TOptions> WithHighlightPostTag(string highlightPostTag)
        {
            this.HighlightPostTag = highlightPostTag;
            return this;
        }

        /// <summary>
        /// Sets the string tag that prepends to search hits.
        /// </summary>
        /// <param name="highlightPreTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        public IOptionsBuilder<TModel, TOptions> WithHighlightPreTag(string highlightPreTag)
        {
            this.HighlightPreTag = highlightPreTag;
            return this;
        }

        /// <summary>
        /// sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
        /// </summary>
        /// <param name="minimumCoverage">The desired minimum coverage.</param>
        /// <returns>the updated builder.</returns>
        public IOptionsBuilder<TModel, TOptions> WithMinimumCoverage(double? minimumCoverage)
        {
            if (minimumCoverage < 0 || minimumCoverage > 100) throw new ArgumentOutOfRangeException(nameof(minimumCoverage), minimumCoverage, $"{nameof(minimumCoverage)} must be between 0 and 100");

            this.MinimumCoverage = minimumCoverage;
            return this;
        }

        /// <summary>
        /// Appends to the list of field names to search for the specified search text.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="lambdaExpression">The lambda expression representing the search field.</param>
        /// <returns>the updated builder.</returns>
        public IOptionsBuilder<TModel, TOptions> WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._searchFields == null)
            {
                this._searchFields = new List<string>();
            }

            string field = PropertyNameUtility.GetPropertyName(lambdaExpression, this.JsonSerializerSettings, false);
            this._searchFields.Add(field);

            return this;
        }

        /// <summary>
        /// Sets the number of items to retrieve. 
        /// </summary>
        /// <param name="top">The desired top value.</param>
        /// <returns>the updated builder.</returns>
        public IOptionsBuilder<TModel, TOptions> WithSize(int? size)
        {
            this.Size = size;
            return this;
        }
    }
}
