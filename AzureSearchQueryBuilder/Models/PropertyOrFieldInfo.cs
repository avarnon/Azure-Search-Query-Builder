using System;

namespace AzureSearchQueryBuilder.Models
{
    internal class PropertyOrFieldInfo
    {
        public PropertyOrFieldInfo(string propertyOrFieldName, string jsonPropertyName, Type propertyOrFieldType, bool useCamlCase)
        {
            if (string.IsNullOrWhiteSpace(propertyOrFieldName)) throw new ArgumentNullException(nameof(propertyOrFieldName));
            if (propertyOrFieldType == null) throw new ArgumentNullException(nameof(propertyOrFieldType));

            this.JsonPropertyName = jsonPropertyName;
            this.PropertyOrFieldName = propertyOrFieldName;
            this.PropertyOrFieldType = propertyOrFieldType;
            this.UseCamlCase = useCamlCase;
        }

        public string JsonPropertyName { get; }

        public string PropertyOrFieldName { get; }

        public Type PropertyOrFieldType { get; }

        public bool UseCamlCase { get; }

        public static implicit operator string(PropertyOrFieldInfo propertyOrFieldInfo) => propertyOrFieldInfo.ToString();

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.JsonPropertyName) == false) return this.JsonPropertyName;
            if (this.UseCamlCase == false) return this.PropertyOrFieldName;

            return this.PropertyOrFieldName.Substring(0, 1).ToLowerInvariant() + this.PropertyOrFieldName.Substring(1);
        }
    }
}
