using System.Collections.Generic;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Models;

namespace AzureSearchQueryBuilder.Builders
{
    public interface IOrderedSuggestParametersBuilder<TModel> : IOrderlessSuggestParametersBuilder<TModel>
        where TModel : SearchModel
    {
        IEnumerable<string> OrderBy { get; }

        IOrderedSuggestParametersBuilder<TModel> WithThenBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        IOrderedSuggestParametersBuilder<TModel> WithThenByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);
    }
}
