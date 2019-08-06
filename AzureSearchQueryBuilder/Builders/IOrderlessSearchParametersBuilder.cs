using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing an orderless <see cref="SearchParameters"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface IOrderlessSearchParametersBuilder<TModel> : IParametersBuilder<TModel, SearchParameters>
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
        bool IncludeTotalResultCount { get; }

        /// <summary>
        /// Gets a value indicating the query type.
        /// </summary>
        /// <remarks>
        /// * <see cref="QueryType.Simple"/> - Search text is interpreted using a simple query language that allows for symbols such as +, * and "".
        ///                                    Queries are evaluated across all searchable fields (or fields indicated in searchFields) in each document by default.
        /// * <see cref="QueryType.Full"/> - Search text is interpreted using the Lucene query language which allows field-specific and weighted searches.
        /// </remarks>
        QueryType QueryType { get; }

        /// <summary>
        /// Gets a value indicating the values for each parameter defined in a scoring function.
        /// </summary>
        IEnumerable<ScoringParameter> ScoringParameters { get; }

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
        /// Adds a proprty to the collection of fields to facet by.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        ISearchParametersBuilder<TModel> WithFacet<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Adds a property to the collection of field names used for hit highlights.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        ISearchParametersBuilder<TModel> WithHighlightField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Sets the value indicating whether to fetch the total count of results.
        /// </summary>
        /// <param name="includeTotalResultCount">The desired include total results count value.</param>
        /// <returns>the updated builder.</returns>
        ISearchParametersBuilder<TModel> WithIncludeTotalResultCount(bool includeTotalResultCount);

        /// <summary>
        /// Sets a value indicating the query type.
        /// </summary>
        /// <param name="queryType">The desired query type.</param>
        /// <returns>the updated builder.</returns>
        ISearchParametersBuilder<TModel> WithQueryType(QueryType queryType);

        /// <summary>
        /// Sets a value indicating the values for each parameter defined in a scoring function.
        /// </summary>
        /// <param name="scoringParameter">The desired additional scoring parameter.</param>
        /// <returns>the updated builder.</returns>
        ISearchParametersBuilder<TModel> WithScoringParameter(ScoringParameter scoringParameter);

        /// <summary>
        /// Sets the name of a scoring profile to evaluate match scores for matching documents in order to sort the results.
        /// </summary>
        /// <param name="scoringProfile">The desired scoring profile name.</param>
        /// <returns>the updated builder.</returns>
        ISearchParametersBuilder<TModel> WithScoringProfile(string scoringProfile);

        /// <summary>
        /// Sets a value indicating whether any or all of the search terms must be matched in order to count the document as a match.
        /// </summary>
        /// <param name="searchMode">The desired search mode.</param>
        /// <returns>the updated builder.</returns>
        ISearchParametersBuilder<TModel> WithSearchMode(SearchMode searchMode);

        /// <summary>
        /// Adds a property to the collection of fields to include in the result set.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        ISearchParametersBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Sets the number of search results to skip. 
        /// </summary>
        /// <param name="skip">The desired skip value.</param>
        /// <returns>the updated builder.</returns>
        ISearchParametersBuilder<TModel> WithSkip(int? skip);
    }
}
