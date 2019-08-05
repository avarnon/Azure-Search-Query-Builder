using System.Collections.Generic;
using System.Linq.Expressions;

namespace AzureSearchQueryBuilder.Builders
{
    public interface IOrderedSearchParametersBuilder<TModel> : IOrderlessSearchParametersBuilder<TModel>
    {
        IEnumerable<string> OrderBy { get; }

        IOrderedSearchParametersBuilder<TModel> WithThenBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        IOrderedSearchParametersBuilder<TModel> WithThenByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);
    }
}
