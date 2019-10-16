using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing an <see cref="AutocompleteParameters"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface IAutocompleteParametersBuilder<TModel>
    {
        /// <summary>
        /// Gets the autocomplete mode.
        /// </summary>
        /// <remarks>
        /// * <see cref="AutocompleteMode.OneTerm"/> - Only one term is suggested. If the query has two terms, only the last term is completed.
        /// * <see cref="AutocompleteMode.TwoTerms"/> - Matching two-term phrases in the index will be suggested.
        /// * <see cref="AutocompleteMode.OneTermWithContext"/> - Completes the last term in a query with two or more terms, where the last two terms are a phrase that exists in the index.
        /// </remarks>
        AutocompleteMode AutocompleteMode { get; }

        /// <summary>
        /// Gets the value indicating that suggestions should be found even if there is a substituted or missing character in the search text.
        /// </summary>
        bool? UseFuzzyMatching { get; }

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
        int? Top { get; }

        /// <summary>
        /// Sets the autocomplete mode.
        /// </summary>
        /// <param name="autocompleteMode">The desired autocomplete mode.</param>
        /// <returns>the updated builder.</returns>
        IAutocompleteParametersBuilder<TModel> WithAutocompleteMode(AutocompleteMode autocompleteMode);

        /// <summary>
        /// Sets the use fuzzy matching value.
        /// </summary>
        /// <param name="useFuzzyMatching">The desired fuzzy matching mode.</param>
        /// <returns>the updated builder.</returns>
        IAutocompleteParametersBuilder<TModel> WithUseFuzzyMatching(bool? useFuzzyMatching);

        /// <summary>
        /// Build an <typeparamref name="AutocompleteParameters"/> object.
        /// </summary>
        /// <returns>the <typeparamref name="AutocompleteParameters"/> object.</returns>
        AutocompleteParameters Build();

        /// <summary>
        /// Adds a where clause to the filter expression.
        /// </summary>
        /// <param name="lambdaExpression">The lambda expression used to generate a filter expression.</param>
        /// <returns>the updated builder.</returns>
        IAutocompleteParametersBuilder<TModel> Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression);

        /// <summary>
        /// Sets the string tag that appends to search hits.
        /// </summary>
        /// <param name="highlightPostTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        IAutocompleteParametersBuilder<TModel> WithHighlightPostTag(string highlightPostTag);

        /// <summary>
        /// Sets the string tag that prepends to search hits.
        /// </summary>
        /// <param name="highlightPreTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        IAutocompleteParametersBuilder<TModel> WithHighlightPreTag(string highlightPreTag);

        /// <summary>
        /// sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
        /// </summary>
        /// <param name="minimumCoverage">The desired minimum coverage.</param>
        /// <returns>the updated builder.</returns>
        IAutocompleteParametersBuilder<TModel> WithMinimumCoverage(double? minimumCoverage);

        /// <summary>
        /// Appends to the list of field names to search for the specified search text.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="lambdaExpression">The lambda expression representing the search field.</param>
        /// <returns>the updated builder.</returns>
        IAutocompleteParametersBuilder<TModel> WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Sets the number of items to retrieve. 
        /// </summary>
        /// <param name="top">The desired top value.</param>
        /// <returns>the updated builder.</returns>
        IAutocompleteParametersBuilder<TModel> WithTop(int? top);
    }
}
