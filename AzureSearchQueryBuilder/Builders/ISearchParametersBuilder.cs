using System.Collections.Generic;
using System.Linq.Expressions;

namespace AzureSearchQueryBuilder.Builders
{
    public interface ISearchParametersBuilder<TModel> : IOrderlessSearchParametersBuilder<TModel>
    {
        IEnumerable<string> OrderBy { get; }

        IOrderedSearchParametersBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        IOrderedSearchParametersBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);
    }
}
