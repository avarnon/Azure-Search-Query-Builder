using System.Linq;
using AzureSearchQueryBuilder.Models;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    public class AutocompleteParametersBuilder<TModel> : ParametersBuilder<TModel, AutocompleteParameters>, IAutocompleteParametersBuilder<TModel>
        where TModel : SearchModel
    {
        private AutocompleteParametersBuilder()
        {
        }

        public AutocompleteMode AutocompleteMode { get; private set; }

        public bool? UseFuzzyMatching { get; private set; }

        public static IAutocompleteParametersBuilder<TModel> Create() => new AutocompleteParametersBuilder<TModel>();

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

        public IAutocompleteParametersBuilder<TModel> WithAutocompleteMode(AutocompleteMode autocompleteMode)
        {
            this.AutocompleteMode = autocompleteMode;
            return this;
        }

        public IAutocompleteParametersBuilder<TModel> WithUseFuzzyMatching(bool? useFuzzyMatching)
        {
            this.UseFuzzyMatching = useFuzzyMatching;
            return this;
        }
    }
}
