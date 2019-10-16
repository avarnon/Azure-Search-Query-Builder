using System.Linq;
using System.Linq.Expressions;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// The <see cref="AutocompleteParameters"/>`1[<typeparamref name="TModel"/>] builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public class AutocompleteParametersBuilder<TModel> : ParametersBuilder<TModel, AutocompleteParameters>, IAutocompleteParametersBuilder<TModel>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        private AutocompleteParametersBuilder()
        {
        }

        /// <summary>
        /// Gets the autocomplete mode.
        /// </summary>
        /// <remarks>
        /// * <see cref="AutocompleteMode.OneTerm"/> - Only one term is suggested. If the query has two terms, only the last term is completed.
        /// * <see cref="AutocompleteMode.TwoTerms"/> - Matching two-term phrases in the index will be suggested.
        /// * <see cref="AutocompleteMode.OneTermWithContext"/> - Completes the last term in a query with two or more terms, where the last two terms are a phrase that exists in the index.
        /// </remarks>
        public AutocompleteMode AutocompleteMode { get; private set; }

        /// <summary>
        /// Gets the value indicating that suggestions should be found even if there is a substituted or missing character in the search text.
        /// </summary>
        public bool? UseFuzzyMatching { get; private set; }

        /// <summary>
        /// Create a new <see cref="IAutocompleteParametersBuilder" />`1[<typeparamref name="TModel"/>]"/>.
        /// </summary>
        /// <returns>a new <see cref="IAutocompleteParametersBuilder" />`1[<typeparamref name="TModel"/>]"/>.</returns>
        public static IAutocompleteParametersBuilder<TModel> Create() => new AutocompleteParametersBuilder<TModel>();

        /// <summary>
        /// Build a <see cref="AutocompleteParameters"/> object.
        /// </summary>
        /// <returns>the <see cref="AutocompleteParameters"/> object.</returns>
        public override AutocompleteParameters Build()
        {
            return new AutocompleteParameters()
            {
                AutocompleteMode = this.AutocompleteMode,
                Filter = this.Filter,
                HighlightPostTag = this.HighlightPostTag,
                HighlightPreTag = this.HighlightPreTag,
                MinimumCoverage = this.MinimumCoverage,
                SearchFields = this.SearchFields?.ToList(),
                Top = this.Top,
                UseFuzzyMatching = this.UseFuzzyMatching,
            };
        }

        /// <summary>
        /// Sets the autocomplete mode.
        /// </summary>
        /// <param name="autocompleteMode">The desired autocomplete mode.</param>
        /// <returns>the updated builder.</returns>
        public IAutocompleteParametersBuilder<TModel> WithAutocompleteMode(AutocompleteMode autocompleteMode)
        {
            this.AutocompleteMode = autocompleteMode;
            return this;
        }

        /// <summary>
        /// Sets the use fuzzy matching value.
        /// </summary>
        /// <param name="useFuzzyMatching">The desired fuzzy matching mode.</param>
        /// <returns>the updated builder.</returns>
        public IAutocompleteParametersBuilder<TModel> WithUseFuzzyMatching(bool? useFuzzyMatching)
        {
            this.UseFuzzyMatching = useFuzzyMatching;
            return this;
        }

        #region IAutocompleteParametersBuilder Explicit Implimentation

        IAutocompleteParametersBuilder<TModel> IAutocompleteParametersBuilder<TModel>.Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression)
        {
            this.Where(lambdaExpression);
            return this;
        }

        IAutocompleteParametersBuilder<TModel> IAutocompleteParametersBuilder<TModel>.WithHighlightPostTag(string highlightPostTag)
        {
            this.WithHighlightPostTag(highlightPostTag);
            return this;
        }

        IAutocompleteParametersBuilder<TModel> IAutocompleteParametersBuilder<TModel>.WithHighlightPreTag(string highlightPreTag)
        {
            this.WithHighlightPreTag(highlightPreTag);
            return this;
        }

        IAutocompleteParametersBuilder<TModel> IAutocompleteParametersBuilder<TModel>.WithMinimumCoverage(double? minimumCoverage)
        {
            this.WithMinimumCoverage(minimumCoverage);
            return this;
        }

        IAutocompleteParametersBuilder<TModel> IAutocompleteParametersBuilder<TModel>.WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            this.WithSearchField(lambdaExpression);
            return this;
        }

        IAutocompleteParametersBuilder<TModel> IAutocompleteParametersBuilder<TModel>.WithTop(int? top)
        {
            this.WithTop(top);
            return this;
        }

        #endregion
    }
}
