namespace AzureSearchQueryBuilder
{
    /// <summary>
    /// Constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The geo.distance() function from Azure Search.
        /// </summary>
        public const string GeoDistance = "geo.distance()";

        /// <summary>
        /// The search.score() function from Azure Search.
        /// </summary>
        public const string SearchScore = "search.score()";

        /// <summary>
        /// The OData member access operator.
        /// </summary>
        internal const string ODataMemberAccessOperator = "/";
    }
}
