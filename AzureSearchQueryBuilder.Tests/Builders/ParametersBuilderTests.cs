using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AzureSearchQueryBuilder.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureSearchQueryBuilder.Tests.Builders
{
    public class Model
    {
        public string Foo { get; set; }
    }

    [TestClass]
    public abstract class ParametersBuilderTests<TParameters>
    {
        [TestMethod]
        public void PropertyNameUtility_Filter()
        {
            IParametersBuilder<Model, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.Filter);

            parametersBuilder.Where(_ => _.Foo == "test");

            Assert.IsNotNull(parametersBuilder.Filter);
            Assert.AreEqual("Foo eq 'test'", parametersBuilder.Filter);

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo filterPropertyInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<Model, TParameters>.Filter));
            Assert.IsNotNull(filterPropertyInfo);

            string filter = filterPropertyInfo.GetValue(parameters) as string;
            Assert.IsNotNull(filter);
            Assert.AreEqual("Foo eq 'test'", filter);
        }

        [TestMethod]
        public void PropertyNameUtility_Filter_Implicit_And()
        {
            IParametersBuilder<Model, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.Filter);

            parametersBuilder.Where(_ => _.Foo == "test")
                .Where(_ => _.Foo != "test2");

            Assert.IsNotNull(parametersBuilder.Filter);
            Assert.AreEqual("(Foo eq 'test') and (Foo ne 'test2')", parametersBuilder.Filter);

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo filterPropertyInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<Model, TParameters>.Filter));
            Assert.IsNotNull(filterPropertyInfo);

            string filter = filterPropertyInfo.GetValue(parameters) as string;
            Assert.IsNotNull(filter);
            Assert.AreEqual("(Foo eq 'test') and (Foo ne 'test2')", filter);
        }

        [TestMethod]
        public void PropertyNameUtility_HighlightPostTag()
        {
            IParametersBuilder<Model, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.HighlightPostTag);

            parametersBuilder.WithHighlightPostTag("test");

            Assert.IsNotNull(parametersBuilder.HighlightPostTag);
            Assert.AreEqual("test", parametersBuilder.HighlightPostTag);

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo highlightPostTagPropertyInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<Model, TParameters>.HighlightPostTag));
            Assert.IsNotNull(highlightPostTagPropertyInfo);

            string highlightPostTag = highlightPostTagPropertyInfo.GetValue(parameters) as string;
            Assert.IsNotNull(highlightPostTag);
            Assert.AreEqual("test", highlightPostTag);
        }

        [TestMethod]
        public void PropertyNameUtility_HighlightPreTag()
        {
            IParametersBuilder<Model, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.HighlightPreTag);

            parametersBuilder.WithHighlightPreTag("test");

            Assert.IsNotNull(parametersBuilder.HighlightPreTag);
            Assert.AreEqual("test", parametersBuilder.HighlightPreTag);

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo highlightPreTagPropertyInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<Model, TParameters>.HighlightPreTag));
            Assert.IsNotNull(highlightPreTagPropertyInfo);

            string highlightPreTag = highlightPreTagPropertyInfo.GetValue(parameters) as string;
            Assert.IsNotNull(highlightPreTag);
            Assert.AreEqual("test", highlightPreTag);
        }

        [TestMethod]
        public void PropertyNameUtility_MinimumCoverage()
        {
            IParametersBuilder<Model, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.MinimumCoverage);

            parametersBuilder.WithMinimumCoverage(1.1);

            Assert.IsNotNull(parametersBuilder.MinimumCoverage);
            Assert.AreEqual(1.1, parametersBuilder.MinimumCoverage);

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo minimumCoveragePropertyInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<Model, TParameters>.MinimumCoverage));
            Assert.IsNotNull(minimumCoveragePropertyInfo);

            double? minimumCoverage = minimumCoveragePropertyInfo.GetValue(parameters) as double?;
            Assert.IsNotNull(minimumCoverage);
            Assert.AreEqual(1.1, minimumCoverage);
        }

        [TestMethod]
        public void PropertyNameUtility_SearchFields()
        {
            IParametersBuilder<Model, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.SearchFields);

            parametersBuilder.WithSearchField(_ => SearchFns.Score());

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

            PropertyInfo searchFieldsPropertyInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<Model, TParameters>.SearchFields));
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
        public void PropertyNameUtility_Top()
        {
            IParametersBuilder<Model, TParameters> parametersBuilder = this.ConstructBuilder();

            Assert.IsNull(parametersBuilder.Top);

            parametersBuilder.WithTop(1);

            Assert.IsNotNull(parametersBuilder.Top);
            Assert.AreEqual(1, parametersBuilder.Top);

            TParameters parameters = parametersBuilder.Build();
            Assert.IsNotNull(parameters);

            PropertyInfo topInfo = parameters.GetType().GetProperty(nameof(IParametersBuilder<Model, TParameters>.Top));
            Assert.IsNotNull(topInfo);

            int? top = topInfo.GetValue(parameters) as int?;
            Assert.IsNotNull(top);
            Assert.AreEqual(1, top);
        }

        protected abstract IParametersBuilder<Model, TParameters> ConstructBuilder();
    }
}
