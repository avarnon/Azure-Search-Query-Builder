using System.Collections.Generic;
using System.Linq.Expressions;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// A method for evaluating a property selection expression.
    /// </summary>
    /// <typeparam name="TModel">The type of the root model.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="model">The model to be queried.</param>
    /// <returns>the selected property.</returns>
    public delegate TProperty PropertyLambdaDelegate<in TModel, out TProperty>(TModel model);

    /// <summary>
    /// A method for evaluating a boolean expression.
    /// </summary>
    /// <typeparam name="TModel">The type of the root model.</typeparam>
    /// <param name="model">The model to be queried.</param>
    /// <returns>the result of the expression.</returns>
    public delegate bool BooleanLambdaDelegate<in TModel>(TModel model);

    /// <summary>
    /// An interface representing an <see cref="OptionsBuilder"/>`2[<typeparamref name="TModel"/>, <typeparamref name="TOptions"/>] builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    /// <typeparam name="TOptions">The type of the Options object to be built.</typeparam>
    public interface IOptionsBuilder<TModel, TOptions>
    {
        /// <summary>
        /// Gets the expression that filters the documents considered for producing the completed term suggestions.
        /// </summary>
        string Filter { get; }

        /// <summary>
        /// Gets the string tag that appends to search hits.
        /// </summary>
        string HighlightPostTag { get; }

        /// <summary>
        /// Gets the string tag that prepends to search hits.
        /// </summary>
        string HighlightPreTag { get; }

        /// <summary>
        /// Gets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
        /// </summary>
        double? MinimumCoverage { get; }

        /// <summary>
        /// Gets a list of field names to search for the specified search text.
        /// </summary>
        IEnumerable<string> SearchFields { get; }

        /// <summary>
        /// Gets the number of items to retrieve.
        /// </summary>
        int? Size { get; }

        /// <summary>
        /// Build a <typeparamref name="TOptions"/> object.
        /// </summary>
        /// <returns>the <typeparamref name="TOptions"/> object.</returns>
        TOptions Build();

        /// <summary>
        /// Adds a where clause to the filter expression.
        /// </summary>
        /// <param name="lambdaExpression">The lambda expression used to generate a filter expression.</param>
        /// <returns>the updated builder.</returns>
        IOptionsBuilder<TModel, TOptions> Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression);

        /// <summary>
        /// Sets the string tag that appends to search hits.
        /// </summary>
        /// <param name="highlightPostTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        IOptionsBuilder<TModel, TOptions> WithHighlightPostTag(string highlightPostTag);

        /// <summary>
        /// Sets the string tag that prepends to search hits.
        /// </summary>
        /// <param name="highlightPreTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        IOptionsBuilder<TModel, TOptions> WithHighlightPreTag(string highlightPreTag);

        /// <summary>
        /// sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
        /// </summary>
        /// <param name="minimumCoverage">The desired minimum coverage.</param>
        /// <returns>the updated builder.</returns>
        IOptionsBuilder<TModel, TOptions> WithMinimumCoverage(double? minimumCoverage);

        /// <summary>
        /// Appends to the list of field names to search for the specified search text.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="lambdaExpression">The lambda expression representing the search field.</param>
        /// <returns>the updated builder.</returns>
        IOptionsBuilder<TModel, TOptions> WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Sets the number of items to retrieve. 
        /// </summary>
        /// <param name="top">The desired top value.</param>
        /// <returns>the updated builder.</returns>
        IOptionsBuilder<TModel, TOptions> WithSize(int? size);
    }
}
