using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing an orderless <see cref="SearchParameters"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface IOrderlessSearchParametersBuilder<TModel>
    {
        /// <summary>
        /// Build a <typeparamref name="SearchParameters"/> object.
        /// </summary>
        /// <returns>the <typeparamref name="SearchParameters"/> object.</returns>
        SearchParameters Build();
    }
}
