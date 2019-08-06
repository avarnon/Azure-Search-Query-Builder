﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AzureSearchQueryBuilder.Models;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSearchQueryBuilder.Helpers
{
    internal static class PropertyNameUtility
    {
        public static PropertyOrFieldInfo GetPropertyName(LambdaExpression lambdaExpression, bool useCamlCase)
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

        public static PropertyOrFieldInfo GetPropertyName(MemberExpression memberExpression, bool useCamlCase)
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
                leafProperty = GetPropertyName(propertyInfo, useCamlCase || (parentProperty?.UseCamlCase ?? false));
            }
            else
            {
                FieldInfo fieldInfo = memberExpression.Member as FieldInfo;
                if (fieldInfo == null) throw new ArgumentException($"Invalid expression body type {memberExpression.Member.GetType()}", nameof(memberExpression));

                leafProperty = GetFieldName(fieldInfo, parentProperty?.UseCamlCase ?? false);
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
                    leafProperty.UseCamlCase);
            }
        }

        public static PropertyOrFieldInfo GetPropertyName(MethodCallExpression methodCallExpression, bool useCamlCase)
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
                string.Join(Constants.ODataMemberAccessOperator, tokens),
                lastPropertyInfo?.PropertyOrFieldType,
                useCamlCaseLocal || (lastPropertyInfo?.UseCamlCase ?? false));
        }

        public static PropertyOrFieldInfo GetPropertyName(PropertyInfo propertyInfo, bool useCamlCase)
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

        public static PropertyOrFieldInfo GetFieldName(FieldInfo fieldInfo, bool useCamlCase)
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