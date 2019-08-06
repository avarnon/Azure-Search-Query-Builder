﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AzureSearchQueryBuilder.Helpers
{
    /// <summary>
    /// A helper class for building OData filter expressions.
    /// </summary>
    internal static class FilterExpressionUtility
    {
        /// <summary>
        /// Get the OData filter expression.
        /// </summary>
        /// <param name="lambdaExpression">The expression from which to parse the OData filter.</param>
        /// <returns>the OData filter.</returns>
        public static string GetFilterExpression(LambdaExpression lambdaExpression)
        {
            if (lambdaExpression == null || lambdaExpression.Body == null) throw new ArgumentNullException(nameof(lambdaExpression));

            return GetFilterExpression(lambdaExpression.Body);
        }

        /// <summary>
        /// Get the OData filter expression.
        /// </summary>
        /// <param name="expression">The expression from which to parse the OData filter.</param>
        /// <returns>the OData filter.</returns>
        public static string GetFilterExpression(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.NotEqual:
                    {
                        BinaryExpression binaryExpression = expression as BinaryExpression;
                        if (binaryExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(BinaryExpression)}\r\n\t{expression}", nameof(expression));

                        return GetFilterExpression(binaryExpression);
                    }

                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.Not:
                    {
                        UnaryExpression unaryExpression = expression as UnaryExpression;
                        if (unaryExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(UnaryExpression)}\r\n\t{expression}", nameof(expression));

                        return GetFilterExpression(unaryExpression);
                    }

                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = expression as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(MemberExpression)}\r\n\t{expression}", nameof(expression));

                        return PropertyNameUtility.GetPropertyName(memberExpression, false);
                    }

                case ExpressionType.Call:
                    {
                        MethodCallExpression methodCallExpression = expression as MethodCallExpression;
                        if (methodCallExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(MethodCallExpression)}\r\n\t{expression}", nameof(expression));

                        switch (methodCallExpression.Method.Name)
                        {
                            case nameof(Queryable.Any):
                            case nameof(Queryable.All):
                                {
                                    IList<string> parts = new List<string>();
                                    int idx = 0;
                                    foreach (Expression argumentExpression in methodCallExpression.Arguments)
                                    {
                                        idx++;
                                        switch (argumentExpression.NodeType)
                                        {
                                            case ExpressionType.MemberAccess:
                                                {
                                                    MemberExpression argumentMemberExpression = argumentExpression as MemberExpression;
                                                    if (argumentMemberExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(methodCallExpression.Arguments)}[{idx}] to be of type {nameof(MemberExpression)}\r\n\t{methodCallExpression}", nameof(expression));

                                                    parts.Add(PropertyNameUtility.GetPropertyName(argumentMemberExpression, false));
                                                }

                                                break;

                                            case ExpressionType.Lambda:
                                                {
                                                    LambdaExpression argumentLambdaExpression = argumentExpression as LambdaExpression;
                                                    if (argumentLambdaExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(methodCallExpression.Arguments)}[{idx}] to be of type {nameof(LambdaExpression)}\r\n\t{methodCallExpression}", nameof(expression));

                                                    ParameterExpression argumentParameterExpression = argumentLambdaExpression.Parameters.SingleOrDefault() as ParameterExpression;
                                                    if (argumentParameterExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(methodCallExpression.Arguments)}[{idx}].{nameof(LambdaExpression.Parameters)}[0] to be of type {nameof(ParameterExpression)}\r\n\t{argumentLambdaExpression}", nameof(expression));

                                                    string inner = GetFilterExpression(argumentLambdaExpression);
                                                    parts.Add(Constants.ODataMemberAccessOperator);
                                                    parts.Add(methodCallExpression.Method.Name.ToLowerInvariant());
                                                    parts.Add($"({argumentParameterExpression.Name}:{argumentParameterExpression.Name}");
                                                    if (string.IsNullOrWhiteSpace(inner) == false &&
                                                        inner.StartsWith(" ") == false)
                                                    {
                                                        parts.Add(Constants.ODataMemberAccessOperator);
                                                    }

                                                    parts.Add(inner);
                                                    parts.Add(")");
                                                }

                                                break;

                                            default:
                                                throw new ArgumentException($"Invalid expression type {argumentExpression.NodeType}\r\n\t{methodCallExpression}", nameof(expression));
                                        }
                                    }

                                    return string.Join(string.Empty, parts);
                                }

                            case nameof(Queryable.Select):
                                {
                                    IList<string> parts = new List<string>();
                                    int idx = -1;
                                    foreach (Expression argumentExpression in methodCallExpression.Arguments)
                                    {
                                        idx++;
                                        switch (argumentExpression.NodeType)
                                        {
                                            case ExpressionType.MemberAccess:
                                                {
                                                    MemberExpression argumentMemberExpression = argumentExpression as MemberExpression;
                                                    if (argumentMemberExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(methodCallExpression.Arguments)}[{idx}] to be of type {nameof(MemberExpression)}\r\n\t{expression}", nameof(expression));

                                                    parts.Add(PropertyNameUtility.GetPropertyName(argumentMemberExpression, false));
                                                }

                                                break;

                                            case ExpressionType.Lambda:
                                                {
                                                    LambdaExpression argumentLambdaExpression = argumentExpression as LambdaExpression;
                                                    if (argumentLambdaExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(methodCallExpression.Arguments)}[{idx}] to be of type {nameof(LambdaExpression)}\r\n\t{expression}", nameof(expression));

                                                    ParameterExpression argumentParameterExpression = argumentLambdaExpression.Parameters.SingleOrDefault() as ParameterExpression;
                                                    if (argumentParameterExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(methodCallExpression.Arguments)}[{idx}].{nameof(LambdaExpression.Parameters)}[0] to be of type {nameof(ParameterExpression)}\r\n\t{argumentLambdaExpression}", nameof(expression));

                                                    string inner = GetFilterExpression(argumentLambdaExpression);
                                                    parts.Add(Constants.ODataMemberAccessOperator);
                                                    parts.Add(inner);
                                                }

                                                break;

                                            default:
                                                throw new ArgumentException($"Invalid expression type {argumentExpression.NodeType}\r\n\t{expression}", nameof(expression));
                                        }
                                    }

                                    return string.Join(string.Empty, parts);
                                }

                            case nameof(string.Format):
                                {
                                    Func<string> stringFormat = Expression.Lambda<Func<string>>(Expression.Convert(methodCallExpression, methodCallExpression.Type)).Compile();
                                    return stringFormat();
                                }

                            default:
                                throw new ArgumentException($"Invalid method {methodCallExpression.Method}\r\n\t{expression}", nameof(expression));
                        }
                    }

                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    {
                        BinaryExpression binaryExpression = expression as BinaryExpression;
                        if (binaryExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(LambdaExpression.Body)} to be of type {nameof(BinaryExpression)}\r\n\t{expression}", nameof(expression));

                        string op = expression.NodeType == ExpressionType.And || expression.NodeType == ExpressionType.AndAlso
                            ? "and"
                            : "or";

                        return $"({GetFilterExpression(binaryExpression.Left)}) {op} ({GetFilterExpression(binaryExpression.Right)})";
                    }


                default:
                    throw new ArgumentException($"Invalid expression type {expression.NodeType}\r\n\t{expression}", nameof(expression));
            }
        }

        /// <summary>
        /// Get the OData filter expression.
        /// </summary>
        /// <param name="binaryExpression">The expression from which to parse the OData filter.</param>
        /// <returns>the OData filter.</returns>
        public static string GetFilterExpression(BinaryExpression binaryExpression)
        {
            if (binaryExpression == null || binaryExpression.Left == null || binaryExpression.Right == null) throw new ArgumentNullException(nameof(binaryExpression));

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
                    throw new ArgumentException($"Invalid expression type {binaryExpression.NodeType}\r\n\t{binaryExpression}", nameof(binaryExpression));
            }

            string left = null;
            switch (binaryExpression.Left.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = binaryExpression.Left as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(binaryExpression)}.{nameof(BinaryExpression.Left)} to be of type {nameof(MemberExpression)}\r\n\t{binaryExpression}", nameof(binaryExpression));

                        left = PropertyNameUtility.GetPropertyName(memberExpression, false);
                    }

                    break;

                case ExpressionType.Parameter:
                    left = string.Empty;
                    break;

                case ExpressionType.Convert:
                    {
                        UnaryExpression unaryExpression = binaryExpression.Left as UnaryExpression;
                        if (unaryExpression == null) throw new ArgumentException($"Expected {nameof(binaryExpression)}.{nameof(BinaryExpression.Left)} to be of type {nameof(UnaryExpression)}\r\n\t{binaryExpression}", nameof(binaryExpression));

                        switch (unaryExpression.Operand.NodeType)
                        {
                            case ExpressionType.MemberAccess:
                                {
                                    MemberExpression memberExpression = unaryExpression.Operand as MemberExpression;
                                    if (unaryExpression == null) throw new ArgumentException($"Expected {nameof(binaryExpression)}.{nameof(BinaryExpression.Left)}.{nameof(UnaryExpression.Operand)} to be of type {nameof(MemberExpression)}\r\n\t{binaryExpression}", nameof(binaryExpression));

                                    left = PropertyNameUtility.GetPropertyName(memberExpression, false);
                                }

                                break;

                            default:
                                throw new ArgumentException($"Invalid expression type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));
                        }
                    }

                    break;

                default:
                    throw new ArgumentException($"Invalid expression type {binaryExpression.Left.NodeType}\r\n\t{binaryExpression}", nameof(binaryExpression));
            }

            object rightValue = null;
            switch (binaryExpression.Right.NodeType)
            {
                case ExpressionType.Constant:
                    {
                        ConstantExpression constantExpression = binaryExpression.Right as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Expected {nameof(binaryExpression)}.{nameof(BinaryExpression.Right)} to be of type {nameof(ConstantExpression)}\r\n\t{binaryExpression}", nameof(binaryExpression));

                        rightValue = constantExpression.Value;
                    }

                    break;

                case ExpressionType.Call:
                    {
                        MethodCallExpression methodCallExpression = binaryExpression.Right as MethodCallExpression;
                        if (methodCallExpression == null) throw new ArgumentException($"Expected {nameof(binaryExpression)}.{nameof(BinaryExpression.Right)} to be of type {nameof(MethodCallExpression)}\r\n\t{binaryExpression}", nameof(binaryExpression));

                        rightValue = methodCallExpression.Method.Invoke(null, new object[0]);
                    }

                    break;

                case ExpressionType.Convert:
                    {
                        UnaryExpression unaryExpression = binaryExpression.Right as UnaryExpression;
                        if (unaryExpression == null) throw new ArgumentException($"Expected {nameof(binaryExpression)}.{nameof(BinaryExpression.Right)} to be of type {nameof(UnaryExpression)}\r\n\t{binaryExpression}", nameof(binaryExpression));

                        rightValue = GetFilterValueForBinaryRightConvert(unaryExpression);
                    }

                    break;

                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = binaryExpression.Right as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(binaryExpression)}.{nameof(BinaryExpression.Right)} to be of type {nameof(MemberExpression)}\r\n\t{binaryExpression}", nameof(binaryExpression));

                        ConstantExpression constantExpression = memberExpression.Expression as ConstantExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(binaryExpression)}.{nameof(BinaryExpression.Right)}.{nameof(MemberExpression.Expression)} to be of type {nameof(ConstantExpression)}\r\n\t{binaryExpression}", nameof(binaryExpression));

                        string fieldName = memberExpression.Member.Name;
                        object expressionValue = constantExpression.Value;
                        FieldInfo fieldInfo = expressionValue.GetType().GetField(fieldName, BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance);
                        if (fieldInfo == null) throw new ArgumentException($"Expected {nameof(binaryExpression)}.{nameof(BinaryExpression.Right)}.{nameof(MemberExpression.Expression)} is malformed\r\n\t{binaryExpression}", nameof(binaryExpression));

                        rightValue = fieldInfo.GetValue(expressionValue);
                    }

                    break;

                default:
                    throw new ArgumentException($"Invalid expression type {binaryExpression.Right.NodeType}\r\n\t{binaryExpression}", nameof(binaryExpression));
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

        /// <summary>
        /// Get the OData filter expression.
        /// </summary>
        /// <param name="unaryExpression">The expression from which to parse the OData filter.</param>
        /// <returns>the OData filter.</returns>
        public static string GetFilterExpression(UnaryExpression unaryExpression)
        {
            if (unaryExpression == null) throw new ArgumentNullException(nameof(unaryExpression));

            string operand = null;
            switch (unaryExpression.Operand.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = unaryExpression.Operand as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(unaryExpression)}.{nameof(UnaryExpression.Operand)} to be of type {nameof(MemberExpression)}\r\n\t{unaryExpression}", nameof(unaryExpression));

                        operand = PropertyNameUtility.GetPropertyName(memberExpression, false);
                    }

                    break;

                default:
                    throw new ArgumentException($"Invalid expression type {unaryExpression.Operand.NodeType}\r\n\t{unaryExpression}", nameof(unaryExpression));
            }

            switch (unaryExpression.NodeType)
            {
                case ExpressionType.IsFalse:
                case ExpressionType.Not:
                    return $"not {operand}";

                case ExpressionType.IsTrue:
                    return $"{operand}";

                default:
                    throw new ArgumentException($"Invalid expression type {unaryExpression.NodeType}\r\n\t{unaryExpression}", nameof(unaryExpression));
            }
        }

        /// <summary>
        /// Get the right value from a binary expression whose right expression is a convert expression.
        /// </summary>
        /// <param name="unaryExpression">The expression from which to parse the value</param>
        /// <returns>the value.</returns>
        private static object GetFilterValueForBinaryRightConvert(UnaryExpression unaryExpression)
        {
            if (unaryExpression == null) throw new ArgumentNullException(nameof(unaryExpression));

            switch (unaryExpression.Operand.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = unaryExpression.Operand as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(unaryExpression)}.{nameof(UnaryExpression.Operand)} to be of type {nameof(MemberExpression)}\r\n\t{unaryExpression}", nameof(unaryExpression));

                        ConstantExpression constantExpression = memberExpression.Expression as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Expected {nameof(unaryExpression)}.{nameof(UnaryExpression.Operand)}.{nameof(MemberExpression.Expression)} to be of type {nameof(ConstantExpression)}\r\n\t{unaryExpression}", nameof(unaryExpression));

                        string fieldName = memberExpression.Member.Name;
                        object expressionValue = constantExpression.Value;
                        FieldInfo fieldInfo = expressionValue.GetType().GetField(fieldName, BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance);
                        if (fieldInfo == null) throw new ArgumentException($"Expected {nameof(unaryExpression)}.{nameof(UnaryExpression.Operand)}.{nameof(MemberExpression.Expression)} is malformed\r\n\t{unaryExpression}", nameof(unaryExpression));

                        return fieldInfo.GetValue(expressionValue);
                    }

                case ExpressionType.Constant:
                    {
                        ConstantExpression constantExpression = unaryExpression.Operand as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Expected {nameof(unaryExpression)}.{nameof(UnaryExpression.Operand)} to be of type {nameof(ConstantExpression)}\r\n\t{unaryExpression}", nameof(unaryExpression));

                        return constantExpression.Value;
                    }

                case ExpressionType.Convert:
                    {
                        UnaryExpression innerUnaryExpression = unaryExpression.Operand as UnaryExpression;
                        if (innerUnaryExpression == null) throw new ArgumentException($"Expected {nameof(unaryExpression)}.{nameof(UnaryExpression.Operand)} to be of type {nameof(UnaryExpression)}\r\n\t{unaryExpression}", nameof(unaryExpression));

                        return GetFilterValueForBinaryRightConvert(innerUnaryExpression);
                    }

                case ExpressionType.New:
                    {
                        NewExpression newExpression = unaryExpression.Operand as NewExpression;
                        if (newExpression == null) throw new ArgumentException($"Expected {nameof(unaryExpression)}.{nameof(UnaryExpression.Operand)} to be of type {nameof(NewExpression)}\r\n\t{unaryExpression}", nameof(unaryExpression));

                        return Activator.CreateInstance(newExpression.Type, newExpression.Arguments.Select(a => (a as ConstantExpression).Value).ToArray());
                    }

                default:
                    throw new ArgumentException($"Invalid expression type {unaryExpression.NodeType}\r\n\t{unaryExpression}", nameof(unaryExpression));
            }
        }
    }
}
