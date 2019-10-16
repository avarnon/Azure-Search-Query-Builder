# Azure-Search-Query-Builder
This is a library that uses expression tree parsing to build a parameters object for performing search, suggest, and autocomplete actions with the Azure Search .NET SDK.





## [AutocompleteParametersBuilder](AzureSearchQueryBuilder/Builders/AutocompleteParametersBuilder.cs)\<TModel>
The [Microsoft.Azure.Search.Models.AutocompleteParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.AutocompleteParameters)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### static IAutocompleteParametersBuilder\<TModel> Create()
Create a new IAutocompleteParametersBuilder\<TModel>.
### Example using models from [How to use Azure Search from a .NET Application](https://docs.microsoft.com/en-us/azure/search/search-howto-dotnet-sdk#how-the-net-sdk-handles-documents)
```C#
AutocompleteParametersBuilder<Hotel>.Create()
  .Where(hotel => hotel.HotelId == "Some ID" || hotel.HotelName == "Some Name")
  .Where(hotel => hotel.ParkingIncluded == true)
  .WithAutocompleteMode(AutocompleteMode.OneTerm)
  .WithHighlightPostTag("Post Tag")
  .WithHighlightPreTag("Pre Tag")
  .WithMinimumCoverage(0.75)
  .WithSearchField(hotel => hotel.HotelName)
  .WithSearchField(hotel => hotel.Description)
  .WithTop(10)
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
| Top              | 10                                                                                   |
| UseFuzzyMatching | True                                                                                 |




## [IAutocompleteParametersBuilder](AzureSearchQueryBuilder/Builders/IAutocompleteParametersBuilder.cs)\<TModel>
The [Microsoft.Azure.Search.Models.AutocompleteParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.AutocompleteParameters)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### AutocompleteParameters Build()
Build a [Microsoft.Azure.Search.Models.AutocompleteParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.AutocompleteParameters) object.
### IAutocompleteParametersBuilder\<TModel> WithAutocompleteMode(AutocompleteMode autocompleteMode)
Sets the autocomplete mode.
- autocompleteMode: The desired [Microsoft.Azure.Search.Models.AutocompleteMode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.AutocompleteMode).
### IAutocompleteParametersBuilder\<TModel> WithUseFuzzyMatching(bool? useFuzzyMatching)
Sets the use fuzzy matching value.
- useFuzzyMatching: The desired fuzzy matching mode.
### IAutocompleteParametersBuilder\<TModel> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
```C#
.Where(model => model.Property == value)
```
### IAutocompleteParametersBuilder\<TModel> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### IAutocompleteParametersBuilder\<TModel> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### IAutocompleteParametersBuilder\<TModel> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### IAutocompleteParametersBuilder\<TModel> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
```C#
.WithSearchField(model => model.Property)
```
### IAutocompleteParametersBuilder\<TModel> WithTop(int? top)
Sets the number of items to retrieve. 
- top: The desired top value.





## [IOrderedSearchParametersBuilder](AzureSearchQueryBuilder/Builders/IOrderedSearchParametersBuilder.cs)\<TModel>
The [Microsoft.Azure.Search.Models.SearchParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.searchparameters)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### SearchParameters Build()
Build a [Microsoft.Azure.Search.Models.SearchParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.searchparameters) object.
### IOrderedSearchParametersBuilder\<TModel> WithFacet\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a proprty to the collection of fields to facet by.
- lambdaExpression: An expression to extract a property.
```C#
.WithFacet(model => model.Property)
```
### IOrderedSearchParametersBuilder\<TModel> WithHighlightField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of field names used for hit highlights.
- lambdaExpression: An expression to extract a property.
```C#
.WithHighlightField(model => model.Property)
```
### IOrderedSearchParametersBuilder\<TModel> WithIncludeTotalResultCount(bool includeTotalResultCount)
Sets the value indicating whether to fetch the total count of results.
- includeTotalResultCount: The desired include total results count value.
### IOrderedSearchParametersBuilder\<TModel> WithQueryType(QueryType queryType)
Sets a value indicating the query type.
- queryType: The desired [Microsoft.Azure.Search.Models.QueryType](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Azure.Search.Models.QueryType).
### IOrderedSearchParametersBuilder\<TModel> WithScoringParameter(ScoringParameter scoringParameter)
Sets a value indicating the values for each parameter defined in a scoring function.
- scoringParameter: The desired additional [Microsoft.Azure.Search.Models.ScoringParameter](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Azure.Search.Models.ScoringParameter).
### IOrderedSearchParametersBuilder\<TModel> WithScoringProfile(string scoringProfile)
Sets the name of a scoring profile to evaluate match scores for matching documents in order to sort the results.
- scoringProfile: The desired scoring profile name.
### IOrderedSearchParametersBuilder\<TModel> WithSearchMode(SearchMode searchMode)
Sets a value indicating whether any or all of the search terms must be matched in order to count the document as a match.
- searchMode: The desired [Microsoft.Azure.Search.Models.SearchMode](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Azure.Search.Models.SearchMode).
### IOrderedSearchParametersBuilder\<TModel> WithSelect\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of fields to include in the result set.
- lambdaExpression: An expression to extract a property.
```C#
.WithSelect(model => model.Property)
```
### IOrderedSearchParametersBuilder\<TModel> WithSkip(int? skip)
Sets the number of search results to skip.
- skip: The desired skip value.
### IOrderedSearchParametersBuilder\<TModel> WithThenBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithThenBy(model => model.Property)
```
### IOrderedSearchParametersBuilder\<TModel> WithThenByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithThenByDescending(model => model.Property)
```
### IOrderedSearchParametersBuilder\<TModel> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
```C#
.Where(model => model.Property == value)
```
### IOrderedSearchParametersBuilder\<TModel> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### IOrderedSearchParametersBuilder\<TModel> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### IOrderedSearchParametersBuilder\<TModel> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### IOrderedSearchParametersBuilder\<TModel> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
```C#
.WithSearchField(model => model.Property)
```
### IOrderedSearchParametersBuilder\<TModel> WithTop(int? top)
Sets the number of items to retrieve. 
- top: The desired top value.





## [IOrderedSuggestParametersBuilder](AzureSearchQueryBuilder/Builders/IOrderedSuggestParametersBuilder.cs)\<TModel>
The [Microsoft.Azure.Search.Models.SuggestParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.SuggestParameters)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### SuggestParameters Build()
Build a [Microsoft.Azure.Search.Models.SuggestParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.SuggestParameters) object.
### ISuggestParametersBuilder\<TModel> WithSelect\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of fields to include in the result set.
- lambdaExpression: An expression to extract a property.
```C#
.WithSelect(model => model.Property)
```
### IOrderedSuggestParametersBuilder\<TModel> WithThenBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithThenBy(model => model.Property)
```
### IOrderedSuggestParametersBuilder\<TModel> WithThenByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for performing a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithThenByDescending(model => model.Property)
```
### IOrderedSuggestParametersBuilder\<TModel> WithUseFuzzyMatching(bool useFuzzyMatching)
Sets the use fuzzy matching value.
- useFuzzyMatching: The desired fuzzy matching mode.
### IOrderedSuggestParametersBuilder\<TModel> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
```C#
.Where(model => model.Property == value)
```
### IOrderedSuggestParametersBuilder\<TModel> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### IOrderedSuggestParametersBuilder\<TModel> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### IOrderedSuggestParametersBuilder\<TModel> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### IOrderedSuggestParametersBuilder\<TModel> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
```C#
.WithSearchField(model => model.Property)
```
### IOrderedSuggestParametersBuilder\<TModel> WithTop(int? top)
Sets the number of items to retrieve. 
- top: The desired top value.





## [ISearchParametersBuilder](AzureSearchQueryBuilder/Builders/ISearchParametersBuilder.cs)\<TModel>
The [Microsoft.Azure.Search.Models.SearchParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.searchparameters)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### SearchParameters Build()
Build a [Microsoft.Azure.Search.Models.SearchParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.searchparameters) object.
### ISearchParametersBuilder\<TModel> WithFacet\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a proprty to the collection of fields to facet by.
- lambdaExpression: An expression to extract a property.
```C#
.WithFacet(model => model.Property)
```
### ISearchParametersBuilder\<TModel> WithHighlightField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of field names used for hit highlights.
- lambdaExpression: An expression to extract a property.
```C#
.WithHighlightField(model => model.Property)
```
### ISearchParametersBuilder\<TModel> WithIncludeTotalResultCount(bool includeTotalResultCount)
Sets the value indicating whether to fetch the total count of results.
- includeTotalResultCount: The desired include total results count value.
### IOrderedSearchParametersBuilder\<TModel> WithOrderBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithOrderBy(model => model.Property)
```
### IOrderedSearchParametersBuilder\<TModel> WithOrderByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in descending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithOrderByDescending(model => model.Property)
```
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
```C#
.WithSelect(model => model.Property)
```
### ISearchParametersBuilder\<TModel> WithSkip(int? skip)
Sets the number of search results to skip.
- skip: The desired skip value.rom each element.
```C#
.WithThenByDescending(model => model.Property)
```
### ISearchParametersBuilder\<TModel> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
```C#
.Where(model => model.Property == value)
```
### ISearchParametersBuilder\<TModel> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### ISearchParametersBuilder\<TModel> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### ISearchParametersBuilder\<TModel> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### ISearchParametersBuilder\<TModel> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
```C#
.WithSearchField(model => model.Property)
```
### ISearchParametersBuilder\<TModel> WithTop(int? top)
Sets the number of items to retrieve. 
- top: The desired top value.





## [ISuggestParametersBuilder](AzureSearchQueryBuilder/Builders/ISuggestParametersBuilder.cs)\<TModel>
The [Microsoft.Azure.Search.Models.SuggestParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.SuggestParameters)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### SuggestParameters Build()
Build a [Microsoft.Azure.Search.Models.SuggestParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.SuggestParameters) object.
### IOrderedSuggestParametersBuilder\<TModel> WithOrderBy\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in ascending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithOrderBy(model => model.Property)
```
### IOrderedSuggestParametersBuilder\<TModel> WithOrderByDescending\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Uses an expression for sorting the elements of a sequence in descending order using a specified comparer.
- lambdaExpression: An expression to extract a property from each element.
```C#
.WithOrderByDescending(model => model.Property)
```
### ISuggestParametersBuilder\<TModel> WithSelect\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Adds a property to the collection of fields to include in the result set.
- lambdaExpression: An expression to extract a property.
```C#
.WithSelect(model => model.Property)
```
### ISuggestParametersBuilder\<TModel> WithUseFuzzyMatching(bool useFuzzyMatching)
Sets the use fuzzy matching value.
- useFuzzyMatching: The desired fuzzy matching mode.
### ISuggestParametersBuilder\<TModel> Where(Expression<BooleanLambdaDelegate\<TModel>> lambdaExpression)
Adds a where clause to the filter expression.
- lambdaExpression: The lambda expression used to generate a filter expression.
```C#
.Where(model => model.Property == value)
```
### ISuggestParametersBuilder\<TModel> WithHighlightPostTag(string highlightPostTag)
Sets the string tag that appends to search hits.
- highlightPostTag: the desired tag.
### ISuggestParametersBuilder\<TModel> WithHighlightPreTag(string highlightPreTag)
Sets the string tag that prepends to search hits.
- highlightPreTag: the desired tag.
### ISuggestParametersBuilder\<TModel> WithMinimumCoverage(double? minimumCoverage)
Sets a number between 0 and 100 indicating the percentage of the index that must be covered by a query in order for the query to be reported as a success. 
- minimumCoverage: The desired minimum coverage.
### ISuggestParametersBuilder\<TModel> WithSearchField\<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
Appends to the list of field names to search for the specified search text.
- lambdaExpression: The lambda expression representing the search field.
```C#
.WithSearchField(model => model.Property)
```
### ISuggestParametersBuilder\<TModel> WithTop(int? top)
Sets the number of items to retrieve. 
- top: The desired top value.





