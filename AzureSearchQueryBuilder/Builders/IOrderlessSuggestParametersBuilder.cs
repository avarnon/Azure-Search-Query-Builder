using System.Collections.Generic;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Models;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    public interface IOrderlessSuggestParametersBuilder<TModel> : IParametersBuilder<TModel, SuggestParameters>
        where TModel : SearchModel
    {
        IEnumerable<string> Select { get; }

        bool UseFuzzyMatching { get; }

        ISuggestParametersBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        ISuggestParametersBuilder<TModel> WithUseFuzzyMatching(bool useFuzzyMatching);
    }
}
