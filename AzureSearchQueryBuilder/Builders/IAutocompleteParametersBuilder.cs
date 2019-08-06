using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing an <see cref="AutocompleteParameters"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface IAutocompleteParametersBuilder<TModel> : IParametersBuilder<TModel, AutocompleteParameters>
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
    }
}
