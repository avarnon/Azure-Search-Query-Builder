using AzureSearchQueryBuilder.Builders;
using Microsoft.Azure.Search.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureSearchQueryBuilder.Tests.Builders
{
    [TestClass]
    public class AutocompleteParametersBuilderTests : ParametersBuilderTests<TestModel, AutocompleteParameters>
    {
        [TestMethod]
        public void AutocompleteParametersBuilder_AutocompleteMode()
        {
            IAutocompleteParametersBuilder<TestModel> autocompleteParametersBuilder = AutocompleteParametersBuilder<TestModel>.Create();

            Assert.AreEqual(AutocompleteMode.OneTerm, autocompleteParametersBuilder.AutocompleteMode);

            autocompleteParametersBuilder.WithAutocompleteMode(AutocompleteMode.OneTermWithContext);

            Assert.IsNotNull(autocompleteParametersBuilder.AutocompleteMode);
            Assert.AreEqual(AutocompleteMode.OneTermWithContext, autocompleteParametersBuilder.AutocompleteMode);

            AutocompleteParameters parameters = autocompleteParametersBuilder.Build();
            Assert.IsNotNull(parameters);
            Assert.AreEqual(AutocompleteMode.OneTermWithContext, parameters.AutocompleteMode);
        }

        [TestMethod]
        public void AutocompleteParametersBuilder_UseFuzzyMatching()
        {
            IAutocompleteParametersBuilder<TestModel> autocompleteParametersBuilder = AutocompleteParametersBuilder<TestModel>.Create();

            Assert.IsNull(autocompleteParametersBuilder.UseFuzzyMatching);

            autocompleteParametersBuilder.WithUseFuzzyMatching(true);

            Assert.IsNotNull(autocompleteParametersBuilder.UseFuzzyMatching);
            Assert.AreEqual(true, autocompleteParametersBuilder.UseFuzzyMatching);

            AutocompleteParameters parameters = autocompleteParametersBuilder.Build();
            Assert.IsNotNull(parameters);
            Assert.AreEqual(true, parameters.UseFuzzyMatching);
        }

        protected override IParametersBuilder<TestModel, AutocompleteParameters> ConstructBuilder()
        {
            return AutocompleteParametersBuilder<TestModel>.Create();
        }
    }
}