using AzureSearchQueryBuilder.Models;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    public interface IAutocompleteParametersBuilder<TModel> : IParametersBuilder<TModel, AutocompleteParameters>
        where TModel : SearchModel
    {
        AutocompleteMode AutocompleteMode { get; }

        bool? UseFuzzyMatching { get; }

        IAutocompleteParametersBuilder<TModel> WithAutocompleteMode(AutocompleteMode autocompleteMode);

        IAutocompleteParametersBuilder<TModel> WithUseFuzzyMatching(bool? useFuzzyMatching);
    }
}
