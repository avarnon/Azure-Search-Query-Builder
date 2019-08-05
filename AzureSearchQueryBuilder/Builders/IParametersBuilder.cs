using System.Collections.Generic;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Models;
using Newtonsoft.Json;

namespace AzureSearchQueryBuilder.Builders
{
    public delegate TProperty PropertyLambdaDelegate<in TModel, out TProperty>(TModel model);
    public delegate bool BooleanLambdaDelegate<in TModel>(TModel model);

    public interface IParametersBuilder<TModel, TOptions>
        where TModel : SearchModel
    {
        string Filter { get; }

        string HighlightPostTag { get; }

        string HighlightPreTag { get; }

        double? MinimumCoverage { get; }

        IEnumerable<string> SearchFields { get; }

        JsonSerializerSettings SerializerSettings { get; }

        int? Top { get; }

        TOptions Build();

        IParametersBuilder<TModel, TOptions> Where<TProperty>(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression);

        IParametersBuilder<TModel, TOptions> WithHighlightPostTag(string highlightPostTag);

        IParametersBuilder<TModel, TOptions> WithHighlightPreTag(string highlightPreTag);

        IParametersBuilder<TModel, TOptions> WithMinimumCoverage(double? minimumCoverage);

        IParametersBuilder<TModel, TOptions> WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        IParametersBuilder<TModel, TOptions> WithTop(int? top);
    }
}
