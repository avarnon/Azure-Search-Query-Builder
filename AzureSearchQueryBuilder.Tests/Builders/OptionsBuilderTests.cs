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
    public abstract class OptionsBuilderTests<TOptions>
    {
        [TestMethod]
        public void PropertyNameUtility_Filter()
        {
            IOptionsBuilder<Model, TOptions> OptionsBuilder = this.ConstructBuilder();

            Assert.IsNull(OptionsBuilder.Filter);

            OptionsBuilder.Where(_ => _.Foo == "test");

            Assert.IsNotNull(OptionsBuilder.Filter);
            Assert.AreEqual("foo eq 'test'", OptionsBuilder.Filter);

            TOptions Options = OptionsBuilder.Build();
            Assert.IsNotNull(Options);

            PropertyInfo filterPropertyInfo = Options.GetType().GetProperty(nameof(IOptionsBuilder<Model, TOptions>.Filter));
            Assert.IsNotNull(filterPropertyInfo);

            string filter = filterPropertyInfo.GetValue(Options) as string;
            Assert.IsNotNull(filter);
            Assert.AreEqual("foo eq 'test'", filter);
        }

        [TestMethod]
        public void PropertyNameUtility_Filter_Implicit_And()
        {
            IOptionsBuilder<Model, TOptions> OptionsBuilder = this.ConstructBuilder();

            Assert.IsNull(OptionsBuilder.Filter);

            OptionsBuilder.Where(_ => _.Foo == "test")
                .Where(_ => _.Foo != "test2");

            Assert.IsNotNull(OptionsBuilder.Filter);
            Assert.AreEqual("(foo eq 'test') and (foo ne 'test2')", OptionsBuilder.Filter);

            TOptions Options = OptionsBuilder.Build();
            Assert.IsNotNull(Options);

            PropertyInfo filterPropertyInfo = Options.GetType().GetProperty(nameof(IOptionsBuilder<Model, TOptions>.Filter));
            Assert.IsNotNull(filterPropertyInfo);

            string filter = filterPropertyInfo.GetValue(Options) as string;
            Assert.IsNotNull(filter);
            Assert.AreEqual("(foo eq 'test') and (foo ne 'test2')", filter);
        }

        [TestMethod]
        public void PropertyNameUtility_HighlightPostTag()
        {
            IOptionsBuilder<Model, TOptions> OptionsBuilder = this.ConstructBuilder();

            Assert.IsNull(OptionsBuilder.HighlightPostTag);

            OptionsBuilder.WithHighlightPostTag("test");

            Assert.IsNotNull(OptionsBuilder.HighlightPostTag);
            Assert.AreEqual("test", OptionsBuilder.HighlightPostTag);

            TOptions Options = OptionsBuilder.Build();
            Assert.IsNotNull(Options);

            PropertyInfo highlightPostTagPropertyInfo = Options.GetType().GetProperty(nameof(IOptionsBuilder<Model, TOptions>.HighlightPostTag));
            Assert.IsNotNull(highlightPostTagPropertyInfo);

            string highlightPostTag = highlightPostTagPropertyInfo.GetValue(Options) as string;
            Assert.IsNotNull(highlightPostTag);
            Assert.AreEqual("test", highlightPostTag);
        }

        [TestMethod]
        public void PropertyNameUtility_HighlightPreTag()
        {
            IOptionsBuilder<Model, TOptions> OptionsBuilder = this.ConstructBuilder();

            Assert.IsNull(OptionsBuilder.HighlightPreTag);

            OptionsBuilder.WithHighlightPreTag("test");

            Assert.IsNotNull(OptionsBuilder.HighlightPreTag);
            Assert.AreEqual("test", OptionsBuilder.HighlightPreTag);

            TOptions Options = OptionsBuilder.Build();
            Assert.IsNotNull(Options);

            PropertyInfo highlightPreTagPropertyInfo = Options.GetType().GetProperty(nameof(IOptionsBuilder<Model, TOptions>.HighlightPreTag));
            Assert.IsNotNull(highlightPreTagPropertyInfo);

            string highlightPreTag = highlightPreTagPropertyInfo.GetValue(Options) as string;
            Assert.IsNotNull(highlightPreTag);
            Assert.AreEqual("test", highlightPreTag);
        }

        [TestMethod]
        public void PropertyNameUtility_MinimumCoverage()
        {
            IOptionsBuilder<Model, TOptions> OptionsBuilder = this.ConstructBuilder();

            Assert.IsNull(OptionsBuilder.MinimumCoverage);

            OptionsBuilder.WithMinimumCoverage(1.1);

            Assert.IsNotNull(OptionsBuilder.MinimumCoverage);
            Assert.AreEqual(1.1, OptionsBuilder.MinimumCoverage);

            TOptions Options = OptionsBuilder.Build();
            Assert.IsNotNull(Options);

            PropertyInfo minimumCoveragePropertyInfo = Options.GetType().GetProperty(nameof(IOptionsBuilder<Model, TOptions>.MinimumCoverage));
            Assert.IsNotNull(minimumCoveragePropertyInfo);

            double? minimumCoverage = minimumCoveragePropertyInfo.GetValue(Options) as double?;
            Assert.IsNotNull(minimumCoverage);
            Assert.AreEqual(1.1, minimumCoverage);
        }

        [TestMethod]
        public void PropertyNameUtility_SearchFields()
        {
            IOptionsBuilder<Model, TOptions> OptionsBuilder = this.ConstructBuilder();

            Assert.IsNull(OptionsBuilder.SearchFields);

            OptionsBuilder.WithSearchField(_ => SearchFns.Score());

            try
            {
                Assert.IsNotNull(OptionsBuilder.SearchFields);
                Assert.AreEqual(1, OptionsBuilder.SearchFields.Count());
                Assert.AreEqual("search.score()", OptionsBuilder.SearchFields.ElementAtOrDefault(0));
            }
            catch
            {
                if (OptionsBuilder.SearchFields != null)
                {
                    foreach (string searchField in OptionsBuilder.SearchFields)
                    {
                        Console.WriteLine(searchField);
                    }
                }

                throw;
            }

            TOptions Options = OptionsBuilder.Build();
            Assert.IsNotNull(Options);

            PropertyInfo searchFieldsPropertyInfo = Options.GetType().GetProperty(nameof(IOptionsBuilder<Model, TOptions>.SearchFields));
            Assert.IsNotNull(searchFieldsPropertyInfo);

            IEnumerable<string> searchFields = searchFieldsPropertyInfo.GetValue(Options) as IEnumerable<string>;
            try
            {
                Assert.IsNotNull(searchFields);
                Assert.AreEqual(1, searchFields.Count());
                Assert.AreEqual("search.score()", searchFields.ElementAtOrDefault(0));
            }
            catch
            {
                if (OptionsBuilder.SearchFields != null)
                {
                    foreach (string searchField in OptionsBuilder.SearchFields)
                    {
                        Console.WriteLine(searchField);
                    }
                }

                throw;
            }
        }

        [TestMethod]
        public void PropertyNameUtility_Size()
        {
            IOptionsBuilder<Model, TOptions> OptionsBuilder = this.ConstructBuilder();

            Assert.IsNull(OptionsBuilder.Size);

            OptionsBuilder.WithSize(1);

            Assert.IsNotNull(OptionsBuilder.Size);
            Assert.AreEqual(1, OptionsBuilder.Size);

            TOptions Options = OptionsBuilder.Build();
            Assert.IsNotNull(Options);

            PropertyInfo topInfo = Options.GetType().GetProperty(nameof(IOptionsBuilder<Model, TOptions>.Size));
            Assert.IsNotNull(topInfo);

            int? top = topInfo.GetValue(Options) as int?;
            Assert.IsNotNull(top);
            Assert.AreEqual(1, top);
        }

        protected abstract IOptionsBuilder<Model, TOptions> ConstructBuilder();
    }
}
