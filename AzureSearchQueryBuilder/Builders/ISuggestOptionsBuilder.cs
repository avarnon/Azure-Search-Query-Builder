using System.Collections.Generic;
using System.Linq.Expressions;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing a <see cref="SuggestOptions"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface ISuggestOptionsBuilder<TModel> : IOrderlessSuggesOptionsBuilder<TModel>
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
        /// Gets a list of expressions to sort the results by.
        /// Each expression can be either a field name or a call to the geo.distance() function.
        /// Each expression can be followed by asc to indicated ascending, and desc to indicate descending.
        /// The default is ascending order.
        /// There is a limit of 32 clauses for $orderby. 
        /// </summary>
        IEnumerable<string> OrderBy { get; }

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
        /// Adds a property to the collection of fields to include in the result set.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being selected.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property.</param>
        /// <returns>the updated builder.</returns>
        ISuggestOptionsBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Sets the use fuzzy matching value.
        /// </summary>
        /// <param name="useFuzzyMatching">The desired fuzzy matching mode.</param>
        /// <returns>the updated builder.</returns>
        ISuggestOptionsBuilder<TModel> WithUseFuzzyMatching(bool useFuzzyMatching);

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        IOrderedSuggestOptionsBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in descending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        IOrderedSuggestOptionsBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Adds a where clause to the filter expression.
        /// </summary>
        /// <param name="lambdaExpression">The lambda expression used to generate a filter expression.</param>
        /// <returns>the updated builder.</returns>
        ISuggestOptionsBuilder<TModel> Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression);

        /// <summary>
        /// Sets the string tag that appends to search hits.
        /// </summary>
        /// <param name="highlightPostTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        ISuggestOptionsBuilder<TModel> WithHighlightPostTag(string highlightPostTag);

        /// <summary>
        /// Sets the string tag that prepends to search hits.
        /// </summary>
        /// <param name="highlightPreTag">the desired tag.</param>
        /// <returns>the updated builder.</returns>
        ISuggestOptionsBuilder<TModel> WithHighlightPreTag(string highlightPreTag);

        /// <summary>
        /// sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
        /// </summary>
        /// <param name="minimumCoverage">The desired minimum coverage.</param>
        /// <returns>the updated builder.</returns>
        ISuggestOptionsBuilder<TModel> WithMinimumCoverage(double? minimumCoverage);

        /// <summary>
        /// Appends to the list of field names to search for the specified search text.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="lambdaExpression">The lambda expression representing the search field.</param>
        /// <returns>the updated builder.</returns>
        ISuggestOptionsBuilder<TModel> WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Sets the number of items to retrieve. 
        /// </summary>
        /// <param name="top">The desired top value.</param>
        /// <returns>the updated builder.</returns>
        ISuggestOptionsBuilder<TModel> WithSize(int? size);
    }
}
