using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AzureSearchQueryBuilder.Helpers;

[assembly: InternalsVisibleTo("AzureSearchQueryBuilder.Tests")]

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// The <see cref="ParametersBuilder"/>`2[<typeparamref name="TModel"/>, <typeparamref name="TParameters"/>] builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    /// <typeparam name="TParameters">The type of the parameters object to be built.</typeparam>
    public abstract class ParametersBuilder<TModel, TParameters> : IParametersBuilder<TModel, TParameters>
    {
        private IList<string> _searchFields;

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
        public int? Top { get; private set; }

        /// <summary>
        /// Build a <typeparamref name="TParameters"/> object.
        /// </summary>
        /// <returns>the <typeparamref name="TParameters"/> object.</returns>
        public abstract TParameters Build();

        /// <summary>
        /// Adds a where clause to the filter expression.
        /// </summary>
        /// <param name="lambdaExpression">The lambda expression used to generate a filter expression.</param>
        /// <returns>the updated builder.</returns>
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

        /// <summary>
        /// Sets the string tag that appends to search hits.
        /// </summary>
        /// <param name="highlightPostTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        public IParametersBuilder<TModel, TParameters> WithHighlightPostTag(string highlightPostTag)
        {
            this.HighlightPostTag = highlightPostTag;
            return this;
        }

        /// <summary>
        /// Sets the string tag that prepends to search hits.
        /// </summary>
        /// <param name="highlightPreTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        public IParametersBuilder<TModel, TParameters> WithHighlightPreTag(string highlightPreTag)
        {
            this.HighlightPreTag = highlightPreTag;
            return this;
        }

        /// <summary>
        /// sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
        /// </summary>
        /// <param name="minimumCoverage">The desired minimum coverage.</param>
        /// <returns>the updated builder.</returns>
        public IParametersBuilder<TModel, TParameters> WithMinimumCoverage(double? minimumCoverage)
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

        /// <summary>
        /// Sets the number of items to retrieve. 
        /// </summary>
        /// <param name="top">The desired top value.</param>
        /// <returns>the updated builder.</returns>
        public IParametersBuilder<TModel, TParameters> WithTop(int? top)
        {
            this.Top = top;
            return this;
        }
    }
}
