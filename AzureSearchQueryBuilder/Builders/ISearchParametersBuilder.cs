using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    public interface ISearchParametersBuilder<TModel> : IParametersBuilder<TModel, SearchParameters>
    {
        IEnumerable<string> Facets { get; }

        IEnumerable<string> HighlightFields { get; }

        bool IncludeTotalResultCount { get; }

        IEnumerable<string> OrderBy { get; }

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

        ISearchParametersBuilder<TModel> OrderByAscending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        ISearchParametersBuilder<TModel> OrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        ISearchParametersBuilder<TModel> WithScoringParameter<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        ISearchParametersBuilder<TModel> WithScoringProfile(string scoringProfile);

        ISearchParametersBuilder<TModel> WithSearchMode(SearchMode searchMode);

        ISearchParametersBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        ISearchParametersBuilder<TModel> WithSkip(int? skip);

        ISearchParametersBuilder<TModel> ThenByAscending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        ISearchParametersBuilder<TModel> ThenByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);
    }
}
