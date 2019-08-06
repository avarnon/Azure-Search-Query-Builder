using System.Collections.Generic;
using System.Linq.Expressions;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing a <see cref="SuggestParameters"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface ISuggestParametersBuilder<TModel> : IOrderlessSuggestParametersBuilder<TModel>
    {
        /// <summary>
        /// Gets a list of expressions to sort the results by.
        /// Each expression can be either a field name or a call to the geo.distance() function.
        /// Each expression can be followed by asc to indicated ascending, and desc to indicate descending.
        /// The default is ascending order.
        /// There is a limit of 32 clauses for $orderby. 
        /// </summary>
        IEnumerable<string> OrderBy { get; }

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        IOrderedSuggestParametersBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Uses an expression for sorting the elements of a sequence in descending order using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        IOrderedSuggestParametersBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);
    }
}