## [SearchParametersBuilder](AzureSearchQueryBuilder/Builders/SearchParametersBuilder.cs)\<TModel>
The [Microsoft.Azure.Search.Models.SearchParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.searchparameters)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### static ISearchParametersBuilder\<TModel> Create()
Create a new ISearchParametersBuilder\<TModel>.
### Example using models from [How to use Azure Search from a .NET Application](https://docs.microsoft.com/en-us/azure/search/search-howto-dotnet-sdk#how-the-net-sdk-handles-documents)
```C#
SearchParametersBuilder<Hotel>.Create()
  .Where(hotel => hotel.HotelId == "Some ID" || hotel.HotelName == "Some Name")
  .Where(hotel => hotel.ParkingIncluded == true)
  .WithFacet(hotel => hotel.Address.City)
  .WithFacet(hotel => hotel.Address.StateProvince)
  .WithHighlightField(hotel => hotel.Category)
  .WithHighlightField(hotel => hotel.Rating)
  .WithHighlightPostTag("Post Tag")
  .WithHighlightPreTag("Pre Tag")
  .WithIncludeTotalResultCount(true)
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
  .WithTop(10)
  .Build();
```
| Property                 | Value                                                                                |
| ------------------------ | ------------------------------------------------------------------------------------ |
| Facets                   | Adress/City, Address/StateProvince                                                   |
| Filter                   | ((HotelId eq 'Some ID') or (HotelName eq 'Some Name')) and (ParkingIncluded eq true) |
| HighlightFields          | Category, Rating                                                                     |
| HighlightPostTag         | Post Tag                                                                             |
| HighlightPreTag          | Pre Tag                                                                              |
| IncludeTotalResultCount  | True                                                                                 |
| MinimumCoverage          | 0.75                                                                                 |
| OrderBy                  | HotelName asc, LastRenovationDate desc                                               |
| QueryType                | Full                                                                                 |
| ScoringParameters        | First (one, two), Second (three, four)                                               |
| ScoringProfile           | Scoring Profile Name                                                                 |
| SearchFields             | HotelName, Description                                                               |
| SearchMode               | Any                                                                                  |
| Select                   | HotelName, Address/Country                                                           |
| Skip                     | 20                                                                                   |
| Top                      | 10                                                                                   |





