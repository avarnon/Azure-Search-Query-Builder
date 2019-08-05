using AzureSearchQueryBuilder.Models;
using Microsoft.Azure.Search.Models;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AzureSearchQueryBuilder.Builders
{
    public abstract class ParametersBuilder<TModel, TParameters> : IParametersBuilder<TModel, TParameters>
        where TModel : SearchModel
    {
        private const string ODataMemberAccessOperator = "/";

        private IList<string> _searchFields;

        public string Filter { get; private set; }

        public string HighlightPostTag { get; private set; }

        public string HighlightPreTag { get; private set; }

        public double? MinimumCoverage { get; private set; }

        public IEnumerable<string> SearchFields { get => _searchFields; }

        public JsonSerializerSettings SerializerSettings { get; set;  } = new JsonSerializerSettings()
        {
            ConstructorHandling = ConstructorHandling.Default,
            ContractResolver = new ReadOnlyJsonContractResolver(),
            Converters =
            {
                new Iso8601TimeSpanConverter(),
            },
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK",
            DateParseHandling = DateParseHandling.DateTime,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DefaultValueHandling = DefaultValueHandling.Include,
            FloatFormatHandling = FloatFormatHandling.String,
            FloatParseHandling = FloatParseHandling.Double,
            Formatting = Formatting.Indented,
            MetadataPropertyHandling = MetadataPropertyHandling.Default,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            StringEscapeHandling = StringEscapeHandling.Default,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            TypeNameHandling = TypeNameHandling.None,
        };

        public int? Top { get; private set; }

        public abstract TParameters Build();

        public IParametersBuilder<TModel, TParameters> Where(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            string newFilter = GetFilterExpression(lambdaExpression);
            if (string.IsNullOrWhiteSpace(this.Filter))
            {
                this.Filter = newFilter;
            }
            else
            {
                this.Filter = $"({this.Filter}) and ({newFilter})";
            }

            return this;
        }

        public IParametersBuilder<TModel, TParameters> WithHighlightPostTag(string highlightPostTag)
        {
            this.HighlightPostTag = highlightPostTag;
            return this;
        }

        public IParametersBuilder<TModel, TParameters> WithHighlightPreTag(string highlightPreTag)
        {
            this.HighlightPreTag = highlightPreTag;
            return this;
        }

        public IParametersBuilder<TModel, TParameters> WithMinimumCoverage(double? minimumCoverage)
        {
            this.MinimumCoverage = minimumCoverage;
            return this;
        }

        public IParametersBuilder<TModel, TParameters> WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._searchFields == null)
            {
                this._searchFields = new List<string>();
            }

            string field = GetPropertyName(lambdaExpression);
            this._searchFields.Add(field);

            return this;
        }

        public IParametersBuilder<TModel, TParameters> WithTop(int? top)
        {
            this.Top = top;
            return this;
        }

        protected string GetFilterExpression(LambdaExpression lambdaExpression)
        {
            if (lambdaExpression == null || lambdaExpression.Body == null) throw new ArgumentNullException(nameof(lambdaExpression));

            switch (lambdaExpression.Body.NodeType)
            {
                case ExpressionType.Equal:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.NotEqual:
                    {
                        BinaryExpression binaryExpression = lambdaExpression.Body as BinaryExpression;
                        if (binaryExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));
                        return GetFilterExpression(binaryExpression);
                    }

                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.Not:
                    {
                        UnaryExpression unaryExpression = lambdaExpression.Body as UnaryExpression;
                        if (unaryExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));
                        return GetFilterExpression(unaryExpression);
                    }

                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = lambdaExpression.Body as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));

                        return GetPropertyName(memberExpression, false);
                    }

                case ExpressionType.Call:
                    {
                        MethodCallExpression methodCallExpression = lambdaExpression.Body as MethodCallExpression;
                        if (methodCallExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));

                        switch (methodCallExpression.Method.Name)
                        {
                            case nameof(Queryable.Any):
                            case nameof(Queryable.All):
                                {
                                    IList<string> parts = new List<string>();
                                    foreach (Expression argumentExpression in methodCallExpression.Arguments)
                                    {
                                        switch (argumentExpression.NodeType)
                                        {
                                            case ExpressionType.MemberAccess:
                                                {
                                                    MemberExpression argumentMemberExpression = argumentExpression as MemberExpression;
                                                    if (argumentMemberExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));
                                                    parts.Add(GetPropertyName(argumentMemberExpression, false));
                                                }

                                                break;

                                            case ExpressionType.Lambda:
                                                {
                                                    LambdaExpression argumentLambdaExpression = argumentExpression as LambdaExpression;
                                                    if (argumentLambdaExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));

                                                    ParameterExpression argumentParameterExpression = argumentLambdaExpression.Parameters.SingleOrDefault() as ParameterExpression;
                                                    if (argumentParameterExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));

                                                    string inner = GetFilterExpression(argumentLambdaExpression);
                                                    parts.Add(ODataMemberAccessOperator);
                                                    parts.Add(methodCallExpression.Method.Name.ToLowerInvariant());
                                                    parts.Add($"({argumentParameterExpression.Name}:{argumentParameterExpression.Name}");
                                                    if (string.IsNullOrWhiteSpace(inner) == false &&
                                                        inner.StartsWith(" ") == false)
                                                    {
                                                        parts.Add(ODataMemberAccessOperator);
                                                    }

                                                    parts.Add(inner);
                                                    parts.Add(")");
                                                }

                                                break;

                                            default:
                                                throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));
                                        }
                                    }

                                    return string.Join(string.Empty, parts);
                                }

                            case nameof(Queryable.Select):
                                {
                                    IList<string> parts = new List<string>();
                                    foreach (Expression argumentExpression in methodCallExpression.Arguments)
                                    {
                                        switch (argumentExpression.NodeType)
                                        {
                                            case ExpressionType.MemberAccess:
                                                {
                                                    MemberExpression argumentMemberExpression = argumentExpression as MemberExpression;
                                                    if (argumentMemberExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));
                                                    parts.Add(GetPropertyName(argumentMemberExpression, false));
                                                }

                                                break;

                                            case ExpressionType.Lambda:
                                                {
                                                    LambdaExpression argumentLambdaExpression = argumentExpression as LambdaExpression;
                                                    if (argumentLambdaExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));

                                                    ParameterExpression argumentParameterExpression = argumentLambdaExpression.Parameters.SingleOrDefault() as ParameterExpression;
                                                    if (argumentParameterExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));

                                                    string inner = GetFilterExpression(argumentLambdaExpression);
                                                    parts.Add(ODataMemberAccessOperator);
                                                    parts.Add(inner);
                                                }

                                                break;

                                            default:
                                                throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));
                                        }
                                    }

                                    return string.Join(string.Empty, parts);
                                }

                            default:
                                throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));
                        }
                    }

                default:
                    throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));
            }
        }

        protected string GetPropertyName(LambdaExpression lambdaExpression)
        {
            if (lambdaExpression == null || lambdaExpression.Body == null) throw new ArgumentNullException(nameof(lambdaExpression));

            return GetPropertyName(lambdaExpression, false);
        }

        private string GetFilterExpression(BinaryExpression binaryExpression)
        {
            if (binaryExpression == null || binaryExpression.Left == null || binaryExpression.Right == null) throw new ArgumentNullException(nameof(binaryExpression));

            Func<UnaryExpression, object> getRightValueForConvert = null;
            getRightValueForConvert = (UnaryExpression unaryExpression) =>
            {
                if (unaryExpression == null) throw new ArgumentException($"Invalid expression body type {binaryExpression.Right.GetType()}", nameof(binaryExpression));

                switch (unaryExpression.Operand.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        {
                            MemberExpression memberExpression = unaryExpression.Operand as MemberExpression;
                            if (memberExpression == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));

                            ConstantExpression constantExpression = memberExpression.Expression as ConstantExpression;
                            if (constantExpression == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));

                            string fieldName = memberExpression.Member.Name;
                            object expressionValue = constantExpression.Value;
                            FieldInfo fieldInfo = expressionValue.GetType().GetField(fieldName, BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance);
                            if (fieldInfo == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));

                            return fieldInfo.GetValue(expressionValue);
                        }

                    case ExpressionType.Constant:
                        {
                            ConstantExpression constantExpression = unaryExpression.Operand as ConstantExpression;
                            if (constantExpression == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));

                            return constantExpression.Value;
                        }

                    case ExpressionType.Convert:
                        {
                            UnaryExpression innerUnaryExpression = unaryExpression.Operand as UnaryExpression;
                            if (innerUnaryExpression == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));

                            return getRightValueForConvert(innerUnaryExpression);
                        }

                    case ExpressionType.New:
                        {
                            NewExpression newExpression = unaryExpression.Operand as NewExpression;
                            if (newExpression == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));

                            return Activator.CreateInstance(newExpression.Type, newExpression.Arguments.Select(a => (a as ConstantExpression).Value).ToArray());
                        }

                    default:
                        throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));
                }
            };

            string op = null;
            switch (binaryExpression.NodeType)
            {
                case ExpressionType.Equal:
                    op = "eq";
                    break;

                case ExpressionType.GreaterThan:
                    op = "gt";
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    op = "ge";
                    break;

                case ExpressionType.LessThan:
                    op = "lt";
                    break;

                case ExpressionType.LessThanOrEqual:
                    op = "le";
                    break;

                case ExpressionType.NotEqual:
                    op = "ne";
                    break;

                default:
                    throw new ArgumentException($"Invalid expression body type {binaryExpression.GetType()}", nameof(binaryExpression));
            }

            string left = null;
            switch (binaryExpression.Left.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = binaryExpression.Left as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Invalid expression body type {binaryExpression.Left.GetType()}", nameof(binaryExpression));

                        left = GetPropertyName(memberExpression, false);
                    }

                    break;

                case ExpressionType.Parameter:
                    left = string.Empty;
                    break;

                case ExpressionType.Convert:
                    {
                        UnaryExpression unaryExpression = binaryExpression.Left as UnaryExpression;
                        if (unaryExpression == null) throw new ArgumentException($"Invalid expression body type {binaryExpression.Left.GetType()}", nameof(binaryExpression));

                        switch (unaryExpression.Operand.NodeType)
                        {
                            case ExpressionType.MemberAccess:
                                {
                                    MemberExpression memberExpression = unaryExpression.Operand as MemberExpression;
                                    if (memberExpression == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));

                                    left = GetPropertyName(memberExpression, false);
                                }

                                break;

                            default:
                                throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));
                        }
                    }

                    break;

                default:
                    throw new ArgumentException($"Invalid expression body type {binaryExpression.Left.GetType()}", nameof(binaryExpression));
            }

            object rightValue = null;
            switch (binaryExpression.Right.NodeType)
            {
                case ExpressionType.Constant:
                    {
                        ConstantExpression constantExpression = binaryExpression.Right as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Invalid expression body type {binaryExpression.Right.GetType()}", nameof(binaryExpression));

                        rightValue = constantExpression.Value;
                    }

                    break;

                case ExpressionType.Call:
                    {
                        MethodCallExpression methodCallExpression = binaryExpression.Right as MethodCallExpression;
                        if (methodCallExpression == null) throw new ArgumentException($"Invalid expression body type {binaryExpression.Right.GetType()}", nameof(binaryExpression));

                        rightValue = methodCallExpression.Method.Invoke(null, new object[0]);
                    }

                    break;

                case ExpressionType.Convert:
                    {
                        UnaryExpression unaryExpression = binaryExpression.Right as UnaryExpression;
                        if (unaryExpression == null) throw new ArgumentException($"Invalid expression body type {binaryExpression.Right.GetType()}", nameof(binaryExpression));

                        rightValue = getRightValueForConvert(unaryExpression);
                    }

                    break;

                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = binaryExpression.Right as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Invalid expression body type {binaryExpression.Right.GetType()}", nameof(binaryExpression));

                        ConstantExpression constantExpression = memberExpression.Expression as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Invalid expression body type {binaryExpression.Right.GetType()}", nameof(binaryExpression));

                        string fieldName = memberExpression.Member.Name;
                        object expressionValue = constantExpression.Value;
                        FieldInfo fieldInfo = expressionValue.GetType().GetField(fieldName, BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance);
                        if (fieldInfo == null) throw new ArgumentException($"Invalid expression body type {binaryExpression.Right.GetType()}", nameof(binaryExpression));

                        rightValue = fieldInfo.GetValue(expressionValue);
                    }

                    break;

                default:
                    throw new ArgumentException($"Invalid expression body type {binaryExpression.Right.GetType()}", nameof(binaryExpression));
            }

            if (rightValue == null) throw new ArgumentException($"Invalid expression body type {binaryExpression.Right.GetType()}", nameof(binaryExpression));

            string right = null;
            if (rightValue.GetType() == typeof(string))
            {
                right = string.Concat("'", rightValue as string, "'");
            }
            else if (rightValue.GetType() == typeof(Guid) ||
                     rightValue.GetType() == typeof(Guid?) ||
                     rightValue.GetType() == typeof(TimeSpan) ||
                     rightValue.GetType() == typeof(TimeSpan?))
            {
                right = string.Concat("'", rightValue.ToString().ToLowerInvariant(), "'");
            }
            else if (rightValue.GetType() == typeof(DateTime) ||
                     rightValue.GetType() == typeof(DateTime?))
            {
                right = string.Concat("'", ((DateTime)rightValue).ToString("O"), "'");
            }
            else if (rightValue.GetType() == typeof(DateTimeOffset) ||
                     rightValue.GetType() == typeof(DateTimeOffset?))
            {
                right = string.Concat("'", ((DateTimeOffset)rightValue).ToString("O"), "'");
            }
            else if (rightValue.GetType() == typeof(bool) ||
                     rightValue.GetType() == typeof(bool?))
            {
                right = rightValue.ToString().ToLowerInvariant();
            }
            else
            {
                right = rightValue.ToString();
            }

            return $"{left} {op} {right}";
        }

        private string GetFilterExpression(UnaryExpression unaryExpression)
        {
            if (unaryExpression == null) throw new ArgumentNullException(nameof(unaryExpression));

            string operand = null;
            switch (unaryExpression.Operand.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = unaryExpression.Operand as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(unaryExpression));

                        operand = GetPropertyName(memberExpression, false);
                    }

                    break;

                default:
                    throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(unaryExpression));
            }

            switch (unaryExpression.NodeType)
            {
                case ExpressionType.IsFalse:
                case ExpressionType.Not:
                    return $"not {operand}";

                case ExpressionType.IsTrue:
                    return $"{operand}";

                default:
                    throw new ArgumentException($"Invalid expression body type {unaryExpression.GetType()}", nameof(unaryExpression));
            }
        }

        private PropertyOrFieldInfo GetPropertyName(LambdaExpression lambdaExpression, bool useCamlCase)
        {
            if (lambdaExpression == null || lambdaExpression.Body == null) throw new ArgumentNullException(nameof(lambdaExpression));

            switch (lambdaExpression.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = lambdaExpression.Body as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body.GetType()}", nameof(lambdaExpression));

                        return GetPropertyName(memberExpression, useCamlCase);
                    }

                case ExpressionType.Call:
                    {
                        MethodCallExpression methodCallExpression = lambdaExpression.Body as MethodCallExpression;
                        if (methodCallExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body.GetType()}", nameof(lambdaExpression));

                        return GetPropertyName(methodCallExpression, useCamlCase);
                    }

                case ExpressionType.Constant:
                    {
                        ConstantExpression constantExpression = lambdaExpression.Body as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));

                        string value = constantExpression.Value?.ToString();

                        return new PropertyOrFieldInfo(
                            value,
                            value,
                            constantExpression.Type,
                            false);
                    }

                default:
                    throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));
            }
        }

        private PropertyOrFieldInfo GetPropertyName(MemberExpression memberExpression, bool useCamlCase)
        {
            if (memberExpression == null) throw new ArgumentNullException(nameof(memberExpression));

            PropertyOrFieldInfo parentProperty = null;
            if (memberExpression.Expression == null)
            {
                switch (memberExpression.Expression.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        {
                            MemberExpression childMemberExpression = memberExpression.Expression as MemberExpression;
                            if (childMemberExpression == null) throw new ArgumentException($"Invalid expression body type {memberExpression.Expression.GetType()}", nameof(memberExpression));

                            parentProperty = GetPropertyName(childMemberExpression, false);
                        }

                        break;

                    case ExpressionType.Call:
                        {
                            MethodCallExpression methodCallExpression = memberExpression.Expression as MethodCallExpression;
                            if (methodCallExpression == null) throw new ArgumentException($"Invalid expression body type {memberExpression.Expression.GetType()}", nameof(memberExpression));

                            parentProperty = GetPropertyName(methodCallExpression, false);
                        }

                        break;

                    case ExpressionType.Parameter:
                        break;

                    default:
                        throw new ArgumentException($"Invalid expression body type {memberExpression.Expression.GetType()}", nameof(memberExpression));
                }
            }

            PropertyOrFieldInfo leafProperty = null;
            PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo != null)
            {
                leafProperty = GetPropertyName(propertyInfo, parentProperty?.UseCamlCase ?? false);
            }
            else
            {
                FieldInfo fieldInfo = memberExpression.Member as FieldInfo;
                if (fieldInfo == null) throw new ArgumentException($"Invalid expression body type {memberExpression.Member.GetType()}", nameof(memberExpression));

                leafProperty = GetPropertyName(fieldInfo, parentProperty?.UseCamlCase ?? false);
            }

            if (parentProperty == null)
            {
                return leafProperty;
            }
            else
            {
                return new PropertyOrFieldInfo(
                    leafProperty.PropertyOrFieldName,
                    string.Concat(
                        parentProperty,
                        ODataMemberAccessOperator,
                        leafProperty),
                    leafProperty.PropertyOrFieldType,
                    leafProperty.UseCamlCase);
            }
        }

        private PropertyOrFieldInfo GetPropertyName(MethodCallExpression methodCallExpression, bool useCamlCase)
        {
            if (methodCallExpression == null) throw new ArgumentNullException(nameof(methodCallExpression));

            IList<PropertyOrFieldInfo> tokens = new List<PropertyOrFieldInfo>();
            bool useCamlCaseLocal = useCamlCase;
            foreach (Expression argumentExpression in methodCallExpression.Arguments)
            {
                switch (argumentExpression.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        {
                            MemberExpression argumentMemberExpression = argumentExpression as MemberExpression;
                            if (argumentMemberExpression == null) throw new ArgumentException($"Invalid expression body type {argumentExpression.GetType()}", nameof(methodCallExpression));

                            PropertyOrFieldInfo newToken = GetPropertyName(argumentMemberExpression, useCamlCaseLocal);
                            useCamlCaseLocal = useCamlCaseLocal || newToken.UseCamlCase;
                            tokens.Add(newToken);
                        }

                        break;

                    case ExpressionType.Lambda:
                        {
                            LambdaExpression argumentLambdaExpression = argumentExpression as LambdaExpression;
                            if (argumentLambdaExpression == null) throw new ArgumentException($"Invalid expression body type {argumentExpression.GetType()}", nameof(methodCallExpression));

                            PropertyOrFieldInfo newToken = GetPropertyName(argumentLambdaExpression, useCamlCaseLocal);
                            useCamlCaseLocal = useCamlCaseLocal || newToken.UseCamlCase;
                            tokens.Add(newToken);
                        }

                        break;

                    default:
                        throw new ArgumentException($"Invalid expression body type {argumentExpression.GetType()}", nameof(methodCallExpression));
                }
            }

            PropertyOrFieldInfo lastPropertyInfo = tokens.LastOrDefault();

            return new PropertyOrFieldInfo(
                lastPropertyInfo?.PropertyOrFieldName,
                string.Join(ODataMemberAccessOperator, tokens),
                lastPropertyInfo?.PropertyOrFieldType,
                useCamlCaseLocal || (lastPropertyInfo?.UseCamlCase ?? false));
        }

        private PropertyOrFieldInfo GetPropertyName(PropertyInfo propertyInfo, bool useCamlCase)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            JsonPropertyAttribute jsonProperty = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>(true);

            bool useCamlCaseLocal = false;

            if (jsonProperty != null && string.IsNullOrWhiteSpace(jsonProperty.PropertyName) == false)
            {
                useCamlCaseLocal = false;
            }
            else if (useCamlCase)
            {
                useCamlCaseLocal = true;
            }
            else if (this.SerializerSettings != null &&
                this.SerializerSettings.ContractResolver != null &&
                this.SerializerSettings.ContractResolver is CamelCasePropertyNamesContractResolver)
            {
                useCamlCaseLocal = true;
            }
            else if (jsonProperty != null &&
                jsonProperty.NamingStrategyType != null &&
                jsonProperty.NamingStrategyType == typeof(CamelCaseNamingStrategy))
            {
                useCamlCaseLocal = true;
            }
            else if (propertyInfo.DeclaringType.GetCustomAttributes<SerializePropertyNamesAsCamelCaseAttribute>().Any())
            {
                useCamlCaseLocal = true;
            }
            else
            {
                useCamlCaseLocal = false;
            }

            return new PropertyOrFieldInfo(
                propertyInfo.Name,
                jsonProperty?.PropertyName,
                propertyInfo.PropertyType,
                useCamlCaseLocal);
        }

        private PropertyOrFieldInfo GetPropertyName(FieldInfo fieldInfo, bool useCamlCase)
        {
            if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));

            JsonPropertyAttribute jsonProperty = fieldInfo.GetCustomAttribute<JsonPropertyAttribute>(true);

            bool useCamlCaseLocal = false;

            if (jsonProperty != null && string.IsNullOrWhiteSpace(jsonProperty.PropertyName) == false)
            {
                useCamlCaseLocal = false;
            }
            else if (useCamlCase)
            {
                useCamlCaseLocal = true;
            }
            else if (this.SerializerSettings != null &&
                this.SerializerSettings.ContractResolver != null &&
                this.SerializerSettings.ContractResolver is CamelCasePropertyNamesContractResolver)
            {
                useCamlCaseLocal = true;
            }
            else if (jsonProperty != null &&
                jsonProperty.NamingStrategyType != null &&
                jsonProperty.NamingStrategyType == typeof(CamelCaseNamingStrategy))
            {
                useCamlCaseLocal = true;
            }
            else if (fieldInfo.DeclaringType.GetCustomAttributes<SerializePropertyNamesAsCamelCaseAttribute>().Any())
            {
                useCamlCaseLocal = true;
            }
            else
            {
                useCamlCaseLocal = false;
            }

            return new PropertyOrFieldInfo(
                fieldInfo.Name,
                jsonProperty?.PropertyName,
                fieldInfo.FieldType,
                useCamlCaseLocal);
        }
    }
}
