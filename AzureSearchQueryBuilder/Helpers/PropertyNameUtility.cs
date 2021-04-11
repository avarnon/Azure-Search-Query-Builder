using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AzureSearchQueryBuilder.Builders;
using AzureSearchQueryBuilder.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSearchQueryBuilder.Helpers
{
    /// <summary>
    /// A helper class for building OData property names.
    /// </summary>
    internal static class PropertyNameUtility
    {
        /// <summary>
        /// Get the OData property name.
        /// </summary>
        /// <param name="expression">The expression from which to parse the OData property name.</param>
        /// <param name="useCamlCase">Is the property name expected to be in CAML case?</param>
        /// <returns>the OData property name.</returns>
        public static PropertyOrFieldInfo GetPropertyName(Expression expression, JsonSerializerSettings jsonSerializerSettings, bool useCamlCase)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    {
                        LambdaExpression labmdaExpression = expression as LambdaExpression;
                        if (labmdaExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(LambdaExpression)}\r\n\t{expression}", nameof(expression));

                        return GetPropertyName(labmdaExpression, jsonSerializerSettings, useCamlCase);
                    }

                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = expression as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(MemberExpression)}\r\n\t{expression}", nameof(expression));

                        return GetPropertyName(memberExpression, jsonSerializerSettings, useCamlCase);
                    }

                case ExpressionType.Call:
                    {
                        MethodCallExpression methodCallExpression = expression as MethodCallExpression;
                        if (methodCallExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(MethodCallExpression)}\r\n\t{expression}", nameof(expression));

                        return GetPropertyName(methodCallExpression, jsonSerializerSettings, useCamlCase);
                    }

                case ExpressionType.Constant:
                    {
                        ConstantExpression constantExpression = expression as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(ConstantExpression)}\r\n\t{expression}", nameof(expression));

                        string value = constantExpression.Value?.ToString();

                        return new PropertyOrFieldInfo(
                            value,
                            value,
                            constantExpression.Type,
                            jsonSerializerSettings,
                            false);
                    }

                default:
                    throw new ArgumentException($"Invalid expression type {expression.NodeType}\r\n\t{expression}", nameof(expression));
            }
        }

        /// <summary>
        /// Get the OData property name.
        /// </summary>
        /// <param name="lambdaExpression">The expression from which to parse the OData property name.</param>
        /// <param name="useCamlCase">Is the property name expected to be in CAML case?</param>
        /// <returns>the OData property name.</returns>
        private static PropertyOrFieldInfo GetPropertyName(LambdaExpression lambdaExpression, JsonSerializerSettings jsonSerializerSettings, bool useCamlCase)
        {
            if (lambdaExpression == null || lambdaExpression.Body == null) throw new ArgumentNullException(nameof(lambdaExpression));

            switch (lambdaExpression.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = lambdaExpression.Body as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(lambdaExpression)}.{nameof(LambdaExpression.Body)} to be of type {nameof(MemberExpression)}\r\n\t{lambdaExpression}", nameof(lambdaExpression));

                        return GetPropertyName(memberExpression, jsonSerializerSettings, useCamlCase);
                    }

                case ExpressionType.Call:
                    {
                        MethodCallExpression methodCallExpression = lambdaExpression.Body as MethodCallExpression;
                        if (methodCallExpression == null) throw new ArgumentException($"Expected {nameof(lambdaExpression)}.{nameof(LambdaExpression.Body)} to be of type {nameof(MethodCallExpression)}\r\n\t{lambdaExpression}", nameof(lambdaExpression));

                        return GetPropertyName(methodCallExpression, jsonSerializerSettings, useCamlCase);
                    }

                case ExpressionType.Constant:
                    {
                        ConstantExpression constantExpression = lambdaExpression.Body as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Expected {nameof(lambdaExpression)}.{nameof(LambdaExpression.Body)} to be of type {nameof(ConstantExpression)}\r\n\t{lambdaExpression}", nameof(lambdaExpression));

                        string value = constantExpression.Value?.ToString();

                        return new PropertyOrFieldInfo(
                            value,
                            value,
                            constantExpression.Type,
                            jsonSerializerSettings,
                            false);
                    }

                default:
                    throw new ArgumentException($"Invalid expression type {lambdaExpression.NodeType}\r\n\t{lambdaExpression}", nameof(lambdaExpression));
            }
        }

        /// <summary>
        /// Get the OData property name.
        /// </summary>
        /// <param name="memberExpression">The expression from which to parse the OData property name.</param>
        /// <param name="useCamlCase">Is the property name expected to be in CAML case?</param>
        /// <returns>the OData property name.</returns>
        public static PropertyOrFieldInfo GetPropertyName(MemberExpression memberExpression, JsonSerializerSettings jsonSerializerSettings, bool useCamlCase)
        {
            if (memberExpression == null) throw new ArgumentNullException(nameof(memberExpression));

            PropertyOrFieldInfo parentProperty = null;
            if (memberExpression.Expression != null)
            {
                switch (memberExpression.Expression.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        {
                            MemberExpression childMemberExpression = memberExpression.Expression as MemberExpression;
                            if (childMemberExpression == null) throw new ArgumentException($"Expected {nameof(memberExpression)}.{nameof(MemberExpression.Expression)} to be of type {nameof(MemberExpression)}\r\n\t{memberExpression}", nameof(memberExpression));

                            parentProperty = GetPropertyName(childMemberExpression, jsonSerializerSettings, false);
                        }

                        break;

                    case ExpressionType.Call:
                        {
                            MethodCallExpression methodCallExpression = memberExpression.Expression as MethodCallExpression;
                            if (methodCallExpression == null) throw new ArgumentException($"Expected {nameof(memberExpression)}.{nameof(MemberExpression.Expression)} to be of type {nameof(MethodCallExpression)}\r\n\t{memberExpression}", nameof(memberExpression));

                            parentProperty = GetPropertyName(methodCallExpression, jsonSerializerSettings, false);
                        }

                        break;

                    case ExpressionType.Parameter:
                        break;

                    default:
                        throw new ArgumentException($"Invalid expression type {memberExpression.NodeType}\r\n\t{memberExpression}", nameof(memberExpression));
                }
            }

            PropertyOrFieldInfo leafProperty = null;

            switch (memberExpression.Member.MemberType)
            {
                case MemberTypes.Field:
                    FieldInfo fieldInfo = memberExpression.Member as FieldInfo;
                    leafProperty = GetFieldName(fieldInfo, jsonSerializerSettings, parentProperty?.UseCamlCase ?? false);
                    break;

                case MemberTypes.Property:
                    PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
                    leafProperty = GetPropertyName(propertyInfo, jsonSerializerSettings, useCamlCase || (parentProperty?.UseCamlCase ?? false));
                    break;

                default:
                    throw new ArgumentException($"Invalid member type {memberExpression.Member.MemberType}", nameof(memberExpression));
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
                        Constants.ODataMemberAccessOperator,
                        leafProperty),
                    leafProperty.PropertyOrFieldType,
                    jsonSerializerSettings,
                    leafProperty.UseCamlCase);
            }
        }

        /// <summary>
        /// Get the OData property name.
        /// </summary>
        /// <param name="methodCallExpression">The expression from which to parse the OData property name.</param>
        /// <param name="useCamlCase">Is the property name expected to be in CAML case?</param>
        /// <returns>the OData property name.</returns>
        public static PropertyOrFieldInfo GetPropertyName(MethodCallExpression methodCallExpression, JsonSerializerSettings jsonSerializerSettings, bool useCamlCase)
        {
            if (methodCallExpression == null) throw new ArgumentNullException(nameof(methodCallExpression));

            if (methodCallExpression.Method.DeclaringType == typeof(SearchFns) &&
                methodCallExpression.Method.Name == nameof(SearchFns.Score))
            {
                return new PropertyOrFieldInfo(
                    nameof(SearchFns.Score),
                    "search.score()",
                    typeof(double),
                    jsonSerializerSettings,
                    useCamlCase);
            }
            else
            {
                IList<PropertyOrFieldInfo> tokens = new List<PropertyOrFieldInfo>();
                bool useCamlCaseLocal = useCamlCase;
                int idx = -1;
                foreach (Expression argumentExpression in methodCallExpression.Arguments)
                {
                    idx++;
                    switch (argumentExpression.NodeType)
                    {
                        case ExpressionType.MemberAccess:
                            {
                                MemberExpression argumentMemberExpression = argumentExpression as MemberExpression;
                                if (argumentMemberExpression == null) throw new ArgumentException($"Expected {nameof(methodCallExpression)}.{nameof(MethodCallExpression.Arguments)}[{idx}] to be of type {nameof(MemberExpression)}\r\n\t{methodCallExpression}", nameof(methodCallExpression));

                                PropertyOrFieldInfo newToken = GetPropertyName(argumentMemberExpression, jsonSerializerSettings, useCamlCaseLocal);
                                useCamlCaseLocal = useCamlCaseLocal || newToken.UseCamlCase;
                                tokens.Add(newToken);
                            }

                            break;

                        case ExpressionType.Lambda:
                            {
                                LambdaExpression argumentLambdaExpression = argumentExpression as LambdaExpression;
                                if (argumentLambdaExpression == null) throw new ArgumentException($"Expected {nameof(methodCallExpression)}.{nameof(MethodCallExpression.Arguments)}[{idx}] to be of type {nameof(LambdaExpression)}\r\n\t{methodCallExpression}", nameof(methodCallExpression));

                                PropertyOrFieldInfo newToken = GetPropertyName(argumentLambdaExpression, jsonSerializerSettings, useCamlCaseLocal);
                                useCamlCaseLocal = useCamlCaseLocal || newToken.UseCamlCase;
                                tokens.Add(newToken);
                            }

                            break;

                        default:
                            throw new ArgumentException($"Invalid expression type {argumentExpression.NodeType}\r\n\t{methodCallExpression}", nameof(methodCallExpression));
                    }
                }

                PropertyOrFieldInfo lastPropertyInfo = tokens.LastOrDefault();

                return new PropertyOrFieldInfo(
                    lastPropertyInfo?.PropertyOrFieldName,
                    string.Join(Constants.ODataMemberAccessOperator, tokens),
                    lastPropertyInfo?.PropertyOrFieldType,
                    jsonSerializerSettings,
                    useCamlCaseLocal || (lastPropertyInfo?.UseCamlCase ?? false));
            }
        }

        /// <summary>
        /// Get the OData property name.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> for the property.</param>
        /// <param name="useCamlCase">Is the property name expected to be in CAML case?</param>
        /// <returns>the OData property name.</returns>
        public static PropertyOrFieldInfo GetPropertyName(PropertyInfo propertyInfo, JsonSerializerSettings jsonSerializerSettings, bool useCamlCase)
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
            else if (jsonSerializerSettings != null &&
                jsonSerializerSettings.ContractResolver != null &&
                jsonSerializerSettings.ContractResolver is CamelCasePropertyNamesContractResolver)
            {
                useCamlCaseLocal = true;
            }
            else if (jsonProperty != null &&
                jsonProperty.NamingStrategyType != null &&
                jsonProperty.NamingStrategyType == typeof(CamelCaseNamingStrategy))
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
                jsonSerializerSettings,
                useCamlCaseLocal);
        }

        /// <summary>
        /// Get the OData property name.
        /// </summary>
        /// <param name="fieldInfo">The <see cref="FieldInfo"/> for the property.</param>
        /// <param name="useCamlCase">Is the property name expected to be in CAML case?</param>
        /// <returns>the OData property name.</returns>
        public static PropertyOrFieldInfo GetFieldName(FieldInfo fieldInfo, JsonSerializerSettings jsonSerializerSettings, bool useCamlCase)
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
            else if (jsonSerializerSettings != null &&
               jsonSerializerSettings.ContractResolver != null &&
               jsonSerializerSettings.ContractResolver is CamelCasePropertyNamesContractResolver)
            {
                useCamlCaseLocal = true;
            }
            else if (jsonProperty != null &&
                jsonProperty.NamingStrategyType != null &&
                jsonProperty.NamingStrategyType == typeof(CamelCaseNamingStrategy))
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
                jsonSerializerSettings,
                useCamlCaseLocal);
        }
    }
}
