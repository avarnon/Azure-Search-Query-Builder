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
    public class PropertyNameUtilityTests
    {
        private const string __someConstant = "Some Constant Value";
        private static readonly JsonSerializerSettings __jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyNameUtility_GetPropertyName_Null()
        {
            Expression<Func<Level1, string>> lambdaExpression = null;
            PropertyNameUtility.GetPropertyName(lambdaExpression, __jsonSerializerSettings, false);
        }

        [TestMethod]
        public void PropertyNameUtility_GetPropertyName_Basic()
        {
            Expression<Func<Level1, string>> lambdaExpression = _ => _.Id;
            string result = PropertyNameUtility.GetPropertyName(lambdaExpression, __jsonSerializerSettings, false);
            Assert.IsNotNull(result);
            Assert.AreEqual("id", result);
        }

        [TestMethod]
        public void PropertyNameUtility_GetPropertyName_JsonProperty()
        {
            Expression<Func<Level1, string>> lambdaExpression = _ => _.JsonProperty;
            string result = PropertyNameUtility.GetPropertyName(lambdaExpression, __jsonSerializerSettings, false);
            Assert.IsNotNull(result);
            Assert.AreEqual("json_property", result);
        }

        [TestMethod]
        public void PropertyNameUtility_GetPropertyName_Child_Basic()
        {
            Expression<Func<Level1, string>> lambdaExpression = _ => _.Complex.Id;
            string result = PropertyNameUtility.GetPropertyName(lambdaExpression, __jsonSerializerSettings, false);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/id", result);
        }

        [TestMethod]
        public void PropertyNameUtility_GetPropertyName_Child_JsonProperty()
        {
            Expression<Func<Level1, string>> lambdaExpression = _ => _.Complex.JsonProperty;
            string result = PropertyNameUtility.GetPropertyName(lambdaExpression, __jsonSerializerSettings, false);
            Assert.IsNotNull(result);
            Assert.AreEqual("complex/json_property", result);
        }

        [TestMethod]
        public void PropertyNameUtility_GetPropertyName_ChildCollection_Basic()
        {
            Expression<Func<Level1, IEnumerable<string>>> lambdaExpression1 = _ => _.CollectionComplex.Select(s => s.Id);
            string result = PropertyNameUtility.GetPropertyName(lambdaExpression1, __jsonSerializerSettings, false);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/id", result);

            Expression<Func<Level1, string>> lambdaExpression2 = _ => _.CollectionComplex.First().Id;
            result = PropertyNameUtility.GetPropertyName(lambdaExpression2, __jsonSerializerSettings, false);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/id", result);
        }

        [TestMethod]
        public void PropertyNameUtility_GetPropertyName_ChildCollection_JsonProperty()
        {
            Expression<Func<Level1, IEnumerable<string>>> lambdaExpression1 = _ => _.CollectionComplex.Select(s => s.JsonProperty);
            string result = PropertyNameUtility.GetPropertyName(lambdaExpression1, __jsonSerializerSettings, false);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/json_property", result);

            Expression<Func<Level1, string>> lambdaExpression2 = _ => _.CollectionComplex.First().JsonProperty;
            result = PropertyNameUtility.GetPropertyName(lambdaExpression2, __jsonSerializerSettings, false);
            Assert.IsNotNull(result);
            Assert.AreEqual("collectionComplex/json_property", result);
        }

        [TestMethod]
        public void PropertyNameUtility_GetPropertyName_Constant()
        {
            Expression<Func<Level1, string>> lambdaExpression1 = _ => __someConstant;
            string result = PropertyNameUtility.GetPropertyName(lambdaExpression1, __jsonSerializerSettings, false);
            Assert.IsNotNull(result);
            Assert.AreEqual(__someConstant, result);
        }

        [TestMethod]
        public void PropertyNameUtility_GetPropertyName_SearchScore()
        {
            Expression<Func<Level1, double>> lambdaExpression1 = _ => SearchFns.Score();
            string result = PropertyNameUtility.GetPropertyName(lambdaExpression1, __jsonSerializerSettings, false);
            Assert.IsNotNull(result);
            Assert.AreEqual("search.score()", result);
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
