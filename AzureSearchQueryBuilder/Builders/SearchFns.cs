using System;
using System.Collections.Generic;

namespace AzureSearchQueryBuilder.Builders
{
    public static class SearchFns
    {
        /// <summary>
        /// The search.ismatch function evaluates a full-text search query as a part of a filter expression. The documents that match the search query will be returned in the result set. 
        /// </summary>
        /// <param name="search">The search query (in either simple or full Lucene query syntax).</param>
        /// <param name="searchFields">Comma-separated list of searchable fields to search in; defaults to all searchable fields in the index. When using fielded search in the search parameter, the field specifiers in the Lucene query override any fields specified in this parameter.</param>
        /// <returns>The search.ismatch function returns a value of type Edm.Boolean, which allows you to compose it with other filter sub-expressions using the Boolean logical operators.</returns>
        /// <remarks>https://docs.microsoft.com/en-us/azure/search/search-query-odata-full-text-search-functions</remarks>
        public static bool IsMatch(string search, IEnumerable<object> searchFields) => throw new NotImplementedException();

        /// <summary>
        /// The search.ismatch function evaluates a full-text search query as a part of a filter expression. The documents that match the search query will be returned in the result set. 
        /// </summary>
        /// <param name="search">The search query (in either simple or full Lucene query syntax).</param>
        /// <param name="searchFields">Comma-separated list of searchable fields to search in; defaults to all searchable fields in the index. When using fielded search in the search parameter, the field specifiers in the Lucene query override any fields specified in this parameter.</param>
        /// <returns>The search.ismatch function returns a value of type Edm.Boolean, which allows you to compose it with other filter sub-expressions using the Boolean logical operators.</returns>
        /// <remarks>https://docs.microsoft.com/en-us/azure/search/search-query-odata-full-text-search-functions</remarks>
        public static bool IsMatch(string search, params object[] searchFields) => throw new NotImplementedException();

        /// <summary>
        /// The search.ismatchscoring function, like the search.ismatch function, returns true for documents that match the full-text search query passed as a parameter. The difference between them is that the relevance score of documents matching the search.ismatchscoring query will contribute to the overall document score, while in the case of search.ismatch, the document score won't be changed. 
        /// </summary>
        /// <param name="search">The search query (in either simple or full Lucene query syntax).</param>
        /// <param name="searchFields">Comma-separated list of searchable fields to search in; defaults to all searchable fields in the index. When using fielded search in the search parameter, the field specifiers in the Lucene query override any fields specified in this parameter.</param>
        /// <returns>The search.ismatchscoring function returns a value of type Edm.Boolean, which allows you to compose it with other filter sub-expressions using the Boolean logical operators.</returns>
        /// <remarks>https://docs.microsoft.com/en-us/azure/search/search-query-odata-full-text-search-functions</remarks>
        public static bool IsMatchScoring(string search, IEnumerable<object> searchFields) => throw new NotImplementedException();

        /// <summary>
        /// The search.ismatchscoring function, like the search.ismatch function, returns true for documents that match the full-text search query passed as a parameter. The difference between them is that the relevance score of documents matching the search.ismatchscoring query will contribute to the overall document score, while in the case of search.ismatch, the document score won't be changed. 
        /// </summary>
        /// <param name="search">The search query (in either simple or full Lucene query syntax).</param>
        /// <param name="searchFields">Comma-separated list of searchable fields to search in; defaults to all searchable fields in the index. When using fielded search in the search parameter, the field specifiers in the Lucene query override any fields specified in this parameter.</param>
        /// <returns>The search.ismatchscoring function returns a value of type Edm.Boolean, which allows you to compose it with other filter sub-expressions using the Boolean logical operators.</returns>
        /// <remarks>https://docs.microsoft.com/en-us/azure/search/search-query-odata-full-text-search-functions</remarks>
        public static bool IsMatchScoring(string search, params object[] searchFields) => throw new NotImplementedException();

        /// <summary>
        /// The search.in function tests whether a given string field or range variable is equal to one of a given list of values. Equality between the variable and each value in the list is determined in a case-sensitive fashion, the same way as for the eq operator. Therefore an expression like search.in(myfield, 'a, b, c') is equivalent to myfield eq 'a' or myfield eq 'b' or myfield eq 'c', except that search.in will yield much better performance.
        /// </summary>
        /// <param name="variable">A string field reference (or a range variable over a string collection field in the case where search.in is used inside an any or all expression).</param>
        /// <param name="valueList">A string containing a delimited list of values to match against the variable parameter. If the delimiters parameter is not specified, the default delimiters are space and comma.</param>
        /// <returns></returns>
        /// <remarks>https://docs.microsoft.com/en-us/azure/search/search-query-odata-search-in-function</remarks>
        public static bool In(string variable, IEnumerable<string> valueList) => throw new NotImplementedException();

        /// <summary>
        /// The search.in function tests whether a given string field or range variable is equal to one of a given list of values. Equality between the variable and each value in the list is determined in a case-sensitive fashion, the same way as for the eq operator. Therefore an expression like search.in(myfield, 'a, b, c') is equivalent to myfield eq 'a' or myfield eq 'b' or myfield eq 'c', except that search.in will yield much better performance.
        /// </summary>
        /// <param name="variable">A string field reference (or a range variable over a string collection field in the case where search.in is used inside an any or all expression).</param>
        /// <param name="valueList">A string containing a delimited list of values to match against the variable parameter. If the delimiters parameter is not specified, the default delimiters are space and comma.</param>
        /// <returns></returns>
        /// <remarks>https://docs.microsoft.com/en-us/azure/search/search-query-odata-search-in-function</remarks>
        public static bool In(string variable, params string[] valueList) => throw new NotImplementedException();

        /// <summary>
        /// The document's search score.
        /// </summary>
        /// <returns></returns>
        /// <remarks>https://docs.microsoft.com/en-us/azure/search/search-query-odata-search-score-function</remarks>
        public static double Score() => throw new NotImplementedException();
    }
}
