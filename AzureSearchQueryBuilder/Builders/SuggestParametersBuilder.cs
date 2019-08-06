using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Helpers;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// The <see cref="SuggestParametersBuilder"/>`1[<typeparamref name="TModel"/>] builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public class SuggestParametersBuilder<TModel> : ParametersBuilder<TModel, SuggestParameters>, ISuggestParametersBuilder<TModel>, IOrderedSuggestParametersBuilder<TModel>
    {
        private IList<string> _orderBy;
        private IList<string> _select;

        /// <summary>
        /// Constructor.
        /// </summary>
        private SuggestParametersBuilder()
        {
        }

        /// <summary>
        /// Gets a list of expressions to sort the results by.
        /// Each expression can be either a field name or a call to the geo.distance() function.
        /// Each expression can be followed by asc to indicated ascending, and desc to indicate descending.
        /// The default is ascending order.
        /// There is a limit of 32 clauses for $orderby. 
        /// </summary>
        public IEnumerable<string> OrderBy { get => this._orderBy; }

        /// <summary>
        /// Gets a collection of fields to include in the result set.
        /// </summary>
        public IEnumerable<string> Select { get => this._select; }

        /// <summary>
        /// Gets the value indicating that suggestions should be found even if there is a substituted or missing character in the search text.
        /// </summary>
        public bool UseFuzzyMatching { get; private set; }

        /// <summary>
        /// Create a new <see cref="ISuggestParametersBuilder" />`1[<typeparamref name="TModel"/>]"/>.
        /// </summary>
        /// <returns>a new <see cref="ISuggestParametersBuilder" />`1[<typeparamref name="TModel"/>]"/>.</returns>
        public static ISuggestParametersBuilder<TModel> Create() => new SuggestParametersBuilder<TModel>();

        /// <summary>
        /// Build a <see cref="SuggestParameters"/> object.
        /// </summary>
        /// <returns>the <see cref="SuggestParameters"/> object.</returns>
        public override SuggestParameters Build()
        {
            return new SuggestParameters()
            {
                Filter = this.Filter,
                HighlightPostTag = this.HighlightPostTag,
                HighlightPreTag = this.HighlightPreTag,
                MinimumCoverage = this.MinimumCoverage,
                OrderBy = this.OrderBy?.ToList(),
                SearchFields = this.SearchFields?.ToList(),
                Select = this.Select?.ToList(),
                Top = this.Top,
                UseFuzzyMatching = this.UseFuzzyMatching,
            };
        }

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        public IOrderedSuggestParametersBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            this._orderBy = new List<string>();

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._orderBy.Add($"{orderBy} asc");
            return this;
        }

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in descending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        public IOrderedSuggestParametersBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            this._orderBy = new List<string>();

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }

        /// <summary>
        /// Adds a property to the collection of fields to include in the result set.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        public ISuggestParametersBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._select == null)
            {
                this._select = new List<string>();
            }

            string selectField = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._select.Add(selectField);
            return this;
        }

        /// <summary>
        /// Uses an expression for performing a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        public IOrderedSuggestParametersBuilder<TModel> WithThenBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));
            if (this._orderBy == null || this._orderBy.Count < 1) throw new ArgumentException($"{nameof(ISuggestParametersBuilder<TModel>.OrderBy)} has not been initialized", nameof(lambdaExpression));
            if (this._orderBy.Count >= 32) throw new ArgumentException($"{nameof(ISuggestParametersBuilder<TModel>.OrderBy)} has exceeded the maximum 32 clauses", nameof(lambdaExpression));

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._orderBy.Add($"{orderBy} asc");
            return this;
        }

        /// <summary>
        /// Uses an expression for performing a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        public IOrderedSuggestParametersBuilder<TModel> WithThenByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));
            if (this._orderBy == null || this._orderBy.Count < 1) throw new ArgumentException($"{nameof(ISuggestParametersBuilder<TModel>.OrderBy)} has not been initialized", nameof(lambdaExpression));
            if (this._orderBy.Count >= 32) throw new ArgumentException($"{nameof(ISuggestParametersBuilder<TModel>.OrderBy)} has exceeded the maximum 32 clauses", nameof(lambdaExpression));

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }

        /// <summary>
        /// Sets the use fuzzy matching value.
        /// </summary>
        /// <param name="useFuzzyMatching">The desired fuzzy matching mode.</param>
        /// <returns>the updated builder.</returns>
        public ISuggestParametersBuilder<TModel> WithUseFuzzyMatching(bool useFuzzyMatching)
        {
            this.UseFuzzyMatching = useFuzzyMatching;
            return this;
        }
    }
}
