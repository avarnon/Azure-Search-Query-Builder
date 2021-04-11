using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// The <see cref="AutocompleteOptions"/>`1[<typeparamref name="TModel"/>] builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public class AutocompleteOptionsBuilder<TModel> : OptionsBuilder<TModel, AutocompleteOptions>, IAutocompleteOptionsBuilder<TModel>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        private AutocompleteOptionsBuilder(JsonSerializerSettings jsonSerializerSettings)
            : base(jsonSerializerSettings)
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
        public AutocompleteMode Mode { get; private set; }

        /// <summary>
        /// Gets the value indicating that suggestions should be found even if there is a substituted or missing character in the search text.
        /// </summary>
        public bool? UseFuzzyMatching { get; private set; }

        /// <summary>
        /// Create a new <see cref="IAutocompleteOptionsBuilder" />`1[<typeparamref name="TModel"/>]"/>.
        /// </summary>
        /// <returns>a new <see cref="IAutocompleteOptionsBuilder" />`1[<typeparamref name="TModel"/>]"/>.</returns>
        public static IAutocompleteOptionsBuilder<TModel> Create(JsonSerializerSettings jsonSerializerSettings) => new AutocompleteOptionsBuilder<TModel>(jsonSerializerSettings);

        /// <summary>
        /// Build a <see cref="AutocompleteOptions"/> object.
        /// </summary>
        /// <returns>the <see cref="AutocompleteOptions"/> object.</returns>
        public override AutocompleteOptions Build()
        {
            AutocompleteOptions autocompleteOptions = new AutocompleteOptions()
            {
                Mode = this.Mode,
                Filter = this.Filter,
                HighlightPostTag = this.HighlightPostTag,
                HighlightPreTag = this.HighlightPreTag,
                MinimumCoverage = this.MinimumCoverage,
                Size = this.Size,
                UseFuzzyMatching = this.UseFuzzyMatching,
            };

            if (this.SearchFields != null)
            {
                foreach (string searchField in this.SearchFields)
                {
                    autocompleteOptions.SearchFields.Add(searchField);
                }
            }

            return autocompleteOptions;
        }

        /// <summary>
        /// Sets the autocomplete mode.
        /// </summary>
        /// <param name="autocompleteMode">The desired autocomplete mode.</param>
        /// <returns>the updated builder.</returns>
        public IAutocompleteOptionsBuilder<TModel> WithMode(AutocompleteMode autocompleteMode)
        {
            this.Mode = autocompleteMode;
            return this;
        }

        /// <summary>
        /// Sets the use fuzzy matching value.
        /// </summary>
        /// <param name="useFuzzyMatching">The desired fuzzy matching mode.</param>
        /// <returns>the updated builder.</returns>
        public IAutocompleteOptionsBuilder<TModel> WithUseFuzzyMatching(bool? useFuzzyMatching)
        {
            this.UseFuzzyMatching = useFuzzyMatching;
            return this;
        }

        #region IAutocompleteOptionsBuilder Explicit Implimentation

        IAutocompleteOptionsBuilder<TModel> IAutocompleteOptionsBuilder<TModel>.Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression)
        {
            this.Where(lambdaExpression);
            return this;
        }

        IAutocompleteOptionsBuilder<TModel> IAutocompleteOptionsBuilder<TModel>.WithHighlightPostTag(string highlightPostTag)
        {
            this.WithHighlightPostTag(highlightPostTag);
            return this;
        }

        IAutocompleteOptionsBuilder<TModel> IAutocompleteOptionsBuilder<TModel>.WithHighlightPreTag(string highlightPreTag)
        {
            this.WithHighlightPreTag(highlightPreTag);
            return this;
        }

        IAutocompleteOptionsBuilder<TModel> IAutocompleteOptionsBuilder<TModel>.WithMinimumCoverage(double? minimumCoverage)
        {
            this.WithMinimumCoverage(minimumCoverage);
            return this;
        }

        IAutocompleteOptionsBuilder<TModel> IAutocompleteOptionsBuilder<TModel>.WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            this.WithSearchField(lambdaExpression);
            return this;
        }

        IAutocompleteOptionsBuilder<TModel> IAutocompleteOptionsBuilder<TModel>.WithSize(int? size)
        {
            this.WithSize(size);
            return this;
        }

        #endregion
    }
}
