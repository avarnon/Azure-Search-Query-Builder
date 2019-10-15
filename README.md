# Azure-Search-Query-Builder
This is a library that uses expression tree parsing to build a parameters object for performing search, suggest, and autocomplete actions with the Azure Search .NET SDK.





## AutocompleteParametersBuilder\<TModel>
The [Microsoft.Azure.Search.Models.AutocompleteParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.AutocompleteParameters)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### static IAutocompleteParametersBuilder\<TModel> Create()
Create a new IAutocompleteParametersBuilder\<TModel>.
### AutocompleteParameters Build()
Build a [Microsoft.Azure.Search.Models.AutocompleteParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.AutocompleteParameters) object.
### IAutocompleteParametersBuilder\<TModel> WithAutocompleteMode(AutocompleteMode autocompleteMode)
Sets the autocomplete mode.
- autocompleteMode: The desired [Microsoft.Azure.Search.Models.AutocompleteMode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.AutocompleteMode).
### IAutocompleteParametersBuilder\<TModel> WithUseFuzzyMatching(bool? useFuzzyMatching)
Sets the use fuzzy matching value.
- useFuzzyMatching: The desired fuzzy matching mode.
### IParametersBuilder<TModel, TParameters> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
> .Where(model => model.Property == value)
### IParametersBuilder<TModel, TParameters> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### IParametersBuilder<TModel, TParameters> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### IParametersBuilder<TModel, TParameters> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### IParametersBuilder<TModel, TParameters> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
> .WithSearchField(model => model.Property)
### IParametersBuilder<TModel, TParameters> WithTop(int? top)
Sets the number of items to retrieve. 
- top: The desired top value.





## SearchParametersBuilder\<TModel>
The [Microsoft.Azure.Search.Models.SearchParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.searchparameters)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### static ISearchParametersBuilder\<TModel> Create()
Create a new ISearchParametersBuilder\<TModel>.
### SearchParameters Build()
Build a [Microsoft.Azure.Search.Models.SearchParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.searchparameters) object.
### ISearchParametersBuilder\<TModel> WithFacet\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a proprty to the collection of fields to facet by.
- lambdaExpression: An expression to extract a property.
> .WithFacet(model => model.Property)
### ISearchParametersBuilder\<TModel> WithHighlightField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of field names used for hit highlights.
- lambdaExpression: An expression to extract a property.
> .WithHighlightField(model => model.Property)
### ISearchParametersBuilder\<TModel> WithIncludeTotalResultCount(bool includeTotalResultCount)
Sets the value indicating whether to fetch the total count of results.
- includeTotalResultCount: The desired include total results count value.
### IOrderedSearchParametersBuilder\<TModel> WithOrderBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
> .WithOrderBy(model => model.Property)
### IOrderedSearchParametersBuilder\<TModel> WithOrderByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in descending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
> .WithOrderByDescending(model => model.Property)
### ISearchParametersBuilder\<TModel> WithQueryType(QueryType queryType)
Sets a value indicating the query type.
- queryType: The desired [Microsoft.Azure.Search.Models.QueryType](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Azure.Search.Models.QueryType).
### ISearchParametersBuilder\<TModel> WithScoringParameter(ScoringParameter scoringParameter)
Sets a value indicating the values for each parameter defined in a scoring function.
- scoringParameter: The desired additional [Microsoft.Azure.Search.Models.ScoringParameter](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Azure.Search.Models.ScoringParameter).
### ISearchParametersBuilder\<TModel> WithScoringProfile(string scoringProfile)
Sets the name of a scoring profile to evaluate match scores for matching documents in order to sort the results.
- scoringProfile: The desired scoring profile name.
### ISearchParametersBuilder\<TModel> WithSearchMode(SearchMode searchMode)
Sets a value indicating whether any or all of the search terms must be matched in order to count the document as a match.
- searchMode: The desired [Microsoft.Azure.Search.Models.SearchMode](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Azure.Search.Models.SearchMode).
### ISearchParametersBuilder\<TModel> WithSelect\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of fields to include in the result set.
- lambdaExpression: An expression to extract a property.
> .WithSelect(model => model.Property)
### ISearchParametersBuilder\<TModel> WithSkip(int? skip)
Sets the number of search results to skip.
- skip: The desired skip value.
### IOrderedSearchParametersBuilder\<TModel> WithThenBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
> .WithThenBy(model => model.Property)
### IOrderedSearchParametersBuilder\<TModel> WithThenByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
> .WithThenByDescending(model => model.Property)
### IParametersBuilder<TModel, TParameters> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
> .Where(model => model.Property == value)
### IParametersBuilder<TModel, TParameters> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### IParametersBuilder<TModel, TParameters> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### IParametersBuilder<TModel, TParameters> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### IParametersBuilder<TModel, TParameters> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
> .WithSearchField(model => model.Property)
### IParametersBuilder<TModel, TParameters> WithTop(int? top)
Sets the number of items to retrieve. 
- top: The desired top value.





## SuggestParametersBuilder\<TModel>
The [Microsoft.Azure.Search.Models.SuggestParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.SuggestParameters)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### static ISuggestParametersBuilder\<TModel> Create()
Create a new ISuggestParametersBuilder\<TModel>.
### SuggestParameters Build()
Build a [Microsoft.Azure.Search.Models.SuggestParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.SuggestParameters) object.
### IOrderedSuggestParametersBuilder\<TModel> WithOrderBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
> .WithOrderBy(model => model.Property)
### IOrderedSuggestParametersBuilder\<TModel> WithOrderByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in descending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
> .WithOrderByDescending(model => model.Property)
### ISuggestParametersBuilder\<TModel> WithSelect\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of fields to include in the result set.
- lambdaExpression: An expression to extract a property.
> .WithSelect(model => model.Property)
### IOrderedSuggestParametersBuilder\<TModel> WithThenBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
> .WithThenBy(model => model.Property)
### IOrderedSuggestParametersBuilder\<TModel> WithThenByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
> .WithThenByDescending(model => model.Property)
### ISuggestParametersBuilder\<TModel> WithUseFuzzyMatching(bool useFuzzyMatching)
Sets the use fuzzy matching value.
- useFuzzyMatching: The desired fuzzy matching mode.
### IParametersBuilder<TModel, TParameters> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
> .Where(model => model.Property == value)
### IParametersBuilder<TModel, TParameters> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### IParametersBuilder<TModel, TParameters> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### IParametersBuilder<TModel, TParameters> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### IParametersBuilder<TModel, TParameters> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
> .WithSearchField(model => model.Property)
### IParametersBuilder<TModel, TParameters> WithTop(int? top)
Sets the number of items to retrieve. 
- top: The desired top value.