using System.Collections.Generic;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Models;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    public interface IOrderlessSearchParametersBuilder<TModel> : IParametersBuilder<TModel, SearchParameters>
        where TModel : SearchModel
    {
        IEnumerable<string> Facets { get; }

        IEnumerable<string> HighlightFields { get; }

        bool IncludeTotalResultCount { get; }

        QueryType QueryType { get; }

        IEnumerable<ScoringParameter> ScoringParameters { get; }

        string ScoringProfile { get; }

        SearchMode SearchMode { get; }

        IEnumerable<string> Select { get; }

        int? Skip { get; }

        ISearchParametersBuilder<TModel> WithFacet<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        ISearchParametersBuilder<TModel> WithHighlightField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        ISearchParametersBuilder<TModel> WithIncludeTotalResultCount(bool includeTotalResultCount);

        ISearchParametersBuilder<TModel> WithQueryType(QueryType queryType);

        ISearchParametersBuilder<TModel> WithScoringParameter(ScoringParameter scoringParameter);

        ISearchParametersBuilder<TModel> WithScoringProfile(string scoringProfile);

        ISearchParametersBuilder<TModel> WithSearchMode(SearchMode searchMode);

        ISearchParametersBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        ISearchParametersBuilder<TModel> WithSkip(int? skip);
    }
}
