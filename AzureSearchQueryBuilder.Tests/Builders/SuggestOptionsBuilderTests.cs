using System.Linq;
using Azure.Search.Documents;
using AzureSearchQueryBuilder.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSearchQueryBuilder.Tests.Builders
{
    [TestClass]
    public class SuggestOptionsBuilderTests : OptionsBuilderTests<SuggestOptions>
    {
        private static readonly JsonSerializerSettings __jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        [TestMethod]
        public void SearchPropertyNameUtility_OrderBy()
        {
            ISuggestOptionsBuilder<Model> suggestOptionsBuilder = SuggestOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsNull(suggestOptionsBuilder.OrderBy);

            suggestOptionsBuilder.WithOrderBy(_ => SearchFns.Score()).WithThenByDescending(_ => SearchFns.Score());

            Assert.IsNotNull(suggestOptionsBuilder.OrderBy);
            Assert.AreEqual(2, suggestOptionsBuilder.OrderBy.Count());
            Assert.AreEqual("search.score() asc", suggestOptionsBuilder.OrderBy.ElementAtOrDefault(0));
            Assert.AreEqual("search.score() desc", suggestOptionsBuilder.OrderBy.ElementAtOrDefault(1));

            SuggestOptions Options = suggestOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.IsNotNull(Options.OrderBy);
            Assert.AreEqual(2, Options.OrderBy.Count());
            Assert.AreEqual("search.score() asc", Options.OrderBy.ElementAtOrDefault(0));
            Assert.AreEqual("search.score() desc", Options.OrderBy.ElementAtOrDefault(1));

            suggestOptionsBuilder.WithOrderByDescending(_ => SearchFns.Score()).WithThenBy(_ => SearchFns.Score());

            Assert.IsNotNull(suggestOptionsBuilder.OrderBy);
            Assert.AreEqual(2, suggestOptionsBuilder.OrderBy.Count());
            Assert.AreEqual("search.score() desc", suggestOptionsBuilder.OrderBy.ElementAtOrDefault(0));
            Assert.AreEqual("search.score() asc", suggestOptionsBuilder.OrderBy.ElementAtOrDefault(1));

            Options = suggestOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.IsNotNull(Options.OrderBy);
            Assert.AreEqual(2, Options.OrderBy.Count());
            Assert.AreEqual("search.score() desc", Options.OrderBy.ElementAtOrDefault(0));
            Assert.AreEqual("search.score() asc", Options.OrderBy.ElementAtOrDefault(1));
        }

        [TestMethod]
        public void SearchPropertyNameUtility_Select()
        {
            ISuggestOptionsBuilder<Model> suggestOptionsBuilder = SuggestOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsNull(suggestOptionsBuilder.Select);

            suggestOptionsBuilder.WithSelect(_ => SearchFns.Score());

            Assert.IsNotNull(suggestOptionsBuilder.Select);
            Assert.AreEqual(1, suggestOptionsBuilder.Select.Count());
            Assert.AreEqual("search.score()", suggestOptionsBuilder.Select.ElementAtOrDefault(0));

            SuggestOptions Options = suggestOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.IsNotNull(Options.Select);
            Assert.AreEqual(1, Options.Select.Count());
            Assert.AreEqual("search.score()", Options.Select.ElementAtOrDefault(0));
        }

        [TestMethod]
        public void SuggestPropertyNameUtility_UseFuzzyMatching()
        {
            ISuggestOptionsBuilder<Model> suggestOptionsBuilder = SuggestOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsFalse(suggestOptionsBuilder.UseFuzzyMatching);

            suggestOptionsBuilder.WithUseFuzzyMatching(true);

            Assert.IsNotNull(suggestOptionsBuilder.UseFuzzyMatching);
            Assert.IsTrue(suggestOptionsBuilder.UseFuzzyMatching);

            SuggestOptions Options = suggestOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.IsTrue(Options.UseFuzzyMatching);
        }

        protected override IOptionsBuilder<Model, SuggestOptions> ConstructBuilder()
        {
            return (SuggestOptionsBuilder<Model>)SuggestOptionsBuilder<Model>.Create(__jsonSerializerSettings);
        }
    }
}
