using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Helpers;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// The <see cref="SearchParametersBuilder"/>`1[<typeparamref name="TModel"/>] builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public class SearchParametersBuilder<TModel> : ParametersBuilder<TModel, SearchParameters>, ISearchParametersBuilder<TModel>, IOrderedSearchParametersBuilder<TModel>
    {
        private IList<string> _facets;
        private IList<string> _highlightFields;
        private IList<string> _orderBy;
        private IList<ScoringParameter> _scoringParameters;
        private IList<string> _select;

        /// <summary>
        /// Constructor.
        /// </summary>
        private SearchParametersBuilder()
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
        public bool IncludeTotalResultCount { get; private set; }

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
        public QueryType QueryType { get; private set; }

        /// <summary>
        /// Gets a value indicating the values for each parameter defined in a scoring function.
        /// </summary>
        public IEnumerable<ScoringParameter> ScoringParameters { get => this._scoringParameters; }

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
        /// Create a new <see cref="ISearchParametersBuilder" />`1[<typeparamref name="TModel"/>]"/>.
        /// </summary>
        /// <returns>a new <see cref="ISearchParametersBuilder" />`1[<typeparamref name="TModel"/>]"/>.</returns>
        public static ISearchParametersBuilder<TModel> Create() => new SearchParametersBuilder<TModel>();

        /// <summary>
        /// Build a <see cref="SearchParameters"/> object.
        /// </summary>
        /// <returns>the <see cref="SearchParameters"/> object.</returns>
        public override SearchParameters Build()
        {
            return new SearchParameters()
            {
                Facets = this.Facets?.ToList(),
                Filter = this.Filter,
                HighlightFields = this.HighlightFields?.ToList(),
                HighlightPostTag = this.HighlightPostTag,
                HighlightPreTag = this.HighlightPreTag,
                IncludeTotalResultCount = this.IncludeTotalResultCount,
                MinimumCoverage = this.MinimumCoverage,
                OrderBy = this.OrderBy?.ToList(),
                QueryType = this.QueryType,
                ScoringParameters = this.ScoringParameters?.ToList(),
                ScoringProfile = this.ScoringProfile,
                SearchFields = this.SearchFields?.ToList(),
                SearchMode = this.SearchMode,
                Select = this.Select?.ToList(),
                Skip = this.Skip,
                Top = this.Top,
            };
        }

        /// <summary>
        /// Adds a proprty to the collection of fields to facet by.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        public ISearchParametersBuilder<TModel> WithFacet<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._facets == null)
            {
                this._facets = new List<string>();
            }

            string facet = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._facets.Add(facet);
            return this;
        }

        /// <summary>
        /// Adds a property to the collection of field names used for hit highlights.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        public ISearchParametersBuilder<TModel> WithHighlightField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._highlightFields == null)
            {
                this._highlightFields = new List<string>();
            }

            string highlightField = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._highlightFields.Add(highlightField);
            return this;
        }

        /// <summary>
        /// Sets the value indicating whether to fetch the total count of results.
        /// </summary>
        /// <param name="includeTotalResultCount">The desired include total results count value.</param>
        /// <returns>the updated builder.</returns>
        public ISearchParametersBuilder<TModel> WithIncludeTotalResultCount(bool includeTotalResultCount)
        {
            this.IncludeTotalResultCount = includeTotalResultCount;
            return this;
        }

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        public IOrderedSearchParametersBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
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
        public IOrderedSearchParametersBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            this._orderBy = new List<string>();

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }

        /// <summary>
        /// Sets a value indicating the query type.
        /// </summary>
        /// <param name="queryType">The desired query type.</param>
        /// <returns>the updated builder.</returns>
        public ISearchParametersBuilder<TModel> WithQueryType(QueryType queryType)
        {
            this.QueryType = queryType;
            return this;
        }

        /// <summary>
        /// Sets a value indicating the values for each parameter defined in a scoring function.
        /// </summary>
        /// <param name="scoringParameter">The desired additional scoring parameter.</param>
        /// <returns>the updated builder.</returns>
        public ISearchParametersBuilder<TModel> WithScoringParameter(ScoringParameter scoringParameter)
        {
            if (scoringParameter == null) throw new ArgumentNullException(nameof(scoringParameter));

            if (this._scoringParameters == null)
            {
                this._scoringParameters = new List<ScoringParameter>();
            }

            this._scoringParameters.Add(scoringParameter);
            return this;
        }

        /// <summary>
        /// Sets the name of a scoring profile to evaluate match scores for matching documents in order to sort the results.
        /// </summary>
        /// <param name="scoringProfile">The desired scoring profile name.</param>
        /// <returns>the updated builder.</returns>
        public ISearchParametersBuilder<TModel> WithScoringProfile(string scoringProfile)
        {
            this.ScoringProfile = scoringProfile;
            return this;
        }

        /// <summary>
        /// Sets a value indicating whether any or all of the search terms must be matched in order to count the document as a match.
        /// </summary>
        /// <param name="searchMode">The desired search mode.</param>
        /// <returns>the updated builder.</returns>
        public ISearchParametersBuilder<TModel> WithSearchMode(SearchMode searchMode)
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
        public ISearchParametersBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
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
        /// Sets the number of search results to skip. 
        /// </summary>
        /// <param name="skip">The desired skip value.</param>
        /// <returns>the updated builder.</returns>
        public ISearchParametersBuilder<TModel> WithSkip(int? skip)
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
        public IOrderedSearchParametersBuilder<TModel> WithThenBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
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
        public IOrderedSearchParametersBuilder<TModel> WithThenByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));
            if (this._orderBy == null || this._orderBy.Count < 1) throw new ArgumentException($"{nameof(ISuggestParametersBuilder<TModel>.OrderBy)} has not been initialized", nameof(lambdaExpression));
            if (this._orderBy.Count >= 32) throw new ArgumentException($"{nameof(ISuggestParametersBuilder<TModel>.OrderBy)} has exceeded the maximum 32 clauses", nameof(lambdaExpression));

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }
    }
}
