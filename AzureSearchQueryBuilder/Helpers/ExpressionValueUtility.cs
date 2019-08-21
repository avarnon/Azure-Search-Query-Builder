using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AzureSearchQueryBuilder.Helpers
{
    /// <summary>
    /// A helper class for getting expression values.
    /// </summary>
    internal static class ExpressionValueUtility
    {
        public static object GetValue(this Expression expression)
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

                        return methodCallExpression.GetValue();
                    }

                case ExpressionType.Convert:
                    {
                        UnaryExpression unaryExpression = expression as UnaryExpression;
                        if (unaryExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(UnaryExpression)}\r\n\t{expression}", nameof(expression));

                        return unaryExpression.GetValue();
                    }

                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = expression as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(MemberExpression)}\r\n\t{expression}", nameof(expression));

                        return memberExpression.GetValue();
                    }

                case ExpressionType.New:
                    {
                        NewExpression newExpression = expression as NewExpression;
                        if (newExpression == null) throw new ArgumentException($"Expected {nameof(expression)} to be of type {nameof(NewExpression)}\r\n\t{expression}", nameof(expression));

                        return newExpression.GetValue();
                    }

                default:
                    throw new ArgumentException($"Invalid expression type {expression.NodeType}\r\n\t{expression}", nameof(expression));
            }
        }

        public static object GetValue(this ConstantExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return expression.Value;
        }

        public static object GetValue(this MemberExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            object memberValue = null;

            switch (expression.Expression.NodeType)
            {
                case ExpressionType.Constant:
                    {
                        ConstantExpression constantExpression = expression.Expression as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(MemberExpression.Expression)} to be of type {nameof(ConstantExpression)}\r\n\t{expression}", nameof(expression));

                        memberValue = constantExpression.GetValue();
                    }

                    break;

                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = expression.Expression as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(MemberExpression.Expression)} to be of type {nameof(MemberExpression)}\r\n\t{expression}", nameof(expression));

                        memberValue = memberExpression.GetValue();
                    }

                    break;

                default:
                    throw new ArgumentException($"Invalid expression type {expression.Expression.NodeType}\r\n\t{expression.Expression}", nameof(expression));
            }

            if (memberValue == null) return null;
            if (expression.Type == memberValue.GetType()) return memberValue;

            FieldInfo fieldInfo = expression.Member as FieldInfo;
            if (fieldInfo != null)
            {
                object fieldValue = fieldInfo.GetValue(memberValue);

                return fieldValue;
            }
            else
            {
                PropertyInfo propertyInfo = expression.Member as PropertyInfo;
                if (propertyInfo != null)
                {
                    object propertyValue = propertyInfo.GetValue(memberValue);

                    return propertyValue;
                }
                else
                {
                    throw new ArgumentException($"Invalid expression type {expression.Expression.NodeType}\r\n\t{expression.Expression}", nameof(expression));
                }
            }
        }

        public static object GetValue(this MethodCallExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return expression.Method.Invoke(null, new object[0]);
        }

        public static object GetValue(this NewExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return Activator.CreateInstance(expression.Type, expression.Arguments.Select(a => a.GetValue()).ToArray());
        }

        public static object GetValue(this UnaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            switch (expression.Operand.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = expression.Operand as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(UnaryExpression.Operand)} to be of type {nameof(MemberExpression)}\r\n\t{expression}", nameof(expression));

                        return memberExpression.GetValue();
                    }

                case ExpressionType.Constant:
                    {
                        ConstantExpression constantExpression = expression.Operand as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(UnaryExpression.Operand)} to be of type {nameof(ConstantExpression)}\r\n\t{expression}", nameof(expression));

                        return constantExpression.GetValue();
                    }

                case ExpressionType.Convert:
                    {
                        UnaryExpression innerUnaryExpression = expression.Operand as UnaryExpression;
                        if (innerUnaryExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(UnaryExpression.Operand)} to be of type {nameof(UnaryExpression)}\r\n\t{expression}", nameof(expression));

                        return innerUnaryExpression.GetValue();
                    }

                case ExpressionType.New:
                    {
                        NewExpression newExpression = expression.Operand as NewExpression;
                        if (newExpression == null) throw new ArgumentException($"Expected {nameof(expression)}.{nameof(UnaryExpression.Operand)} to be of type {nameof(NewExpression)}\r\n\t{expression}", nameof(expression));

                        return newExpression.GetValue();
                    }

                default:
                    throw new ArgumentException($"Invalid expression type {expression.NodeType}\r\n\t{expression}", nameof(expression));
            }
        }
    }
}
