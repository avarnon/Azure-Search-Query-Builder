using System.Collections.Generic;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Models;

namespace AzureSearchQueryBuilder.Builders
{
    public interface IOrderedSearchParametersBuilder<TModel> : IOrderlessSearchParametersBuilder<TModel>
        where TModel : SearchModel
    {
        IEnumerable<string> OrderBy { get; }

        IOrderedSearchParametersBuilder<TModel> WithThenBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        IOrderedSearchParametersBuilder<TModel> WithThenByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);
    }
}
