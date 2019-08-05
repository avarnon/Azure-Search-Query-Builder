using System.Collections.Generic;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Models;

namespace AzureSearchQueryBuilder.Builders
{
    public interface ISuggestParametersBuilder<TModel> : IOrderlessSuggestParametersBuilder<TModel>
        where TModel : SearchModel
    {
        IEnumerable<string> OrderBy { get; }

        IOrderedSuggestParametersBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        IOrderedSuggestParametersBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);
    }
}
