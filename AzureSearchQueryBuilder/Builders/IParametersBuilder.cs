using System.Collections.Generic;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Models;
using Newtonsoft.Json;

namespace AzureSearchQueryBuilder.Builders
{
    public delegate TProperty PropertyLambdaDelegate<in TModel, out TProperty>(TModel model);
    public delegate bool BooleanLambdaDelegate<in TModel>(TModel model);

    public interface IParametersBuilder<TModel, TParameters>
        where TModel : SearchModel
    {
        string Filter { get; }

        string HighlightPostTag { get; }

        string HighlightPreTag { get; }

        double? MinimumCoverage { get; }

        IEnumerable<string> SearchFields { get; }

        JsonSerializerSettings SerializerSettings { get; }

        int? Top { get; }

        TParameters Build();

        IParametersBuilder<TModel, TParameters> Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression);

        IParametersBuilder<TModel, TParameters> WithHighlightPostTag(string highlightPostTag);

        IParametersBuilder<TModel, TParameters> WithHighlightPreTag(string highlightPreTag);

        IParametersBuilder<TModel, TParameters> WithMinimumCoverage(double? minimumCoverage);

        IParametersBuilder<TModel, TParameters> WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        IParametersBuilder<TModel, TParameters> WithTop(int? top);
    }
}
