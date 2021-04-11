using System.Collections.Generic;
using System.Linq.Expressions;
using Azure.Search.Documents.Models;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing a <see cref="SearchOptions"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface ISearchOptionsBuilder<TModel> : IOrderlessSearchOptionsBuilder<TModel>
    {
        /// <summary>
        /// Get the collection of fields to facet by.
        /// </summary>
        IEnumerable<string> Facets { get; }

        /// <summary>
        /// Gets the collection of field names used for hit highlights.
        /// </summary>
        IEnumerable<string> HighlightFields { get; }

        /// <summary>
        /// Gets the value indicating whether to fetch the total count of results.
        /// </summary>
        bool IncludeTotalCount { get; }

        /// <summary>
        /// Gets a value indicating the query type.
        /// </summary>
        /// <remarks>
        /// * <see cref="QueryType.Simple"/> - Search text is interpreted using a simple query language that allows for symbols such as +, * and "".
        ///                                    Queries are evaluated across all searchable fields (or fields indicated in searchFields) in each document by default.
        /// * <see cref="QueryType.Full"/> - Search text is interpreted using the Lucene query language which allows field-specific and weighted searches.
        /// </remarks>
        SearchQueryType QueryType { get; }

        /// <summary>
        /// Gets a value indicating the values for each parameter defined in a scoring function.
        /// </summary>
        IEnumerable<string> ScoringParameters { get; }

        /// <summary>
        /// Gets the name of a scoring profile to evaluate match scores for matching documents in order to sort the results.
        /// </summary>
        string ScoringProfile { get; }

        /// <summary>
        /// Gets a value indicating whether any or all of the search terms must be matched in order to count the document as a match.
        /// </summary>
        SearchMode SearchMode { get; }

        /// <summary>
        /// Gets a collection of fields to include in the result set.
        /// </summary>
        IEnumerable<string> Select { get; }

        /// <summary>
        /// Get the number of search results to skip. 
        /// </summary>
        int? Skip { get; }

        /// <summary>
        /// Gets a list of expressions to sort the results by.
        /// Each expression can be either a field name or a call to the geo.distance() function.
        /// Each expression can be followed by asc to indicated ascending, and desc to indicate descending.
        /// The default is ascending order.
        /// There is a limit of 32 clauses for $orderby. 
        /// </summary>
        IEnumerable<string> OrderBy { get; }

        /// <summary>
        /// Gets the expression that filters the documents considered for producing the completed term suggestions.
        /// </summary>
        string Filter { get; }

        /// <summary>
        /// Gets the string tag that appends to search hits.
        /// </summary>
        string HighlightPostTag { get; }

        /// <summary>
        /// Gets the string tag that prepends to search hits.
        /// </summary>
        string HighlightPreTag { get; }

        /// <summary>
        /// Gets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
        /// </summary>
        double? MinimumCoverage { get; }

        /// <summary>
        /// Gets a list of field names to search for the specified search text.
        /// </summary>
        IEnumerable<string> SearchFields { get; }

        /// <summary>
        /// Gets the number of items to retrieve.
        /// </summary>
        int? Size { get; }

        /// <summary>
        /// Adds a proprty to the collection of fields to facet by.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithFacet<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Adds a property to the collection of field names used for hit highlights.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithHighlightField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Sets the value indicating whether to fetch the total count of results.
        /// </summary>
        /// <param name="includeTotalResultCount">The desired include total results count value.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithIncludeTotalCount(bool includeTotalCount);

        /// <summary>
        /// Sets a value indicating the query type.
        /// </summary>
        /// <param name="queryType">The desired query type.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithQueryType(SearchQueryType queryType);

        /// <summary>
        /// Sets a value indicating the values for each parameter defined in a scoring function.
        /// </summary>
        /// <param name="scoringParameter">The desired additional scoring parameter.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithScoringParameter(string scoringParameter);

        /// <summary>
        /// Sets the name of a scoring profile to evaluate match scores for matching documents in order to sort the results.
        /// </summary>
        /// <param name="scoringProfile">The desired scoring profile name.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithScoringProfile(string scoringProfile);

        /// <summary>
        /// Sets a value indicating whether any or all of the search terms must be matched in order to count the document as a match.
        /// </summary>
        /// <param name="searchMode">The desired search mode.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithSearchMode(SearchMode searchMode);

        /// <summary>
        /// Adds a property to the collection of fields to include in the result set.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Sets the number of search results to skip. 
        /// </summary>
        /// <param name="skip">The desired skip value.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithSkip(int? skip);

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        IOrderedSearchOptionsBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in descending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        IOrderedSearchOptionsBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Adds a where clause to the filter expression.
        /// </summary>
        /// <param name="lambdaExpression">The lambda expression used to generate a filter expression.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression);

        /// <summary>
        /// Sets the string tag that appends to search hits.
        /// </summary>
        /// <param name="highlightPostTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithHighlightPostTag(string highlightPostTag);

        /// <summary>
        /// Sets the string tag that prepends to search hits.
        /// </summary>
        /// <param name="highlightPreTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithHighlightPreTag(string highlightPreTag);

        /// <summary>
        /// sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
        /// </summary>
        /// <param name="minimumCoverage">The desired minimum coverage.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithMinimumCoverage(double? minimumCoverage);

        /// <summary>
        /// Appends to the list of field names to search for the specified search text.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="lambdaExpression">The lambda expression representing the search field.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Sets the number of items to retrieve. 
        /// </summary>
        /// <param name="top">The desired top value.</param>
        /// <returns>the updated builder.</returns>
        ISearchOptionsBuilder<TModel> WithSize(int? size);
    }
}
