using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AzureSearchQueryBuilder.Builders;
using Newtonsoft.Json;

namespace AzureSearchQueryBuilder.Helpers
{
    /// <summary>
    /// A helper class for getting expression values.
    /// </summary>
    internal static class ExpressionValueUtility
    {
        /// <summary>
        /// Get the value from an expression.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns>the value.</returns>
        public static object GetValue(this Expression expression, JsonSerializerSettings jsonSerializerSettings)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    {
                        ConstantExpression constantExpression = expression as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(ConstantExpression)}\r\n\t{expression}", nameof(expression));

                        return constantExpression.GetValue();
                    }

                case ExpressionType.Call:
                    {
                        MethodCallExpression methodCallExpression = expression as MethodCallExpression;
                        if (methodCallExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(MethodCallExpression)}\r\n\t{expression}", nameof(expression));

                        return methodCallExpression.GetValue(jsonSerializerSettings);
                    }

                case ExpressionType.Convert:
                    {
                        UnaryExpression unaryExpression = expression as UnaryExpression;
                        if (unaryExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(UnaryExpression)}\r\n\t{expression}", nameof(expression));

                        return unaryExpression.GetValue(jsonSerializerSettings);
                    }

                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = expression as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(MemberExpression)}\r\n\t{expression}", nameof(expression));

                        return memberExpression.GetValue(jsonSerializerSettings);
                    }

                case ExpressionType.New:
                    {
                        NewExpression newExpression = expression as NewExpression;
                        if (newExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(NewExpression)}\r\n\t{expression}", nameof(expression));

                        return newExpression.GetValue(jsonSerializerSettings);
                    }

                case ExpressionType.Not:
                    {
                        UnaryExpression unaryExpression = expression as UnaryExpression;
                        if (unaryExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(UnaryExpression)}\r\n\t{expression}", nameof(expression));

                        object unaryValue = unaryExpression.GetValue(jsonSerializerSettings);
                        if (unaryValue == null ||
                            (unaryValue.GetType() != typeof(bool) && unaryValue.GetType() != typeof(bool?)))
                        {
                            throw new ArgumentException($"Invalid type", nameof(expression));
                        }

                        return !((bool)unaryValue);
                    }

                case ExpressionType.Parameter:
                    {
                        ParameterExpression parameterExpression = expression as ParameterExpression;
                        if (parameterExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(ParameterExpression)}\r\n\t{expression}", nameof(expression));

                        throw new ArgumentException($"Invalid expression type {expression.NodeType}\r\n\t{expression}", nameof(expression));
                    }


                case ExpressionType.NewArrayInit:
                    {
                        NewArrayExpression newArrayExpression = expression as NewArrayExpression;
                        if (newArrayExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(NewArrayExpression)}\r\n\t{expression}", nameof(expression));

                        return newArrayExpression.Expressions.Select(_ => _.GetValue(jsonSerializerSettings)).ToArray();
                    }

                default:
                    throw new ArgumentException($"Invalid expression type {expression.NodeType}\r\n\t{expression}", nameof(expression));
            }
        }

        /// <summary>
        /// Get the value from an expression.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns>the value.</returns>
        public static object GetValue(this ConstantExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return expression.Value;
        }

        /// <summary>
        /// Get the value from an expression.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns>the value.</returns>
        public static object GetValue(this MemberExpression expression, JsonSerializerSettings jsonSerializerSettings)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            object memberValue = expression.Expression.GetValue(jsonSerializerSettings);

            if (memberValue == null) return null;
            if (expression.Type == memberValue.GetType()) return memberValue;

            return expression.Member.GetValue(memberValue);
        }

        /// <summary>
        /// Get the value from an expression.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns>the value.</returns>
        public static object GetValue(this MethodCallExpression expression, JsonSerializerSettings jsonSerializerSettings)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            if (expression.Method.DeclaringType == typeof(SearchFns))
            {
                switch (expression.Method.Name)
                {
                    case nameof(SearchFns.IsMatch):
                        {
                            string search = expression.Arguments[0].GetValue(jsonSerializerSettings) as string;
                            NewArrayExpression searchFieldsNewArrayExpression = expression.Arguments[1] as NewArrayExpression;
                            IEnumerable<string> searchFields = searchFieldsNewArrayExpression.Expressions.Select(_ => PropertyNameUtility.GetPropertyName(_, jsonSerializerSettings, false).ToString()).ToArray();
                            return $"search.ismatch('{search}', '{string.Join(", ", searchFields)}')";
                        }

                    case nameof(SearchFns.IsMatchScoring):
                        {
                            string search = expression.Arguments[0].GetValue(jsonSerializerSettings) as string;
                            NewArrayExpression searchFieldsNewArrayExpression = expression.Arguments[1] as NewArrayExpression;
                            IEnumerable<string> searchFields = searchFieldsNewArrayExpression.Expressions.Select(_ => PropertyNameUtility.GetPropertyName(_, jsonSerializerSettings, false).ToString()).ToArray();
                            return $"search.ismatchscoring('{search}', '{string.Join(", ", searchFields)}')";
                        }

                    case nameof(SearchFns.In):
                        {
                            string variable = PropertyNameUtility.GetPropertyName(expression.Arguments[0], jsonSerializerSettings, false);
                            NewArrayExpression valueListNewArrayExpression = expression.Arguments[1] as NewArrayExpression;
                            IEnumerable<object> valueList = valueListNewArrayExpression.Expressions.Select(_ => _.GetValue(jsonSerializerSettings)).ToArray();
                            return $"search.in('{variable}', '{string.Join(", ", valueList.Select(_ => _.ToString()).ToArray())}')";
                        }

                    case nameof(SearchFns.Score):
                        return "search.score()";

                    default:
                        throw new ArgumentException($"Invalid method {expression.Method}\r\n\t{expression}", nameof(expression));
                }
            }
            else
            {
                return expression.Method.Invoke(
                    expression.Object.GetValue(jsonSerializerSettings),
                    expression.Arguments.Select(_ => _.GetValue(jsonSerializerSettings)).ToArray());
            }
        }

        /// <summary>
        /// Get the value from an expression.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns>the value.</returns>
        public static object GetValue(this NewExpression expression, JsonSerializerSettings jsonSerializerSettings)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return Activator.CreateInstance(expression.Type, expression.Arguments.Select(a => a.GetValue(jsonSerializerSettings)).ToArray());
        }

        /// <summary>
        /// Get the value from an expression.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns>the value.</returns>
        public static object GetValue(this UnaryExpression expression, JsonSerializerSettings jsonSerializerSettings)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return expression.Operand.GetValue(jsonSerializerSettings);
        }

        /// <summary>
        /// Get the value from a member.
        /// </summary>
        /// <param name="memberInfo">The member to be evaluated.</param>
        /// <param name="obj">The object whose member value will be returned.</param>
        /// <returns>the value.</returns>
        private static object GetValue(this MemberInfo memberInfo, object obj)
        {
            if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return (memberInfo as FieldInfo).GetValue(obj);

                case MemberTypes.Property:
                    return (memberInfo as PropertyInfo).GetValue(obj);

                default:
                    throw new ArgumentException($"Invalid member type {memberInfo.MemberType}\r\n\t{memberInfo}", nameof(memberInfo));
            }
        }
    }
}
