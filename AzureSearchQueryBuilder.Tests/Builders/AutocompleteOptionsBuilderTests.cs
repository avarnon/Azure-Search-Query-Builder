using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using AzureSearchQueryBuilder.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSearchQueryBuilder.Tests.Builders
{
    [TestClass]
    public class AutocompleteOptionsBuilderTests : OptionsBuilderTests<AutocompleteOptions>
    {
        private static readonly JsonSerializerSettings __jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        [TestMethod]
        public void AutocompletePropertyNameUtility_AutocompleteMode()
        {
            IAutocompleteOptionsBuilder<Model> AutocompleteOptionsBuilder = AutocompleteOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.AreEqual(AutocompleteMode.OneTerm, AutocompleteOptionsBuilder.Mode);

            AutocompleteOptionsBuilder.WithMode(AutocompleteMode.OneTermWithContext);

            Assert.IsNotNull(AutocompleteOptionsBuilder.Mode);
            Assert.AreEqual(AutocompleteMode.OneTermWithContext, AutocompleteOptionsBuilder.Mode);

            AutocompleteOptions Options = AutocompleteOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.AreEqual(AutocompleteMode.OneTermWithContext, Options.Mode);
        }

        [TestMethod]
        public void AutocompletePropertyNameUtility_UseFuzzyMatching()
        {
            IAutocompleteOptionsBuilder<Model> AutocompleteOptionsBuilder = AutocompleteOptionsBuilder<Model>.Create(__jsonSerializerSettings);

            Assert.IsNull(AutocompleteOptionsBuilder.UseFuzzyMatching);

            AutocompleteOptionsBuilder.WithUseFuzzyMatching(true);

            Assert.IsNotNull(AutocompleteOptionsBuilder.UseFuzzyMatching);
            Assert.AreEqual(true, AutocompleteOptionsBuilder.UseFuzzyMatching);

            AutocompleteOptions Options = AutocompleteOptionsBuilder.Build();
            Assert.IsNotNull(Options);
            Assert.AreEqual(true, Options.UseFuzzyMatching);
        }

        protected override IOptionsBuilder<Model, AutocompleteOptions> ConstructBuilder()
        {
            return (AutocompleteOptionsBuilder<Model>)AutocompleteOptionsBuilder<Model>.Create(__jsonSerializerSettings);
        }
    }
}
