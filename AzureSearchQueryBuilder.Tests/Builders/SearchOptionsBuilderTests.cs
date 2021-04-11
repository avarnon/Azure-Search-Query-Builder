using System.Linq;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using AzureSearchQueryBuilder.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSearchQueryBuilder.Tests.Builders
{
    [TestClass]
    public class SearchOptionsBuilderTests : OptionsBuilderTests<SearchOptions>
    {
        private static readonly JsonSerializerSettings __jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };


        [TestMethod]
        public void SearchPropertyNameUtility_Facets()
        {
            ISearchOptionsBuilder<Model> searchOptionsBuilder = SearchOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsNull(searchOptionsBuilder.Facets);

            searchOptionsBuilder.WithFacet(_ => SearchFns.Score());

            Assert.IsNotNull(searchOptionsBuilder.Facets);
            Assert.AreEqual(1, searchOptionsBuilder.Facets.Count());
            Assert.AreEqual("search.score()", searchOptionsBuilder.Facets.ElementAtOrDefault(0));

            SearchOptions Options = searchOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.IsNotNull(Options.Facets);
            Assert.AreEqual(1, Options.Facets.Count());
            Assert.AreEqual("search.score()", Options.Facets.ElementAtOrDefault(0));
        }

        [TestMethod]
        public void SearchPropertyNameUtility_HighlightFields()
        {
            ISearchOptionsBuilder<Model> searchOptionsBuilder = SearchOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsNull(searchOptionsBuilder.HighlightFields);

            searchOptionsBuilder.WithHighlightField(_ => SearchFns.Score());

            Assert.IsNotNull(searchOptionsBuilder.HighlightFields);
            Assert.AreEqual(1, searchOptionsBuilder.HighlightFields.Count());
            Assert.AreEqual("search.score()", searchOptionsBuilder.HighlightFields.ElementAtOrDefault(0));

            SearchOptions Options = searchOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.IsNotNull(Options.HighlightFields);
            Assert.AreEqual(1, Options.HighlightFields.Count());
            Assert.AreEqual("search.score()", Options.HighlightFields.ElementAtOrDefault(0));
        }

        [TestMethod]
        public void SearchPropertyNameUtility_IncludeTotalResultCount()
        {
            ISearchOptionsBuilder<Model> searchOptionsBuilder = SearchOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsFalse(searchOptionsBuilder.IncludeTotalCount);

            searchOptionsBuilder.WithIncludeTotalCount(true);

            Assert.IsTrue(searchOptionsBuilder.IncludeTotalCount);

            SearchOptions Options = searchOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.IsTrue(Options.IncludeTotalCount);
        }

        [TestMethod]
        public void SearchPropertyNameUtility_OrderBy()
        {
            ISearchOptionsBuilder<Model> searchOptionsBuilder = SearchOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsNull(searchOptionsBuilder.OrderBy);

            searchOptionsBuilder.WithOrderBy(_ => SearchFns.Score()).WithThenByDescending(_ => SearchFns.Score());

            Assert.IsNotNull(searchOptionsBuilder.OrderBy);
            Assert.AreEqual(2, searchOptionsBuilder.OrderBy.Count());
            Assert.AreEqual("search.score() asc", searchOptionsBuilder.OrderBy.ElementAtOrDefault(0));
            Assert.AreEqual("search.score() desc", searchOptionsBuilder.OrderBy.ElementAtOrDefault(1));

            SearchOptions Options = searchOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.IsNotNull(Options.OrderBy);
            Assert.AreEqual(2, Options.OrderBy.Count());
            Assert.AreEqual("search.score() asc", Options.OrderBy.ElementAtOrDefault(0));
            Assert.AreEqual("search.score() desc", Options.OrderBy.ElementAtOrDefault(1));

            searchOptionsBuilder.WithOrderByDescending(_ => SearchFns.Score()).WithThenBy(_ => SearchFns.Score());

            Assert.IsNotNull(searchOptionsBuilder.OrderBy);
            Assert.AreEqual(2, searchOptionsBuilder.OrderBy.Count());
            Assert.AreEqual("search.score() desc", searchOptionsBuilder.OrderBy.ElementAtOrDefault(0));
            Assert.AreEqual("search.score() asc", searchOptionsBuilder.OrderBy.ElementAtOrDefault(1));

            Options = searchOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.IsNotNull(Options.OrderBy);
            Assert.AreEqual(2, Options.OrderBy.Count());
            Assert.AreEqual("search.score() desc", Options.OrderBy.ElementAtOrDefault(0));
            Assert.AreEqual("search.score() asc", Options.OrderBy.ElementAtOrDefault(1));
        }

        [TestMethod]
        public void SearchPropertyNameUtility_ScoringParameters()
        {
            ISearchOptionsBuilder<Model> searchOptionsBuilder = SearchOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsNull(searchOptionsBuilder.ScoringParameters);

            searchOptionsBuilder.WithScoringParameter("foo");

            Assert.IsNotNull(searchOptionsBuilder.ScoringParameters);
            Assert.AreEqual(1, searchOptionsBuilder.ScoringParameters.Count());

            SearchOptions Options = searchOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.IsNotNull(Options.ScoringParameters);
            Assert.AreEqual(1, Options.ScoringParameters.Count());
        }

        [TestMethod]
        public void SearchPropertyNameUtility_ScoringProfile()
        {
            ISearchOptionsBuilder<Model> searchOptionsBuilder = SearchOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsNull(searchOptionsBuilder.ScoringProfile);

            searchOptionsBuilder.WithScoringProfile("test");

            Assert.IsNotNull(searchOptionsBuilder.ScoringProfile);
            Assert.AreEqual("test", searchOptionsBuilder.ScoringProfile);

            SearchOptions Options = searchOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.AreEqual("test", Options.ScoringProfile);
        }

        [TestMethod]
        public void SearchPropertyNameUtility_SearchMode()
        {
            ISearchOptionsBuilder<Model> searchOptionsBuilder = SearchOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.AreEqual(SearchMode.Any, searchOptionsBuilder.SearchMode);

            searchOptionsBuilder.WithSearchMode(SearchMode.All);

            Assert.AreEqual(SearchMode.All, searchOptionsBuilder.SearchMode);

            SearchOptions Options = searchOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.AreEqual(SearchMode.All, Options.SearchMode);
        }

        [TestMethod]
        public void SearchPropertyNameUtility_Select()
        {
            ISearchOptionsBuilder<Model> searchOptionsBuilder = SearchOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsNull(searchOptionsBuilder.Select);

            searchOptionsBuilder.WithSelect(_ => SearchFns.Score());

            Assert.IsNotNull(searchOptionsBuilder.Select);
            Assert.AreEqual(1, searchOptionsBuilder.Select.Count());
            Assert.AreEqual("search.score()", searchOptionsBuilder.Select.ElementAtOrDefault(0));

            SearchOptions Options = searchOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.IsNotNull(Options.Select);
            Assert.AreEqual(1, Options.Select.Count());
            Assert.AreEqual("search.score()", Options.Select.ElementAtOrDefault(0));
        }

        [TestMethod]
        public void SearchPropertyNameUtility_Skip()
        {
            ISearchOptionsBuilder<Model> searchOptionsBuilder = SearchOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsNull(searchOptionsBuilder.Skip);

            searchOptionsBuilder.WithSkip(1);

            Assert.IsNotNull(searchOptionsBuilder.Skip);
            Assert.AreEqual(1, searchOptionsBuilder.Skip);

            SearchOptions Options = searchOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.AreEqual(1, Options.Skip);
        }

        protected override IOptionsBuilder<Model, SearchOptions> ConstructBuilder()
        {
            return (SearchOptionsBuilder<Model>)SearchOptionsBuilder<Model>.Create(__jsonSerializerSettings);
        }
    }
}
