using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AzureSearchQueryBuilder.Builders;
using AzureSearchQueryBuilder.Models;
using Microsoft.Azure.Search.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParametersBuilder_GetPropertyName_Null()
        {
            Expression<Func<Level1, string>> lambdaExpression = null;
            new MultilevelBuilder().GetPropertyName(lambdaExpression);
        }

        [TestMethod]
        public void ParametersBuilder_GetPropertyName_Basic()
        {
            Expression<Func<Level1, string>> lambdaExpression = _ => _.Id;
            string result = new MultilevelBuilder().GetPropertyName(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("id", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetPropertyName_JsonProperty()
        {
            Expression<Func<Level1, string>> lambdaExpression = _ => _.JsonProperty;
            string result = new MultilevelBuilder().GetPropertyName(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("json_property", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetPropertyName_Child_Basic()
        {
            Expression<Func<Level1, string>> lambdaExpression = _ => _.Complex.Id;
            string result = new MultilevelBuilder().GetPropertyName(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/id", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetPropertyName_Child_JsonProperty()
        {
            Expression<Func<Level1, string>> lambdaExpression = _ => _.Complex.JsonProperty;
            string result = new MultilevelBuilder().GetPropertyName(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/json_property", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetPropertyName_ChildCollection_Basic()
        {
            Expression<Func<Level1, IEnumerable<string>>> lambdaExpression1 = _ => _.CollectionComplex.Select(s => s.Id);
            string result = new MultilevelBuilder().GetPropertyName(lambdaExpression1);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/id", result);

            Expression<Func<Level1, string>> lambdaExpression2 = _ => _.CollectionComplex.First().Id;
            result = new MultilevelBuilder().GetPropertyName(lambdaExpression2);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/id", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetPropertyName_ChildCollection_JsonProperty()
        {
            Expression<Func<Level1, IEnumerable<string>>> lambdaExpression1 = _ => _.CollectionComplex.Select(s => s.JsonProperty);
            string result = new MultilevelBuilder().GetPropertyName(lambdaExpression1);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/json_property", result);

            Expression<Func<Level1, string>> lambdaExpression2 = _ => _.CollectionComplex.First().JsonProperty;
            result = new MultilevelBuilder().GetPropertyName(lambdaExpression2);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/json_property", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParametersBuilder_GetFilterExpression_Null()
        {
            Expression<Func<Level1, bool>> lambdaExpression = null;
            string result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
        }

        [TestMethod]
        public void ParametersBuilder_GetFilterExpression_Equal()
        {
            bool boolValue = true;
            byte byteValue = 1;
            DateTime dateTimeValue = new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            DateTimeOffset dateTimeOffsetValue = new DateTimeOffset(2019, 08, 02, 13, 09, 08, 07, TimeSpan.FromHours(-5));
            double doubleValue = 1.1;
            Guid guidValue = Guid.Parse("00000000-ABCD-0000-0000-000000000000");
            int intValue = 1;
            long longValue = 1;
            short shortValue = 1;
            string stringValue = "Foo";
            TimeSpan timeSpanValue = new TimeSpan(00, 13, 09, 08, 07);

            Expression<Func<Level1, bool>> lambdaExpression = _ => _.Boolean == true;
            string result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("boolean eq true", result);

            lambdaExpression = _ => _.Boolean == boolValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("boolean eq true", result);

            lambdaExpression = _ => _.Byte == (byte)1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte eq 1", result);

            lambdaExpression = _ => _.Byte == byteValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte eq 1", result);

            lambdaExpression = _ => _.CollectionComplex.Any(c => c.JsonProperty == "Foo");
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/any(c:c/json_property eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.Any(c => c.JsonProperty == stringValue);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/any(c:c/json_property eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.All(c => c.JsonProperty == "Foo");
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/all(c:c/json_property eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.All(c => c.JsonProperty == stringValue);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/all(c:c/json_property eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.Any(c => c == "Foo");
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/any(c:c eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.Any(c => c == stringValue);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/any(c:c eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.All(c => c == "Foo");
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/all(c:c eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.All(c => c == stringValue);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/all(c:c eq 'Foo')", result);

            lambdaExpression = _ => _.Complex.JsonProperty == "Foo";
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/json_property eq 'Foo'", result);

            lambdaExpression = _ => _.Complex.JsonProperty == stringValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/json_property eq 'Foo'", result);

            lambdaExpression = _ => _.DateTime == new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime eq '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTime == dateTimeValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime eq '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTimeOffset == dateTimeOffsetValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset eq '2019-08-02T13:09:08.0070000-05:00'", result);

            lambdaExpression = _ => _.Double == 1.1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double eq 1.1", result);

            lambdaExpression = _ => _.Double == doubleValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double eq 1.1", result);

            lambdaExpression = _ => _.Guid == guidValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("guid eq '00000000-abcd-0000-0000-000000000000'", result);

            lambdaExpression = _ => _.Int16 == (short)1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 eq 1", result);

            lambdaExpression = _ => _.Int16 == shortValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 eq 1", result);

            lambdaExpression = _ => _.Int32 == 1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 eq 1", result);

            lambdaExpression = _ => _.Int32 == intValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 eq 1", result);

            lambdaExpression = _ => _.Int64 == 1L;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 eq 1", result);

            lambdaExpression = _ => _.Int64 == longValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 eq 1", result);

            lambdaExpression = _ => _.JsonProperty == "Foo";
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("json_property eq 'Foo'", result);

            lambdaExpression = _ => _.JsonProperty == stringValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("json_property eq 'Foo'", result);

            lambdaExpression = _ => _.TimeSpan == new TimeSpan(00, 13, 09, 08, 07);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan eq '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan == timeSpanValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan eq '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetFilterExpression_GreaterThan()
        {
            byte byteValue = 1;
            DateTime dateTimeValue = new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            DateTimeOffset dateTimeOffsetValue = new DateTimeOffset(2019, 08, 02, 13, 09, 08, 07, TimeSpan.FromHours(-5));
            double doubleValue = 1.1;
            int intValue = 1;
            long longValue = 1;
            short shortValue = 1;
            TimeSpan timeSpanValue = new TimeSpan(00, 13, 09, 08, 07);

            Expression<Func<Level1, bool>> lambdaExpression = _ => _.Byte > 1;
            string result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte gt 1", result);

            lambdaExpression = _ => _.Byte > byteValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte gt 1", result);

            lambdaExpression = _ => _.DateTime > new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime gt '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTime > dateTimeValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime gt '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTimeOffset > dateTimeOffsetValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset gt '2019-08-02T13:09:08.0070000-05:00'", result);

            lambdaExpression = _ => _.Double > 1.1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double gt 1.1", result);

            lambdaExpression = _ => _.Double > doubleValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double gt 1.1", result);

            lambdaExpression = _ => _.Int16 > (short)1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 gt 1", result);

            lambdaExpression = _ => _.Int16 > shortValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 gt 1", result);

            lambdaExpression = _ => _.Int32 > 1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 gt 1", result);

            lambdaExpression = _ => _.Int32 > intValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 gt 1", result);

            lambdaExpression = _ => _.Int64 > 1L;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 gt 1", result);

            lambdaExpression = _ => _.Int64 > longValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 gt 1", result);

            lambdaExpression = _ => _.TimeSpan > new TimeSpan(00, 13, 09, 08, 07);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan gt '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan > timeSpanValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan gt '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetFilterExpression_GreaterThanOrEqual()
        {
            byte byteValue = 1;
            DateTime dateTimeValue = new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            DateTimeOffset dateTimeOffsetValue = new DateTimeOffset(2019, 08, 02, 13, 09, 08, 07, TimeSpan.FromHours(-5));
            double doubleValue = 1.1;
            int intValue = 1;
            long longValue = 1;
            short shortValue = 1;
            TimeSpan timeSpanValue = new TimeSpan(00, 13, 09, 08, 07);

            Expression<Func<Level1, bool>> lambdaExpression = _ => _.Byte >= 1;
            string result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte ge 1", result);

            lambdaExpression = _ => _.Byte >= byteValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte ge 1", result);

            lambdaExpression = _ => _.DateTime >= new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime ge '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTime >= dateTimeValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime ge '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTimeOffset >= dateTimeOffsetValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset ge '2019-08-02T13:09:08.0070000-05:00'", result);

            lambdaExpression = _ => _.Double >= 1.1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double ge 1.1", result);

            lambdaExpression = _ => _.Double >= doubleValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double ge 1.1", result);

            lambdaExpression = _ => _.Int16 >= (short)1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 ge 1", result);

            lambdaExpression = _ => _.Int16 >= shortValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 ge 1", result);

            lambdaExpression = _ => _.Int32 >= 1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 ge 1", result);

            lambdaExpression = _ => _.Int32 >= intValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 ge 1", result);

            lambdaExpression = _ => _.Int64 >= 1L;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 ge 1", result);

            lambdaExpression = _ => _.Int64 >= longValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 ge 1", result);

            lambdaExpression = _ => _.TimeSpan >= new TimeSpan(00, 13, 09, 08, 07);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan ge '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan >= timeSpanValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan ge '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetFilterExpression_LessThan()
        {
            byte byteValue = 1;
            DateTime dateTimeValue = new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            DateTimeOffset dateTimeOffsetValue = new DateTimeOffset(2019, 08, 02, 13, 09, 08, 07, TimeSpan.FromHours(-5));
            double doubleValue = 1.1;
            int intValue = 1;
            long longValue = 1;
            short shortValue = 1;
            TimeSpan timeSpanValue = new TimeSpan(00, 13, 09, 08, 07);

            Expression<Func<Level1, bool>> lambdaExpression = _ => _.Byte < 1;
            string result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte lt 1", result);

            lambdaExpression = _ => _.Byte < byteValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte lt 1", result);

            lambdaExpression = _ => _.DateTime < new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime lt '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTime < dateTimeValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime lt '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTimeOffset < dateTimeOffsetValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset lt '2019-08-02T13:09:08.0070000-05:00'", result);

            lambdaExpression = _ => _.Double < 1.1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double lt 1.1", result);

            lambdaExpression = _ => _.Double < doubleValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double lt 1.1", result);

            lambdaExpression = _ => _.Int16 < (short)1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 lt 1", result);

            lambdaExpression = _ => _.Int16 < shortValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 lt 1", result);

            lambdaExpression = _ => _.Int32 < 1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 lt 1", result);

            lambdaExpression = _ => _.Int32 < intValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 lt 1", result);

            lambdaExpression = _ => _.Int64 < 1L;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 lt 1", result);

            lambdaExpression = _ => _.Int64 < longValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 lt 1", result);

            lambdaExpression = _ => _.TimeSpan < new TimeSpan(00, 13, 09, 08, 07);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan lt '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan < timeSpanValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan lt '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetFilterExpression_LessThanOrEqual()
        {
            byte byteValue = 1;
            DateTime dateTimeValue = new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            DateTimeOffset dateTimeOffsetValue = new DateTimeOffset(2019, 08, 02, 13, 09, 08, 07, TimeSpan.FromHours(-5));
            double doubleValue = 1.1;
            int intValue = 1;
            long longValue = 1;
            short shortValue = 1;
            TimeSpan timeSpanValue = new TimeSpan(00, 13, 09, 08, 07);

            Expression<Func<Level1, bool>> lambdaExpression = _ => _.Byte <= 1;
            string result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte le 1", result);

            lambdaExpression = _ => _.Byte <= byteValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte le 1", result);

            lambdaExpression = _ => _.DateTime <= new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime le '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTime <= dateTimeValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime le '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTimeOffset <= dateTimeOffsetValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset le '2019-08-02T13:09:08.0070000-05:00'", result);

            lambdaExpression = _ => _.Double <= 1.1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double le 1.1", result);

            lambdaExpression = _ => _.Double <= doubleValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double le 1.1", result);

            lambdaExpression = _ => _.Int16 <= (short)1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 le 1", result);

            lambdaExpression = _ => _.Int16 <= shortValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 le 1", result);

            lambdaExpression = _ => _.Int32 <= 1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 le 1", result);

            lambdaExpression = _ => _.Int32 <= intValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 le 1", result);

            lambdaExpression = _ => _.Int64 <= 1L;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 le 1", result);

            lambdaExpression = _ => _.Int64 <= longValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 le 1", result);

            lambdaExpression = _ => _.TimeSpan <= new TimeSpan(00, 13, 09, 08, 07);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan le '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan <= timeSpanValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan le '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetFilterExpression_NotEqual()
        {
            bool boolValue = true;
            byte byteValue = 1;
            DateTime dateTimeValue = new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            DateTimeOffset dateTimeOffsetValue = new DateTimeOffset(2019, 08, 02, 13, 09, 08, 07, TimeSpan.FromHours(-5));
            double doubleValue = 1.1;
            Guid guidValue = Guid.Parse("00000000-ABCD-0000-0000-000000000000");
            int intValue = 1;
            long longValue = 1;
            short shortValue = 1;
            string stringValue = "Foo";
            TimeSpan timeSpanValue = new TimeSpan(00, 13, 09, 08, 07);

            Expression<Func<Level1, bool>> lambdaExpression = _ => _.Boolean != true;
            string result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("boolean ne true", result);

            lambdaExpression = _ => _.Boolean != boolValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("boolean ne true", result);

            lambdaExpression = _ => _.Byte != (byte)1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte ne 1", result);

            lambdaExpression = _ => _.Byte != byteValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte ne 1", result);

            lambdaExpression = _ => _.CollectionComplex.Any(c => c.JsonProperty != "Foo");
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/any(c:c/json_property ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.Any(c => c.JsonProperty != stringValue);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/any(c:c/json_property ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.All(c => c.JsonProperty != "Foo");
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/all(c:c/json_property ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.All(c => c.JsonProperty != stringValue);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/all(c:c/json_property ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.Any(c => c != "Foo");
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/any(c:c ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.Any(c => c != stringValue);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/any(c:c ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.All(c => c != "Foo");
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/all(c:c ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.All(c => c != stringValue);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/all(c:c ne 'Foo')", result);

            lambdaExpression = _ => _.Complex.JsonProperty != "Foo";
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/json_property ne 'Foo'", result);

            lambdaExpression = _ => _.Complex.JsonProperty != stringValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/json_property ne 'Foo'", result);

            lambdaExpression = _ => _.DateTime != new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime ne '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTime != dateTimeValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime ne '2019-08-02T13:09:08.0070000Z'", result);

            lambdaExpression = _ => _.DateTimeOffset != dateTimeOffsetValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset ne '2019-08-02T13:09:08.0070000-05:00'", result);

            lambdaExpression = _ => _.Double != 1.1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double ne 1.1", result);

            lambdaExpression = _ => _.Double != doubleValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("double ne 1.1", result);

            lambdaExpression = _ => _.Guid != guidValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("guid ne '00000000-abcd-0000-0000-000000000000'", result);

            lambdaExpression = _ => _.Int16 != (short)1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 ne 1", result);

            lambdaExpression = _ => _.Int16 != shortValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 ne 1", result);

            lambdaExpression = _ => _.Int32 != 1;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 ne 1", result);

            lambdaExpression = _ => _.Int32 != intValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 ne 1", result);

            lambdaExpression = _ => _.Int64 != 1L;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 ne 1", result);

            lambdaExpression = _ => _.Int64 != longValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 ne 1", result);

            lambdaExpression = _ => _.JsonProperty != "Foo";
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("json_property ne 'Foo'", result);

            lambdaExpression = _ => _.JsonProperty != stringValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("json_property ne 'Foo'", result);

            lambdaExpression = _ => _.TimeSpan != new TimeSpan(00, 13, 09, 08, 07);
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan ne '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan != timeSpanValue;
            result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan ne '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetFilterExpression_True()
        {
            Expression<Func<Level1, bool?>> lambdaExpression = _ => _.Boolean;
            string result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("boolean", result);
        }

        [TestMethod]
        public void ParametersBuilder_GetFilterExpression_Not()
        {
            Expression<Func<Level1, bool?>> lambdaExpression = _ => !_.Boolean;
            string result = new MultilevelBuilder().GetFilterExpression(lambdaExpression);
            Assert.IsNotNull(result);
            Assert.AreEqual("not boolean", result);
        }

        protected abstract IParametersBuilder<TModel, TParameters> ConstructBuilder();

        private IParametersBuilder<Level1, object> ConstructMultilevelBuilder()
        {
            return new MultilevelBuilder();
        }

        [SerializePropertyNamesAsCamelCase]
        private class Level1 : SearchModel
        {
            public string Id { get; set; }

            public bool? Boolean { get; set; }

            public byte? Byte { get; set; }

            public ICollection<Level2> CollectionComplex { get; set; }

            public ICollection<string> CollectionSimple { get; set; }

            public Level2 Complex { get; set; }

            public DateTime? DateTime { get; set; }

            public DateTimeOffset? DateTimeOffset { get; set; }

            public double? Double { get; set; }

            public Guid? Guid { get; set; }

            public short? Int16 { get; set; }

            public int? Int32 { get; set; }

            public long? Int64 { get; set; }

            [JsonProperty("json_property")]
            public string JsonProperty { get; set; }

            public TimeSpan? TimeSpan { get; set; }
        }

        private class Level2
        {
            public string Id { get; set; }

            [JsonProperty("json_property")]
            public string JsonProperty { get; set; }
        }

        private class MultilevelBuilder : ParametersBuilder<Level1, object>
        {
            public override object Build()
            {
                throw new NotImplementedException();
            }
        }
    }
}
