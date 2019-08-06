using System.Collections.Generic;
using System.Linq.Expressions;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing an ordered <see cref="SuggestParameters"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface IOrderedSuggestParametersBuilder<TModel> : IOrderlessSuggestParametersBuilder<TModel>
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
        /// Uses an expression for performing a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        IOrderedSuggestParametersBuilder<TModel> WithThenBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);

        /// <summary>
        /// Uses an expression for performing a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to be ordered by.</typeparam>
        /// <param name="lambdaExpression">An expression to extract a property from each element.</param>
        /// <returns>the updated builder.</returns>
        IOrderedSuggestParametersBuilder<TModel> WithThenByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression);
    }
}
