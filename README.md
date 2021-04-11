# Azure-Search-Query-Builder
This is a library that uses expression tree parsing to build a Options object for performing search, suggest, and autocomplete actions with the Azure Search .NET SDK.

![ASP.NET Core CI](https://github.com/avarnon/Azure-Search-Query-Builder/workflows/ASP.NET%20Core%20CI/badge.svg)
![CodeQL](https://github.com/avarnon/Azure-Search-Query-Builder/workflows/CodeQL/badge.svg)




## [AutocompleteOptionsBuilder](AzureSearchQueryBuilder/Builders/AutocompleteOptionsBuilder.cs)\<TModel>
The [Azure.Search.Documents.AutocompleteOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.AutocompleteOptions)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### static IAutocompleteOptionsBuilder\<TModel> Create()
Create a new IAutocompleteOptionsBuilder\<TModel>.
### Example using models from [How to use Azure Search from a .NET Application](https://docs.microsoft.com/en-us/azure/search/search-howto-dotnet-sdk#how-the-net-sdk-handles-documents)
```C#
AutocompleteOptionsBuilder<Hotel>.Create()
  .Where(hotel => hotel.HotelId == "Some ID" || hotel.HotelName == "Some Name")
  .Where(hotel => hotel.ParkingIncluded == true)
  .WithAutocompleteMode(AutocompleteMode.OneTerm)
  .WithHighlightPostTag("Post Tag")
  .WithHighlightPreTag("Pre Tag")
  .WithMinimumCoverage(0.75)
  .WithSearchField(hotel => hotel.HotelName)
  .WithSearchField(hotel => hotel.Description)
  .WithSize(10)
  .WithUseFuzzyMatching(true)
  .Build();
```
| Property         | Value                                                                                |
| ---------------- | ------------------------------------------------------------------------------------ |
| AutocompleteMode | OneTerm                                                                              |
| Filter           | ((HotelId eq 'Some ID') or (HotelName eq 'Some Name')) and (ParkingIncluded eq true) |
| HighlightPostTag | Post Tag                                                                             |
| HighlightPreTag  | Pre Tag                                                                              |
| MinimumCoverage  | 0.75                                                                                 |
| SearchFields     | HotelName, Description                                                               |
| Size              | 10                                                                                   |
| UseFuzzyMatching | True                                                                                 |




## [IAutocompleteOptionsBuilder](AzureSearchQueryBuilder/Builders/IAutocompleteOptionsBuilder.cs)\<TModel>
The [Azure.Search.Documents.AutocompleteOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.AutocompleteOptions)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### AutocompleteOptions Build()
Build a [Azure.Search.Documents.AutocompleteOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.AutocompleteOptions) object.
### IAutocompleteOptionsBuilder\<TModel> WithAutocompleteMode(AutocompleteMode autocompleteMode)
Sets the autocomplete mode.
- autocompleteMode: The desired [Azure.Search.Documents.AutocompleteMode](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.AutocompleteMode).
### IAutocompleteOptionsBuilder\<TModel> WithUseFuzzyMatching(bool? useFuzzyMatching)
Sets the use fuzzy matching value.
- useFuzzyMatching: The desired fuzzy matching mode.
### IAutocompleteOptionsBuilder\<TModel> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
```C#
.Where(model => model.Property == value)
```
### IAutocompleteOptionsBuilder\<TModel> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### IAutocompleteOptionsBuilder\<TModel> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### IAutocompleteOptionsBuilder\<TModel> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### IAutocompleteOptionsBuilder\<TModel> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
```C#
.WithSearchField(model => model.Property)
```
### IAutocompleteOptionsBuilder\<TModel> WithSize(int? size)
Sets the number of items to retrieve. 
- size: The desired size value.





## [IOrderedSearchOptionsBuilder](AzureSearchQueryBuilder/Builders/IOrderedSearchOptionsBuilder.cs)\<TModel>
The [Azure.Search.Documents.SearchOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.searchOptions)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### SearchOptions Build()
Build a [Azure.Search.Documents.SearchOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.searchOptions) object.
### IOrderedSearchOptionsBuilder\<TModel> WithFacet\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a proprty to the collection of fields to facet by.
- lambdaExpression: An expression to extract a property.
```C#
.WithFacet(model => model.Property)
```
### IOrderedSearchOptionsBuilder\<TModel> WithHighlightField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of field names used for hit highlights.
- lambdaExpression: An expression to extract a property.
```C#
.WithHighlightField(model => model.Property)
```
### IOrderedSearchOptionsBuilder\<TModel> WithIncludeTotalCount(bool includeTotalCount)
Sets the value indicating whether to fetch the total count of results.
- includeTotalCount: The desired include total results count value.
### IOrderedSearchOptionsBuilder\<TModel> WithQueryType(QueryType queryType)
Sets a value indicating the query type.
- queryType: The desired [Azure.Search.Documents.QueryType](https://docs.microsoft.com/en-us/dotnet/api/Azure.Search.Documents.QueryType).
### IOrderedSearchOptionsBuilder\<TModel> WithScoringParameter(string scoringParameter)
Sets a value indicating the values for each parameter defined in a scoring function.
- scoringParameter: The desired additional scoring parameter.
### IOrderedSearchOptionsBuilder\<TModel> WithScoringProfile(string scoringProfile)
Sets the name of a scoring profile to evaluate match scores for matching documents in order to sort the results.
- scoringProfile: The desired scoring profile name.
### IOrderedSearchOptionsBuilder\<TModel> WithSearchMode(SearchMode searchMode)
Sets a value indicating whether any or all of the search terms must be matched in order to count the document as a match.
- searchMode: The desired [Azure.Search.Documents.SearchMode](https://docs.microsoft.com/en-us/dotnet/api/Azure.Search.Documents.SearchMode).
### IOrderedSearchOptionsBuilder\<TModel> WithSelect\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of fields to include in the result set.
- lambdaExpression: An expression to extract a property.
```C#
.WithSelect(model => model.Property)
```
### IOrderedSearchOptionsBuilder\<TModel> WithSkip(int? skip)
Sets the number of search results to skip.
- skip: The desired skip value.
### IOrderedSearchOptionsBuilder\<TModel> WithThenBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithThenBy(model => model.Property)
```
### IOrderedSearchOptionsBuilder\<TModel> WithThenByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithThenByDescending(model => model.Property)
```
### IOrderedSearchOptionsBuilder\<TModel> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
```C#
.Where(model => model.Property == value)
```
### IOrderedSearchOptionsBuilder\<TModel> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### IOrderedSearchOptionsBuilder\<TModel> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### IOrderedSearchOptionsBuilder\<TModel> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### IOrderedSearchOptionsBuilder\<TModel> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
```C#
.WithSearchField(model => model.Property)
```
### IOrderedSearchOptionsBuilder\<TModel> WithSize(int? size)
Sets the number of items to retrieve. 
- size: The desired size value.





## [IOrderedSuggestOptionsBuilder](AzureSearchQueryBuilder/Builders/IOrderedSuggestOptionsBuilder.cs)\<TModel>
The [Azure.Search.Documents.SuggestOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.SuggestOptions)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### SuggestOptions Build()
Build a [Azure.Search.Documents.SuggestOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.SuggestOptions) object.
### ISuggestOptionsBuilder\<TModel> WithSelect\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of fields to include in the result set.
- lambdaExpression: An expression to extract a property.
```C#
.WithSelect(model => model.Property)
```
### IOrderedSuggestOptionsBuilder\<TModel> WithThenBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithThenBy(model => model.Property)
```
### IOrderedSuggestOptionsBuilder\<TModel> WithThenByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithThenByDescending(model => model.Property)
```
### IOrderedSuggestOptionsBuilder\<TModel> WithUseFuzzyMatching(bool useFuzzyMatching)
Sets the use fuzzy matching value.
- useFuzzyMatching: The desired fuzzy matching mode.
### IOrderedSuggestOptionsBuilder\<TModel> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
```C#
.Where(model => model.Property == value)
```
### IOrderedSuggestOptionsBuilder\<TModel> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### IOrderedSuggestOptionsBuilder\<TModel> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### IOrderedSuggestOptionsBuilder\<TModel> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### IOrderedSuggestOptionsBuilder\<TModel> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
```C#
.WithSearchField(model => model.Property)
```
### IOrderedSuggestOptionsBuilder\<TModel> WithSize(int? size)
Sets the number of items to retrieve. 
- size: The desired size value.





## [ISearchOptionsBuilder](AzureSearchQueryBuilder/Builders/ISearchOptionsBuilder.cs)\<TModel>
The [Azure.Search.Documents.SearchOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.searchOptions)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### SearchOptions Build()
Build a [Azure.Search.Documents.SearchOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.searchOptions) object.
### ISearchOptionsBuilder\<TModel> WithFacet\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a proprty to the collection of fields to facet by.
- lambdaExpression: An expression to extract a property.
```C#
.WithFacet(model => model.Property)
```
### ISearchOptionsBuilder\<TModel> WithHighlightField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of field names used for hit highlights.
- lambdaExpression: An expression to extract a property.
```C#
.WithHighlightField(model => model.Property)
```
### ISearchOptionsBuilder\<TModel> WithIncludeTotalCount(bool includeTotalCount)
Sets the value indicating whether to fetch the total count of results.
- includeTotalCount: The desired include total results count value.
### IOrderedSearchOptionsBuilder\<TModel> WithOrderBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithOrderBy(model => model.Property)
```
### IOrderedSearchOptionsBuilder\<TModel> WithOrderByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in descending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithOrderByDescending(model => model.Property)
```
### ISearchOptionsBuilder\<TModel> WithQueryType(QueryType queryType)
Sets a value indicating the query type.
- queryType: The desired [Azure.Search.Documents.QueryType](https://docs.microsoft.com/en-us/dotnet/api/Azure.Search.Documents.QueryType).
### ISearchOptionsBuilder\<TModel> WithScoringParameter(string scoringParameter)
Sets a value indicating the values for each parameter defined in a scoring function.
- scoringParameter: The desired additional scoring parameter.
### ISearchOptionsBuilder\<TModel> WithScoringProfile(string scoringProfile)
Sets the name of a scoring profile to evaluate match scores for matching documents in order to sort the results.
- scoringProfile: The desired scoring profile name.
### ISearchOptionsBuilder\<TModel> WithSearchMode(SearchMode searchMode)
Sets a value indicating whether any or all of the search terms must be matched in order to count the document as a match.
- searchMode: The desired [Azure.Search.Documents.SearchMode](https://docs.microsoft.com/en-us/dotnet/api/Azure.Search.Documents.SearchMode).
### ISearchOptionsBuilder\<TModel> WithSelect\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of fields to include in the result set.
- lambdaExpression: An expression to extract a property.
```C#
.WithSelect(model => model.Property)
```
### ISearchOptionsBuilder\<TModel> WithSkip(int? skip)
Sets the number of search results to skip.
- skip: The desired skip value.rom each element.
```C#
.WithThenByDescending(model => model.Property)
```
### ISearchOptionsBuilder\<TModel> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
```C#
.Where(model => model.Property == value)
```
### ISearchOptionsBuilder\<TModel> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### ISearchOptionsBuilder\<TModel> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### ISearchOptionsBuilder\<TModel> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### ISearchOptionsBuilder\<TModel> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
```C#
.WithSearchField(model => model.Property)
```
### ISearchOptionsBuilder\<TModel> WithSize(int? size)
Sets the number of items to retrieve. 
- size: The desired size value.





## [ISuggestOptionsBuilder](AzureSearchQueryBuilder/Builders/ISuggestOptionsBuilder.cs)\<TModel>
The [Azure.Search.Documents.SuggestOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.SuggestOptions)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### SuggestOptions Build()
Build a [Azure.Search.Documents.SuggestOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.SuggestOptions) object.
### IOrderedSuggestOptionsBuilder\<TModel> WithOrderBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithOrderBy(model => model.Property)
```
### IOrderedSuggestOptionsBuilder\<TModel> WithOrderByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in descending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithOrderByDescending(model => model.Property)
```
### ISuggestOptionsBuilder\<TModel> WithSelect\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of fields to include in the result set.
- lambdaExpression: An expression to extract a property.
```C#
.WithSelect(model => model.Property)
```
### ISuggestOptionsBuilder\<TModel> WithUseFuzzyMatching(bool useFuzzyMatching)
Sets the use fuzzy matching value.
- useFuzzyMatching: The desired fuzzy matching mode.
### ISuggestOptionsBuilder\<TModel> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
```C#
.Where(model => model.Property == value)
```
### ISuggestOptionsBuilder\<TModel> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### ISuggestOptionsBuilder\<TModel> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### ISuggestOptionsBuilder\<TModel> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### ISuggestOptionsBuilder\<TModel> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
```C#
.WithSearchField(model => model.Property)
```
### ISuggestOptionsBuilder\<TModel> WithSize(int? size)
Sets the number of items to retrieve. 
- size: The desired size value.





## [SearchOptionsBuilder](AzureSearchQueryBuilder/Builders/SearchOptionsBuilder.cs)\<TModel>
The [Azure.Search.Documents.SearchOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.searchOptions)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### static ISearchOptionsBuilder\<TModel> Create()
Create a new ISearchOptionsBuilder\<TModel>.
### Example using models from [How to use Azure Search from a .NET Application](https://docs.microsoft.com/en-us/azure/search/search-howto-dotnet-sdk#how-the-net-sdk-handles-documents)
```C#
SearchOptionsBuilder<Hotel>.Create()
  .Where(hotel => hotel.HotelId == "Some ID" || hotel.HotelName == "Some Name")
  .Where(hotel => hotel.ParkingIncluded == true)
  .WithFacet(hotel => hotel.Address.City)
  .WithFacet(hotel => hotel.Address.StateProvince)
  .WithHighlightField(hotel => hotel.Category)
  .WithHighlightField(hotel => hotel.Rating)
  .WithHighlightPostTag("Post Tag")
  .WithHighlightPreTag("Pre Tag")
  .WithIncludeTotalCount(true)
  .WithMinimumCoverage(0.75)
  .WithOrderBy(hotel => hotel.HotelName)
  .WithQueryType(QueryType.Full)
  .WithScoringParameter(new ScoringParameter("First", new string[] { "one", "two", }))
  .WithScoringParameter(new ScoringParameter("Second", new string[] { "three", "four", }))
  .WithScoringProfile("Scoring Profile Name")
  .WithSearchField(hotel => hotel.HotelName)
  .WithSearchField(hotel => hotel.Description)
  .WithSearchMode(SearchMode.Any)
  .WithSelect(hotel => hotel.HotelName)
  .WithSelect(hotel => hotel.Address.Country)
  .WithSkip(20)
  .WithThenByDescending(hotel => hotel.LastRenovationDate)
  .WithSize(10)
  .Build();
```
| Property                 | Value                                                                                |
| ------------------------ | ------------------------------------------------------------------------------------ |
| Facets                   | Adress/City, Address/StateProvince                                                   |
| Filter                   | ((HotelId eq 'Some ID') or (HotelName eq 'Some Name')) and (ParkingIncluded eq true) |
| HighlightFields          | Category, Rating                                                                     |
| HighlightPostTag         | Post Tag                                                                             |
| HighlightPreTag          | Pre Tag                                                                              |
| IncludeTotalCount        | True                                                                                 |
| MinimumCoverage          | 0.75                                                                                 |
| OrderBy                  | HotelName asc, LastRenovationDate desc                                               |
| QueryType                | Full                                                                                 |
| ScoringParameters        | First (one, two), Second (three, four)                                               |
| ScoringProfile           | Scoring Profile Name                                                                 |
| SearchFields             | HotelName, Description                                                               |
| SearchMode               | Any                                                                                  |
| Select                   | HotelName, Address/Country                                                           |
| Skip                     | 20                                                                                   |
| Size                      | 10                                                                                   |





## [SuggestOptionsBuilder](AzureSearchQueryBuilder/Builders/SuggestOptionsBuilder.cs)\<TModel>
The [Azure.Search.Documents.SuggestOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.search.documents.SuggestOptions)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### static ISuggestOptionsBuilder\<TModel> Create()
Create a new ISuggestOptionsBuilder\<TModel>.
### Example using models from [How to use Azure Search from a .NET Application](https://docs.microsoft.com/en-us/azure/search/search-howto-dotnet-sdk#how-the-net-sdk-handles-documents)
```C#
SuggestOptionsBuilder<Hotel>.Create()
  .WithHighlightPostTag("Post Tag")
  .WithHighlightPreTag("Pre Tag")
  .WithMinimumCoverage(0.75)
  .WithOrderBy(hotel => hotel.HotelName)
  .WithSearchField(hotel => hotel.HotelName)
  .WithSearchField(hotel => hotel.Description)
  .WithSelect(hotel => hotel.HotelName)
  .WithSelect(hotel => hotel.Address.Country)
  .WithThenByDescending(hotel => hotel.LastRenovationDate)
  .WithSize(10)
  .WithUseFuzzyMatching(true)
  .Where(hotel => hotel.HotelId == "Some ID" || hotel.HotelName == "Some Name")
  .Where(hotel => hotel.ParkingIncluded == true)
  .Build();
```
| Property         | Value                                                                                |
| ---------------- | ------------------------------------------------------------------------------------ |
| Filter           | ((HotelId eq 'Some ID') or (HotelName eq 'Some Name')) and (ParkingIncluded eq true) |
| HighlightPostTag | Post Tag                                                                             |
| HighlightPreTag  | Pre Tag                                                                              |
| MinimumCoverage  | 0.75                                                                                 |
| OrderBy          | HotelName asc, LastRenovationDate desc                                               |
| SearchFields     | HotelName, Description                                                               |
| Select           | HotelName, Address/Country                                                           |
| Size              | 10                                                                                   |
| UseFuzzyMatching | True                                                                                 |





## [SearchFns](AzureSearchQueryBuilder/Builders/SearchFns.cs)
SearchFns provides access to search.in, search.ismatch, search.ismatchscoring, and search.score functions from the builder interfaces.
```C#
SuggestOptionsBuilder<Hotel>.Create()
  .WithOrderBy(hotel => hotel.HotelName)
  .WithThenByBy(hotel => SearchFns.Score())
  .Where(hotel => hotel.HotelId == "Some ID" || hotel.HotelName == "Some Name")
  .Where(hotel => SearchFns.Score() >= 0.5)
  .Where(hotel => SearchFns.In(hotel.Name, "One", "Two", "Three"))
  .Where(hotel => SearchFns.IsMatch("Hotel One", hotel.Name, hotel.Description))
  .Build();
```
