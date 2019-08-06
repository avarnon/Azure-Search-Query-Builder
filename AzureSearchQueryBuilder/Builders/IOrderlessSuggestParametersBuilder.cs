using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing an orderless <see cref="SuggestParameters"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface IOrderlessSuggestParametersBuilder<TModel> : IParametersBuilder<TModel, SuggestParameters>
    {
        /// <summary>
        /// Gets a collection of fields to include in the result set.
        /// </summary>
        IEnumerable<string> Select { get; }

        /// <summary>
        /// Gets the value indicating that suggestions should be found even if there is a substituted or missing character in the search text.
        /// </summary>
        bool UseFuzzyMatching { get; }

        /// <summary>
        /// Adds a property to the collection of fields to include in the result set.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        ISuggestParametersBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Sets the use fuzzy matching value.
        /// </summary>
        /// <param name="useFuzzyMatching">The desired fuzzy matching mode.</param>
        /// <returns>the updated builder.</returns>
        ISuggestParametersBuilder<TModel> WithUseFuzzyMatching(bool useFuzzyMatching);
    }
}
