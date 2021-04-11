using Azure.Search.Documents;

namespace AzureSearchQueryBuilder.Builders
{
    /// <summary>
    /// An interface representing an orderless <see cref="SuggestOptions"/> builder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model representing the search index documents.</typeparam>
    public interface IOrderlessSuggesOptionsBuilder<TModel>
    {
        /// <summary>
        /// Build a <typeparamref name="SuggestOptions"/> object.
        /// </summary>
        /// <returns>the <typeparamref name="SuggestOptions"/> object.</returns>
        SuggestOptions Build();
    }
}
