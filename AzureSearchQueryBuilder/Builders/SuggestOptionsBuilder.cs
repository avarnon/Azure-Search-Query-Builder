using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Azure.Search.Documents;
using AzureSearchQueryBuilder.Helpers;
using Newtonsoft.Json;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// The <see cref="SuggestOptionsBuilder"/>`1[<typeparamref name="TModel"/>] builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public class SuggestOptionsBuilder<TModel> : OptionsBuilder<TModel, SuggestOptions>, ISuggestOptionsBuilder<TModel>, IOrderedSuggestOptionsBuilder<TModel>
    {
        private IList<string> _orderBy;
        private IList<string> _select;

        /// <summary>
        /// Constructor.
        /// </summary>
        private SuggestOptionsBuilder(JsonSerializerSettings jsonSerializerSettings)
            : base(jsonSerializerSettings)
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
        /// Create a new <see cref="ISuggestOptionsBuilder" />`1[<typeparamref name="TModel"/>]"/>.
        /// </summary>
        /// <returns>a new <see cref="ISuggestOptionsBuilder" />`1[<typeparamref name="TModel"/>]"/>.</returns>
        public static ISuggestOptionsBuilder<TModel> Create(JsonSerializerSettings jsonSerializerSettings) => new SuggestOptionsBuilder<TModel>(jsonSerializerSettings);

        /// <summary>
        /// Build a <see cref="SuggestOptions"/> object.
        /// </summary>
        /// <returns>the <see cref="SuggestOptions"/> object.</returns>
        public override SuggestOptions Build()
        {
            SuggestOptions suggestOptions = new SuggestOptions()
            {
                Filter = this.Filter,
                HighlightPostTag = this.HighlightPostTag,
                HighlightPreTag = this.HighlightPreTag,
                MinimumCoverage = this.MinimumCoverage,
                Size = this.Size,
                UseFuzzyMatching = this.UseFuzzyMatching,
            };

            if (this.OrderBy != null)
            {
                foreach (string orderBy in this.OrderBy)
                {
                    suggestOptions.OrderBy.Add(orderBy);
                }
            }

            if (this.SearchFields != null)
            {
                foreach (string searchField in this.SearchFields)
                {
                    suggestOptions.SearchFields.Add(searchField);
                }
            }

            if (this.Select != null)
            {
                foreach (string select in this.Select)
                {
                    suggestOptions.Select.Add(select);
                }
            }

            return suggestOptions;
        }

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        public IOrderedSuggestOptionsBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            this._orderBy = new List<string>();

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, this.JsonSerializerSettings, false);
            this._orderBy.Add($"{orderBy} asc");
            return this;
        }

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in descending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        public IOrderedSuggestOptionsBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            this._orderBy = new List<string>();

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, this.JsonSerializerSettings, false);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }

        /// <summary>
        /// Adds a property to the collection of fields to include in the result set.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        public ISuggestOptionsBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._select == null)
            {
                this._select = new List<string>();
            }

            string selectField = PropertyNameUtility.GetPropertyName(lambdaExpression, this.JsonSerializerSettings, false);
            this._select.Add(selectField);
            return this;
        }

        /// <summary>
        /// Uses an expression for performing a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        public IOrderedSuggestOptionsBuilder<TModel> WithThenBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));
            if (this._orderBy == null || this._orderBy.Count < 1) throw new ArgumentException($"{nameof(ISuggestOptionsBuilder<TModel>.OrderBy)} has not been initialized", nameof(lambdaExpression));
            if (this._orderBy.Count >= 32) throw new ArgumentException($"{nameof(ISuggestOptionsBuilder<TModel>.OrderBy)} has exceeded the maximum 32 clauses", nameof(lambdaExpression));

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, this.JsonSerializerSettings, false);
            this._orderBy.Add($"{orderBy} asc");
            return this;
        }

        /// <summary>
        /// Uses an expression for performing a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        public IOrderedSuggestOptionsBuilder<TModel> WithThenByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));
            if (this._orderBy == null || this._orderBy.Count < 1) throw new ArgumentException($"{nameof(ISuggestOptionsBuilder<TModel>.OrderBy)} has not been initialized", nameof(lambdaExpression));
            if (this._orderBy.Count >= 32) throw new ArgumentException($"{nameof(ISuggestOptionsBuilder<TModel>.OrderBy)} has exceeded the maximum 32 clauses", nameof(lambdaExpression));

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, this.JsonSerializerSettings, false);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }

        /// <summary>
        /// Sets the use fuzzy matching value.
        /// </summary>
        /// <param name="useFuzzyMatching">The desired fuzzy matching mode.</param>
        /// <returns>the updated builder.</returns>
        public ISuggestOptionsBuilder<TModel> WithUseFuzzyMatching(bool useFuzzyMatching)
        {
            this.UseFuzzyMatching = useFuzzyMatching;
            return this;
        }

        #region IOrderedSuggestOptionsBuilder Explicit Implementation

        IOrderedSuggestOptionsBuilder<TModel> IOrderedSuggestOptionsBuilder<TModel>.WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            this.WithSelect(lambdaExpression);
            return this;
        }

        IOrderedSuggestOptionsBuilder<TModel> IOrderedSuggestOptionsBuilder<TModel>.WithUseFuzzyMatching(bool useFuzzyMatching)
        {
            this.WithUseFuzzyMatching(useFuzzyMatching);
            return this;
        }

        IOrderedSuggestOptionsBuilder<TModel> IOrderedSuggestOptionsBuilder<TModel>.Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression)
        {
            this.Where(lambdaExpression);
            return this;
        }

        IOrderedSuggestOptionsBuilder<TModel> IOrderedSuggestOptionsBuilder<TModel>.WithHighlightPostTag(string highlightPostTag)
        {
            this.WithHighlightPostTag(highlightPostTag);
            return this;
        }

        IOrderedSuggestOptionsBuilder<TModel> IOrderedSuggestOptionsBuilder<TModel>.WithHighlightPreTag(string highlightPreTag)
        {
            this.WithHighlightPreTag(highlightPreTag);
            return this;
        }

        IOrderedSuggestOptionsBuilder<TModel> IOrderedSuggestOptionsBuilder<TModel>.WithMinimumCoverage(double? minimumCoverage)
        {
            this.WithMinimumCoverage(minimumCoverage);
            return this;
        }

        IOrderedSuggestOptionsBuilder<TModel> IOrderedSuggestOptionsBuilder<TModel>.WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            this.WithSearchField(lambdaExpression);
            return this;
        }

        IOrderedSuggestOptionsBuilder<TModel> IOrderedSuggestOptionsBuilder<TModel>.WithSize(int? size)
        {
            this.WithSize(size);
            return this;
        }

        #endregion

        #region ISuggestOptionsBuilder Explicit Implementation

        ISuggestOptionsBuilder<TModel> ISuggestOptionsBuilder<TModel>.Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression)
        {
            this.Where(lambdaExpression);
            return this;
        }

        ISuggestOptionsBuilder<TModel> ISuggestOptionsBuilder<TModel>.WithHighlightPostTag(string highlightPostTag)
        {
            this.WithHighlightPostTag(highlightPostTag);
            return this;
        }

        ISuggestOptionsBuilder<TModel> ISuggestOptionsBuilder<TModel>.WithHighlightPreTag(string highlightPreTag)
        {
            this.WithHighlightPreTag(highlightPreTag);
            return this;
        }

        ISuggestOptionsBuilder<TModel> ISuggestOptionsBuilder<TModel>.WithMinimumCoverage(double? minimumCoverage)
        {
            this.WithMinimumCoverage(minimumCoverage);
            return this;
        }

        ISuggestOptionsBuilder<TModel> ISuggestOptionsBuilder<TModel>.WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            this.WithSearchField(lambdaExpression);
            return this;
        }

        ISuggestOptionsBuilder<TModel> ISuggestOptionsBuilder<TModel>.WithSize(int? size)
        {
            this.WithSize(size);
            return this;
        }

        #endregion
    }
}
