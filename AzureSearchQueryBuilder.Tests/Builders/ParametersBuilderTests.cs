using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AzureSearchQueryBuilder.Builders;
using AzureSearchQueryBuilder.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureSearchQueryBuilder.Tests.Builders
{
    [TestClass]
    public abstract class ParametersBuilderTests<TModel, TParameters>
        where TModel : SearchModel
    {
        [TestMethod]
        public void ParametersBuilder_Filter()
        {
            IParametersBuilder<TModel, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.Filter);

            parametersBuilder.Where(_ => _.SearchScore == 0.0);

            Assert.IsNotNull(parametersBuilder.Filter);
            Assert.AreEqual("search.score() eq 0", parametersBuilder.Filter);

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo filterPropertyInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<TModel, TParameters>.Filter));
            Assert.IsNotNull(filterPropertyInfo);

            string filter = filterPropertyInfo.GetValue(parameters) as string;
            Assert.IsNotNull(filter);
            Assert.AreEqual("search.score() eq 0", filter);
        }

        [TestMethod]
        public void ParametersBuilder_HighlightPostTag()
        {
            IParametersBuilder<TModel, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.HighlightPostTag);

            parametersBuilder.WithHighlightPostTag("test");

            Assert.IsNotNull(parametersBuilder.HighlightPostTag);
            Assert.AreEqual("test", parametersBuilder.HighlightPostTag);

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo highlightPostTagPropertyInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<TModel, TParameters>.HighlightPostTag));
            Assert.IsNotNull(highlightPostTagPropertyInfo);

            string highlightPostTag = highlightPostTagPropertyInfo.GetValue(parameters) as string;
            Assert.IsNotNull(highlightPostTag);
            Assert.AreEqual("test", highlightPostTag);
        }

        [TestMethod]
        public void ParametersBuilder_HighlightPreTag()
        {
            IParametersBuilder<TModel, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.HighlightPreTag);

            parametersBuilder.WithHighlightPreTag("test");

            Assert.IsNotNull(parametersBuilder.HighlightPreTag);
            Assert.AreEqual("test", parametersBuilder.HighlightPreTag);

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo highlightPreTagPropertyInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<TModel, TParameters>.HighlightPreTag));
            Assert.IsNotNull(highlightPreTagPropertyInfo);

            string highlightPreTag = highlightPreTagPropertyInfo.GetValue(parameters) as string;
            Assert.IsNotNull(highlightPreTag);
            Assert.AreEqual("test", highlightPreTag);
        }

        [TestMethod]
        public void ParametersBuilder_MinimumCoverage()
        {
            IParametersBuilder<TModel, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.MinimumCoverage);

            parametersBuilder.WithMinimumCoverage(1.1);

            Assert.IsNotNull(parametersBuilder.MinimumCoverage);
            Assert.AreEqual(1.1, parametersBuilder.MinimumCoverage);

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo minimumCoveragePropertyInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<TModel, TParameters>.MinimumCoverage));
            Assert.IsNotNull(minimumCoveragePropertyInfo);

            double? minimumCoverage = minimumCoveragePropertyInfo.GetValue(parameters) as double?;
            Assert.IsNotNull(minimumCoverage);
            Assert.AreEqual(1.1, minimumCoverage);
        }

        [TestMethod]
        public void ParametersBuilder_SearchFields()
        {
            IParametersBuilder<TModel, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.SearchFields);

            parametersBuilder.WithSearchField(_ => _.SearchScore);

            try
            {
                Assert.IsNotNull(parametersBuilder.SearchFields);
                Assert.AreEqual(1, parametersBuilder.SearchFields.Count());
                Assert.AreEqual("search.score()", parametersBuilder.SearchFields.ElementAtOrDefault(0));
            }
            catch
            {
                if (parametersBuilder.SearchFields != null)
                {
                    foreach (string searchField in parametersBuilder.SearchFields)
                    {
                        Console.WriteLine(searchField);
                    }
                }

                throw;
            }

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo searchFieldsPropertyInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<TModel, TParameters>.SearchFields));
            Assert.IsNotNull(searchFieldsPropertyInfo);

            IEnumerable<string> searchFields = searchFieldsPropertyInfo.GetValue(parameters) as IEnumerable<string>;
            try
            {
                Assert.IsNotNull(searchFields);
                Assert.AreEqual(1, searchFields.Count());
                Assert.AreEqual("search.score()", searchFields.ElementAtOrDefault(0));
            }
            catch
            {
                if (parametersBuilder.SearchFields != null)
                {
                    foreach (string searchField in parametersBuilder.SearchFields)
                    {
                        Console.WriteLine(searchField);
                    }
                }

                throw;
            }
        }

        [TestMethod]
        public void ParametersBuilder_SerializerSettings()
        {
            IParametersBuilder<TModel, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNotNull(parametersBuilder.SerializerSettings);
        }

        [TestMethod]
        public void ParametersBuilder_Top()
        {
            IParametersBuilder<TModel, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.Top);

            parametersBuilder.WithTop(1);

            Assert.IsNotNull(parametersBuilder.Top);
            Assert.AreEqual(1, parametersBuilder.Top);

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo topInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<TModel, TParameters>.Top));
            Assert.IsNotNull(topInfo);

            int? top = topInfo.GetValue(parameters) as int?;
            Assert.IsNotNull(top);
            Assert.AreEqual(1, top);
        }

        protected abstract IParametersBuilder<TModel, TParameters> ConstructBuilder();
    }
}
