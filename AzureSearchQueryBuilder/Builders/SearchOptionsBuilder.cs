using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using AzureSearchQueryBuilder.Helpers;
using Newtonsoft.Json;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// The <see cref="SearchOptionsBuilder"/>`1[<typeparamref name="TModel"/>] builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public class SearchOptionsBuilder<TModel> : OptionsBuilder<TModel, SearchOptions>, ISearchOptionsBuilder<TModel>, IOrderedSearchOptionsBuilder<TModel>
    {
        private IList<string> _facets;
        private IList<string> _highlightFields;
        private IList<string> _orderBy;
        private IList<string> _ScoringParameters;
        private IList<string> _select;

        /// <summary>
        /// Constructor.
        /// </summary>
        private SearchOptionsBuilder(JsonSerializerSettings jsonSerializerSettings)
            : base(jsonSerializerSettings)
        {
        }

        /// <summary>
        /// Get the collection of fields to facet by.
        /// </summary>
        public IEnumerable<string> Facets { get => this._facets; }

        /// <summary>
        /// Gets the collection of field names used for hit highlights.
        /// </summary>
        public IEnumerable<string> HighlightFields { get => this._highlightFields; }

        /// <summary>
        /// Gets the value indicating whether to fetch the total count of results.
        /// </summary>
        public bool IncludeTotalCount { get; private set; }

        /// <summary>
        /// Gets a list of expressions to sort the results by.
        /// Each expression can be either a field name or a call to the geo.distance() function.
        /// Each expression can be followed by asc to indicated ascending, and desc to indicate descending.
        /// The default is ascending order.
        /// There is a limit of 32 clauses for $orderby. 
        /// </summary>
        public IEnumerable<string> OrderBy { get => this._orderBy; }

        /// <summary>
        /// Gets a value indicating the query type.
        /// </summary>
        /// <remarks>
        /// * <see cref="QueryType.Simple"/> - Search text is interpreted using a simple query language that allows for symbols such as +, * and "".
        ///                                    Queries are evaluated across all searchable fields (or fields indicated in searchFields) in each document by default.
        /// * <see cref="QueryType.Full"/> - Search text is interpreted using the Lucene query language which allows field-specific and weighted searches.
        /// </remarks>
        public SearchQueryType QueryType { get; private set; }

        /// <summary>
        /// Gets a value indicating the values for each parameter defined in a scoring function.
        /// </summary>
        public IEnumerable<string> ScoringParameters { get => this._ScoringParameters; }

        /// <summary>
        /// Gets the name of a scoring profile to evaluate match scores for matching documents in order to sort the results.
        /// </summary>
        public string ScoringProfile { get; private set; }

        /// <summary>
        /// Gets a value indicating whether any or all of the search terms must be matched in order to count the document as a match.
        /// </summary>
        public SearchMode SearchMode { get; private set; }

        /// <summary>
        /// Gets a collection of fields to include in the result set.
        /// </summary>
        public IEnumerable<string> Select { get => this._select; }

        /// <summary>
        /// Get the number of search results to skip. 
        /// </summary>
        public int? Skip { get; private set; }

        /// <summary>
        /// Create a new <see cref="ISearchOptionsBuilder" />`1[<typeparamref name="TModel"/>]"/>.
        /// </summary>
        /// <returns>a new <see cref="ISearchOptionsBuilder" />`1[<typeparamref name="TModel"/>]"/>.</returns>
        public static ISearchOptionsBuilder<TModel> Create(JsonSerializerSettings jsonSerializerSettings) => new SearchOptionsBuilder<TModel>(jsonSerializerSettings);

        /// <summary>
        /// Build a <see cref="SearchOptions"/> object.
        /// </summary>
        /// <returns>the <see cref="SearchOptions"/> object.</returns>
        public override SearchOptions Build()
        {
            SearchOptions searchOptions = new SearchOptions()
            {
                Filter = this.Filter,
                HighlightPostTag = this.HighlightPostTag,
                HighlightPreTag = this.HighlightPreTag,
                IncludeTotalCount = this.IncludeTotalCount,
                MinimumCoverage = this.MinimumCoverage,
                QueryType = this.QueryType,
                ScoringProfile = this.ScoringProfile,
                SearchMode = this.SearchMode,
                Skip = this.Skip,
                Size = this.Size,
            };

            if (this.Facets != null)
            {
                foreach (string facet in this.Facets)
                {
                    searchOptions.Facets.Add(facet);
                }
            }

            if (this.HighlightFields != null)
            {
                foreach (string highlightField in this.HighlightFields)
                {
                    searchOptions.HighlightFields.Add(highlightField);
                }
            }

            if (this.OrderBy != null)
            {
                foreach (string orderBy in this.OrderBy)
                {
                    searchOptions.OrderBy.Add(orderBy);
                }
            }

            if (this.ScoringParameters != null)
            {
                foreach (string scoringParameter in this.ScoringParameters)
                {
                    searchOptions.ScoringParameters.Add(scoringParameter);
                }
            }

            if (this.SearchFields != null)
            {
                foreach (string searchField in this.SearchFields)
                {
                    searchOptions.SearchFields.Add(searchField);
                }
            }

            if (this.Select != null)
            {
                foreach (string select in this.Select)
                {
                    searchOptions.Select.Add(select);
                }
            }

            return searchOptions;
        }

        /// <summary>
        /// Adds a proprty to the collection of fields to facet by.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        public ISearchOptionsBuilder<TModel> WithFacet<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._facets == null)
            {
                this._facets = new List<string>();
            }

            string facet = PropertyNameUtility.GetPropertyName(lambdaExpression, this.JsonSerializerSettings, false);
            this._facets.Add(facet);
            return this;
        }

        /// <summary>
        /// Adds a property to the collection of field names used for hit highlights.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        public ISearchOptionsBuilder<TModel> WithHighlightField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._highlightFields == null)
            {
                this._highlightFields = new List<string>();
            }

            string highlightField = PropertyNameUtility.GetPropertyName(lambdaExpression, this.JsonSerializerSettings, false);
            this._highlightFields.Add(highlightField);
            return this;
        }

        /// <summary>
        /// Sets the value indicating whether to fetch the total count of results.
        /// </summary>
        /// <param name="includeTotalResultCount">The desired include total results count value.</param>
        /// <returns>the updated builder.</returns>
        public ISearchOptionsBuilder<TModel> WithIncludeTotalCount(bool includeTotalCount)
        {
            this.IncludeTotalCount = includeTotalCount;
            return this;
        }

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        public IOrderedSearchOptionsBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
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
        public IOrderedSearchOptionsBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            this._orderBy = new List<string>();

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, this.JsonSerializerSettings, false);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }

        /// <summary>
        /// Sets a value indicating the query type.
        /// </summary>
        /// <param name="queryType">The desired query type.</param>
        /// <returns>the updated builder.</returns>
        public ISearchOptionsBuilder<TModel> WithQueryType(SearchQueryType queryType)
        {
            this.QueryType = queryType;
            return this;
        }

        /// <summary>
        /// Sets a value indicating the values for each parameter defined in a scoring function.
        /// </summary>
        /// <param name="scoringParameter">The desired additional scoring parameter.</param>
        /// <returns>the updated builder.</returns>
        public ISearchOptionsBuilder<TModel> WithScoringParameter(string scoringParameter)
        {
            if (scoringParameter == null) throw new ArgumentNullException(nameof(scoringParameter));

            if (this._ScoringParameters == null)
            {
                this._ScoringParameters = new List<string>();
            }

            this._ScoringParameters.Add(scoringParameter);
            return this;
        }

        /// <summary>
        /// Sets the name of a scoring profile to evaluate match scores for matching documents in order to sort the results.
        /// </summary>
        /// <param name="scoringProfile">The desired scoring profile name.</param>
        /// <returns>the updated builder.</returns>
        public ISearchOptionsBuilder<TModel> WithScoringProfile(string scoringProfile)
        {
            this.ScoringProfile = scoringProfile;
            return this;
        }

        /// <summary>
        /// Sets a value indicating whether any or all of the search terms must be matched in order to count the document as a match.
        /// </summary>
        /// <param name="searchMode">The desired search mode.</param>
        /// <returns>the updated builder.</returns>
        public ISearchOptionsBuilder<TModel> WithSearchMode(SearchMode searchMode)
        {
            this.SearchMode = searchMode;
            return this;
        }

        /// <summary>
        /// Adds a property to the collection of fields to include in the result set.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        public ISearchOptionsBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
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
        /// Sets the number of search results to skip. 
        /// </summary>
        /// <param name="skip">The desired skip value.</param>
        /// <returns>the updated builder.</returns>
        public ISearchOptionsBuilder<TModel> WithSkip(int? skip)
        {
            this.Skip = skip;
            return this;
        }

        /// <summary>
        /// Uses an expression for performing a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        public IOrderedSearchOptionsBuilder<TModel> WithThenBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
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
        public IOrderedSearchOptionsBuilder<TModel> WithThenByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));
            if (this._orderBy == null || this._orderBy.Count < 1) throw new ArgumentException($"{nameof(ISuggestOptionsBuilder<TModel>.OrderBy)} has not been initialized", nameof(lambdaExpression));
            if (this._orderBy.Count >= 32) throw new ArgumentException($"{nameof(ISuggestOptionsBuilder<TModel>.OrderBy)} has exceeded the maximum 32 clauses", nameof(lambdaExpression));

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, this.JsonSerializerSettings, false);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }

        #region IOrderedSearchOptionsBuilder Explicit Implementation

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithFacet<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            this.WithFacet(lambdaExpression);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithHighlightField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            this.WithHighlightField(lambdaExpression);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithIncludeTotalCount(bool includeTotalCount)
        {
            this.WithIncludeTotalCount(includeTotalCount);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithQueryType(SearchQueryType queryType)
        {
            this.WithQueryType(queryType);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithScoringParameter(string scoringParameter)
        {
            this.WithScoringParameter(scoringParameter);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithScoringProfile(string scoringProfile)
        {
            this.WithScoringProfile(scoringProfile);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithSearchMode(SearchMode searchMode)
        {
            this.WithSearchMode(searchMode);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            this.WithSelect(lambdaExpression);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithSkip(int? skip)
        {
            this.WithSkip(skip);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression)
        {
            this.Where(lambdaExpression);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithHighlightPostTag(string highlightPostTag)
        {
            this.WithHighlightPostTag(highlightPostTag);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithHighlightPreTag(string highlightPreTag)
        {
            this.WithHighlightPreTag(highlightPreTag);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithMinimumCoverage(double? minimumCoverage)
        {
            this.WithMinimumCoverage(minimumCoverage);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            this.WithSearchField(lambdaExpression);
            return this;
        }

        IOrderedSearchOptionsBuilder<TModel> IOrderedSearchOptionsBuilder<TModel>.WithSize(int? size)
        {
            this.WithSize(size);
            return this;
        }

        #endregion

        #region ISearchOptionsBuilder Explicit Implementation

        ISearchOptionsBuilder<TModel> ISearchOptionsBuilder<TModel>.Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression)
        {
            this.Where(lambdaExpression);
            return this;
        }

        ISearchOptionsBuilder<TModel> ISearchOptionsBuilder<TModel>.WithHighlightPostTag(string highlightPostTag)
        {
            this.WithHighlightPostTag(highlightPostTag);
            return this;
        }

        ISearchOptionsBuilder<TModel> ISearchOptionsBuilder<TModel>.WithHighlightPreTag(string highlightPreTag)
        {
            this.WithHighlightPreTag(highlightPreTag);
            return this;
        }

        ISearchOptionsBuilder<TModel> ISearchOptionsBuilder<TModel>.WithMinimumCoverage(double? minimumCoverage)
        {
            this.WithMinimumCoverage(minimumCoverage);
            return this;
        }

        ISearchOptionsBuilder<TModel> ISearchOptionsBuilder<TModel>.WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            this.WithSearchField(lambdaExpression);
            return this;
        }

        ISearchOptionsBuilder<TModel> ISearchOptionsBuilder<TModel>.WithSize(int? size)
        {
            this.WithSize(size);
            return this;
        }

        #endregion
    }
}
