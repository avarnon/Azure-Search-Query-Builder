using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Builders;
using AzureSearchQueryBuilder.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSearchQueryBuilder.Tests.Helpers
{
    [TestClass]
    public class FilterExpressionUtilityTests
    {
        private static readonly JsonSerializerSettings __jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FilterExpressionUtility_GetFilterExpression_Null()
        {
            Expression<Func<Level1, bool>> lambdaExpression = null;
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_Equal()
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
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("boolean eq true", result);

            lambdaExpression = _ => _.Boolean == boolValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("boolean eq true", result);

            lambdaExpression = _ => _.Byte == (byte)1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte eq 1", result);

            lambdaExpression = _ => _.Byte == byteValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte eq 1", result);

            lambdaExpression = _ => _.CollectionComplex.Any(c => c.JsonProperty == "Foo");
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/any(c:c/json_property eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.Any(c => c.JsonProperty == stringValue);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/any(c:c/json_property eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.All(c => c.JsonProperty == "Foo");
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/all(c:c/json_property eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.All(c => c.JsonProperty == stringValue);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/all(c:c/json_property eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.Any(c => c == "Foo");
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/any(c:c eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.Any(c => c == stringValue);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/any(c:c eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.All(c => c == "Foo");
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/all(c:c eq 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.All(c => c == stringValue);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/all(c:c eq 'Foo')", result);

            lambdaExpression = _ => _.Complex.JsonProperty == "Foo";
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/json_property eq 'Foo'", result);

            lambdaExpression = _ => _.Complex.JsonProperty == stringValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/json_property eq 'Foo'", result);

            lambdaExpression = _ => _.DateTime == new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime eq 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTime == dateTimeValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime eq 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTimeOffset == dateTimeOffsetValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset eq 2019-08-02T13:09:08.0070000-05:00", result);

            lambdaExpression = _ => _.Double == 1.1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double eq 1.1", result);

            lambdaExpression = _ => _.Double == doubleValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double eq 1.1", result);

            lambdaExpression = _ => _.Guid == guidValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("guid eq '00000000-abcd-0000-0000-000000000000'", result);

            lambdaExpression = _ => _.Int16 == (short)1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 eq 1", result);

            lambdaExpression = _ => _.Int16 == shortValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 eq 1", result);

            lambdaExpression = _ => _.Int32 == 1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 eq 1", result);

            lambdaExpression = _ => _.Int32 == intValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 eq 1", result);

            lambdaExpression = _ => _.Int64 == 1L;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 eq 1", result);

            lambdaExpression = _ => _.Int64 == longValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 eq 1", result);

            lambdaExpression = _ => _.JsonProperty == "Foo";
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("json_property eq 'Foo'", result);

            lambdaExpression = _ => _.JsonProperty == stringValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("json_property eq 'Foo'", result);

            lambdaExpression = _ => _.TimeSpan == new TimeSpan(00, 13, 09, 08, 07);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan eq '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan == timeSpanValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan eq '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_GreaterThan()
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
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte gt 1", result);

            lambdaExpression = _ => _.Byte > byteValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte gt 1", result);

            lambdaExpression = _ => _.DateTime > new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime gt 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTime > dateTimeValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime gt 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTimeOffset > dateTimeOffsetValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset gt 2019-08-02T13:09:08.0070000-05:00", result);

            lambdaExpression = _ => _.Double > 1.1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double gt 1.1", result);

            lambdaExpression = _ => _.Double > doubleValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double gt 1.1", result);

            lambdaExpression = _ => _.Int16 > (short)1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 gt 1", result);

            lambdaExpression = _ => _.Int16 > shortValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 gt 1", result);

            lambdaExpression = _ => _.Int32 > 1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 gt 1", result);

            lambdaExpression = _ => _.Int32 > intValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 gt 1", result);

            lambdaExpression = _ => _.Int64 > 1L;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 gt 1", result);

            lambdaExpression = _ => _.Int64 > longValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 gt 1", result);

            lambdaExpression = _ => _.TimeSpan > new TimeSpan(00, 13, 09, 08, 07);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan gt '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan > timeSpanValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan gt '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_GreaterThanOrEqual()
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
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte ge 1", result);

            lambdaExpression = _ => _.Byte >= byteValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte ge 1", result);

            lambdaExpression = _ => _.DateTime >= new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime ge 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTime >= dateTimeValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime ge 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTimeOffset >= dateTimeOffsetValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset ge 2019-08-02T13:09:08.0070000-05:00", result);

            lambdaExpression = _ => _.Double >= 1.1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double ge 1.1", result);

            lambdaExpression = _ => _.Double >= doubleValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double ge 1.1", result);

            lambdaExpression = _ => _.Int16 >= (short)1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 ge 1", result);

            lambdaExpression = _ => _.Int16 >= shortValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 ge 1", result);

            lambdaExpression = _ => _.Int32 >= 1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 ge 1", result);

            lambdaExpression = _ => _.Int32 >= intValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 ge 1", result);

            lambdaExpression = _ => _.Int64 >= 1L;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 ge 1", result);

            lambdaExpression = _ => _.Int64 >= longValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 ge 1", result);

            lambdaExpression = _ => _.TimeSpan >= new TimeSpan(00, 13, 09, 08, 07);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan ge '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan >= timeSpanValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan ge '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_LessThan()
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
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte lt 1", result);

            lambdaExpression = _ => _.Byte < byteValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte lt 1", result);

            lambdaExpression = _ => _.DateTime < new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime lt 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTime < dateTimeValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime lt 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTimeOffset < dateTimeOffsetValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset lt 2019-08-02T13:09:08.0070000-05:00", result);

            lambdaExpression = _ => _.Double < 1.1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double lt 1.1", result);

            lambdaExpression = _ => _.Double < doubleValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double lt 1.1", result);

            lambdaExpression = _ => _.Int16 < (short)1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 lt 1", result);

            lambdaExpression = _ => _.Int16 < shortValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 lt 1", result);

            lambdaExpression = _ => _.Int32 < 1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 lt 1", result);

            lambdaExpression = _ => _.Int32 < intValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 lt 1", result);

            lambdaExpression = _ => _.Int64 < 1L;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 lt 1", result);

            lambdaExpression = _ => _.Int64 < longValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 lt 1", result);

            lambdaExpression = _ => _.TimeSpan < new TimeSpan(00, 13, 09, 08, 07);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan lt '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan < timeSpanValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan lt '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_LessThanOrEqual()
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
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte le 1", result);

            lambdaExpression = _ => _.Byte <= byteValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte le 1", result);

            lambdaExpression = _ => _.DateTime <= new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime le 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTime <= dateTimeValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime le 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTimeOffset <= dateTimeOffsetValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset le 2019-08-02T13:09:08.0070000-05:00", result);

            lambdaExpression = _ => _.Double <= 1.1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double le 1.1", result);

            lambdaExpression = _ => _.Double <= doubleValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double le 1.1", result);

            lambdaExpression = _ => _.Int16 <= (short)1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 le 1", result);

            lambdaExpression = _ => _.Int16 <= shortValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 le 1", result);

            lambdaExpression = _ => _.Int32 <= 1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 le 1", result);

            lambdaExpression = _ => _.Int32 <= intValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 le 1", result);

            lambdaExpression = _ => _.Int64 <= 1L;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 le 1", result);

            lambdaExpression = _ => _.Int64 <= longValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 le 1", result);

            lambdaExpression = _ => _.TimeSpan <= new TimeSpan(00, 13, 09, 08, 07);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan le '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan <= timeSpanValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan le '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_NotEqual()
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
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("boolean ne true", result);

            lambdaExpression = _ => _.Boolean != boolValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("boolean ne true", result);

            lambdaExpression = _ => _.Byte != (byte)1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte ne 1", result);

            lambdaExpression = _ => _.Byte != byteValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("byte ne 1", result);

            lambdaExpression = _ => _.CollectionComplex.Any(c => c.JsonProperty != "Foo");
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/any(c:c/json_property ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.Any(c => c.JsonProperty != stringValue);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/any(c:c/json_property ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.All(c => c.JsonProperty != "Foo");
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/all(c:c/json_property ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionComplex.All(c => c.JsonProperty != stringValue);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/all(c:c/json_property ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.Any(c => c != "Foo");
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/any(c:c ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.Any(c => c != stringValue);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/any(c:c ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.All(c => c != "Foo");
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/all(c:c ne 'Foo')", result);

            lambdaExpression = _ => _.CollectionSimple.All(c => c != stringValue);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionSimple/all(c:c ne 'Foo')", result);

            lambdaExpression = _ => _.Complex.JsonProperty != "Foo";
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/json_property ne 'Foo'", result);

            lambdaExpression = _ => _.Complex.JsonProperty != stringValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/json_property ne 'Foo'", result);

            lambdaExpression = _ => _.DateTime != new DateTime(2019, 08, 02, 13, 09, 08, 07, DateTimeKind.Utc);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime ne 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTime != dateTimeValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTime ne 2019-08-02T13:09:08.0070000Z", result);

            lambdaExpression = _ => _.DateTimeOffset != dateTimeOffsetValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("dateTimeOffset ne 2019-08-02T13:09:08.0070000-05:00", result);

            lambdaExpression = _ => _.Double != 1.1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double ne 1.1", result);

            lambdaExpression = _ => _.Double != doubleValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("double ne 1.1", result);

            lambdaExpression = _ => _.Guid != guidValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("guid ne '00000000-abcd-0000-0000-000000000000'", result);

            lambdaExpression = _ => _.Int16 != (short)1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 ne 1", result);

            lambdaExpression = _ => _.Int16 != shortValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int16 ne 1", result);

            lambdaExpression = _ => _.Int32 != 1;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 ne 1", result);

            lambdaExpression = _ => _.Int32 != intValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int32 ne 1", result);

            lambdaExpression = _ => _.Int64 != 1L;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 ne 1", result);

            lambdaExpression = _ => _.Int64 != longValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("int64 ne 1", result);

            lambdaExpression = _ => _.JsonProperty != "Foo";
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("json_property ne 'Foo'", result);

            lambdaExpression = _ => _.JsonProperty != stringValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("json_property ne 'Foo'", result);

            lambdaExpression = _ => _.TimeSpan != new TimeSpan(00, 13, 09, 08, 07);
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan ne '13:09:08.0070000'", result);

            lambdaExpression = _ => _.TimeSpan != timeSpanValue;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("timeSpan ne '13:09:08.0070000'", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_True()
        {
            Expression<Func<Level1, bool?>> lambdaExpression = _ => _.Boolean;
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("boolean", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_Not()
        {
            Expression<Func<Level1, bool?>> lambdaExpression = _ => !_.Boolean;
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("not boolean", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_ConstantExpression()
        {
            const string someConstant = "Some Constant";
            Expression<Func<Level1, string>> lambdaExpression = _ => someConstant;
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual(someConstant, result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_And()
        {
            Expression<Func<Level1, bool>> lambdaExpression = _ => _.Boolean == true && _.Boolean != false;
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("(boolean eq true) and (boolean ne false)", result);

            lambdaExpression = _ => _.Boolean == true & _.Boolean != false;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("(boolean eq true) and (boolean ne false)", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_Or()
        {
            Expression<Func<Level1, bool>> lambdaExpression = _ => _.Boolean == true || _.Boolean != false;
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("(boolean eq true) or (boolean ne false)", result);

            lambdaExpression = _ => _.Boolean == true | _.Boolean != false;
            result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("(boolean eq true) or (boolean ne false)", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_RightSide_Object()
        {
            Level1 source = new Level1()
            {
                Complex = new Level2()
                {
                    JsonProperty = "Foo",
                }
            };

            Expression<Func<Level1, bool>> lambdaExpression = _ => _.Complex.JsonProperty == source.Complex.JsonProperty;
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/json_property eq 'Foo'", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_RightSide_MethodCall()
        {
            Guid source = Guid.Parse("00000000-ABCD-0000-0000-000000000000");
            Expression<Func<Level1, bool>> lambdaExpression = _ => _.Id == source.ToString();
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("id eq '00000000-abcd-0000-0000-000000000000'", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_RightSide_NegatedBoolean()
        {
            bool source = false;
            Expression<Func<Level1, bool>> lambdaExpression = _ => _.Boolean == !source;
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("boolean eq true", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_SearchIn()
        {
            Expression<Func<Level1, bool>> lambdaExpression = _ => SearchFns.In(_.Id, "1", "2", "3");
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("search.in('id', '1, 2, 3')", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_SearchIsMatch()
        {
            Expression<Func<Level1, bool>> lambdaExpression = _ => SearchFns.IsMatch("5", _.Id, _.Complex.Id);
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("search.ismatch('5', 'id, complex/id')", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_SearchIsMatchScoring()
        {
            Expression<Func<Level1, bool>> lambdaExpression = _ => SearchFns.IsMatchScoring("5", _.Id, _.Complex.Id);
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("search.ismatchscoring('5', 'id, complex/id')", result);
        }

        [TestMethod]
        public void FilterExpressionUtility_GetFilterExpression_SearchScore()
        {
            Expression<Func<Level1, bool>> lambdaExpression = _ => SearchFns.Score() > 0.5;
            string result = FilterExpressionUtility.GetFilterExpression(lambdaExpression, __jsonSerializerSettings);
            Assert.IsNotNull(result);
            Assert.AreEqual("search.score() gt 0.5", result);
        }

        private class Level1
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

        private class MultilevelBuilder : OptionsBuilder<Level1, object>
        {
            public MultilevelBuilder(JsonSerializerSettings jsonSerializerSettings)
                : base(jsonSerializerSettings)
            {
            }

            public override object Build()
            {
                throw new NotImplementedException();
            }
        }
    }
}
