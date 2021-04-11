using Newtonsoft.Json;
using System;

namespace AzureSearchQueryBuilder.Models
{
    /// <summary>
    /// A class representing a property or field.
    /// </summary>
    internal class PropertyOrFieldInfo
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propertyOrFieldName">The name of the property or field from reflection.</param>
        /// <param name="jsonPropertyName">The name of the property from the <see cref="Newtonsoft.Json.JsonPropertyAttribute"/> if present.</param>
        /// <param name="propertyOrFieldType">The type of the property or field.</param>
        /// <param name="useCamlCase">Should the serialized property name be in CAML case?</param>
        public PropertyOrFieldInfo(string propertyOrFieldName, string jsonPropertyName, Type propertyOrFieldType, JsonSerializerSettings jsonSerializerSettings, bool useCamlCase)
        {
            if (string.IsNullOrWhiteSpace(propertyOrFieldName)) throw new ArgumentNullException(nameof(propertyOrFieldName));
            if (propertyOrFieldType == null) throw new ArgumentNullException(nameof(propertyOrFieldType));

            this.JsonPropertyName = jsonPropertyName;
            this.PropertyOrFieldName = propertyOrFieldName;
            this.PropertyOrFieldType = propertyOrFieldType;
            JsonSerializerSettings = jsonSerializerSettings;
            this.UseCamlCase = useCamlCase;
        }

        /// <summary>
        /// Gets a value indicating the name of the property or field from reflection.
        /// </summary>
        public string JsonPropertyName { get; }

        /// <summary>
        /// Gets the JSON Serializer settings.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; }

        /// <summary>
        /// Gets a value indicating the name of the property from the <see cref="Newtonsoft.Json.JsonPropertyAttribute"/> if present.
        /// </summary>
        public string PropertyOrFieldName { get; }

        /// <summary>
        /// Gets a value indicating the type of the property or field.
        /// </summary>
        public Type PropertyOrFieldType { get; }

        /// <summary>
        /// Gets a value indicating whether the serialized property name be in CAML case.
        /// </summary>
        public bool UseCamlCase { get; }

        /// <summary>
        /// Convert a <see cref="PropertyOrFieldInfo"/> to a <seealso cref="System.String"/>.
        /// </summary>
        /// <param name="propertyOrFieldInfo"></param>
        public static implicit operator string(PropertyOrFieldInfo propertyOrFieldInfo) => propertyOrFieldInfo.ToString();

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>a string that represents the current object.</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.JsonPropertyName) == false) return this.JsonPropertyName;
            if (this.UseCamlCase == false) return this.PropertyOrFieldName;

            return this.PropertyOrFieldName.Substring(0, 1).ToLowerInvariant() + this.PropertyOrFieldName.Substring(1);
        }
    }
}
