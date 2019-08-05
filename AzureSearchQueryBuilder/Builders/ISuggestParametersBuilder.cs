using System.Collections.Generic;
using System.Linq.Expressions;

namespace AzureSearchQueryBuilder.Builders
{
    public interface ISuggestParametersBuilder<TModel> : IOrderlessSuggestParametersBuilder<TModel>
    {
        IEnumerable<string> OrderBy { get; }

        IOrderedSuggestParametersBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        IOrderedSuggestParametersBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);
    }
}
