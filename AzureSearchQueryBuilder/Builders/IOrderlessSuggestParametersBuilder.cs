using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing an orderless <see cref="SuggestParameters"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface IOrderlessSuggestParametersBuilder<TModel>
    {
        /// <summary>
        /// Build a <typeparamref name="SuggestParameters"/> object.
        /// </summary>
        /// <returns>the <typeparamref name="SuggestParameters"/> object.</returns>
        SuggestParameters Build();
    }
}
