using Azure.Search.Documents;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing an orderless <see cref="SearchOptions"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface IOrderlessSearchOptionsBuilder<TModel>
    {
        /// <summary>
        /// Build a <typeparamref name="SearchOptions"/> object.
        /// </summary>
        /// <returns>the <typeparamref name="SearchOptions"/> object.</returns>
        SearchOptions Build();
    }
}