## [SuggestParametersBuilder](AzureSearchQueryBuilder/Builders/SuggestParametersBuilder.cs)\<TModel>
The [Microsoft.Azure.Search.Models.SuggestParameters](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.search.models.SuggestParameters)\<TModel> builder.
> TModel is the class the prepresents a document in the search index.
### static ISuggestParametersBuilder\<TModel> Create()
Create a new ISuggestParametersBuilder\<TModel>.
### Example using models from [How to use Azure Search from a .NET Application](https://docs.microsoft.com/en-us/azure/search/search-howto-dotnet-sdk#how-the-net-sdk-handles-documents)
```C#
SuggestParametersBuilder<Hotel>.Create()
  .WithHighlightPostTag("Post Tag")
  .WithHighlightPreTag("Pre Tag")
  .WithMinimumCoverage(0.75)
  .WithOrderBy(hotel => hotel.HotelName)
  .WithSearchField(hotel => hotel.HotelName)
  .WithSearchField(hotel => hotel.Description)
  .WithSelect(hotel => hotel.HotelName)
  .WithSelect(hotel => hotel.Address.Country)
  .WithThenByDescending(hotel => hotel.LastRenovationDate)
  .WithTop(10)
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
| Top              | 10                                                                                   |
| UseFuzzyMatching | True                                                                                 |
