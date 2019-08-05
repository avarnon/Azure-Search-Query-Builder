using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AzureSearchQueryBuilder.Builders
{
    public abstract class ParametersBuilder<TModel, TOptions> : IParametersBuilder<TModel, TOptions>
    {
        public const string SearchScore = "search.score()";
        private const string ODataMemberAccessOperator = "/";

        private IList<string> _searchFields;

        public string Filter { get; private set; }

        public string HighlightPostTag { get; private set; }

        public string HighlightPreTag { get; private set; }

        public double? MinimumCoverage { get; private set; }

        public IEnumerable<string> SearchFields { get => _searchFields; }

        public JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings();

        public int? Top { get; private set; }

        public abstract TOptions Build();

        public IParametersBuilder<TModel, TOptions> Where<TProperty>(Expression<BooleanLambdaDelegate<TModel>> lambdaExpression)
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

        public IParametersBuilder<TModel, TOptions> WithHighlightPostTag(string highlightPostTag)
        {
            this.HighlightPostTag = highlightPostTag;
            return this;
        }

        public IParametersBuilder<TModel, TOptions> WithHighlightPreTag(string highlightPreTag)
        {
            this.HighlightPreTag = highlightPreTag;
            return this;
        }

        public IParametersBuilder<TModel, TOptions> WithMinimumCoverage(double? minimumCoverage)
        {
            this.MinimumCoverage = minimumCoverage;
            return this;
        }

        public IParametersBuilder<TModel, TOptions> WithSearchField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
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

        public IParametersBuilder<TModel, TOptions> WithTop(int? top)
        {
            this.Top = top;
            return this;
        }

        protected static string GetFilterExpression(LambdaExpression lambdaExpression)
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
                        return GetPropertyName(memberExpression);
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
                                                    parts.Add(GetPropertyName(argumentMemberExpression));
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
                                                    parts.Add(GetPropertyName(argumentMemberExpression));
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

        protected static string GetPropertyName(LambdaExpression lambdaExpression)
        {
            if (lambdaExpression == null || lambdaExpression.Body == null) throw new ArgumentNullException(nameof(lambdaExpression));

            switch (lambdaExpression.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = lambdaExpression.Body as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body.GetType()}", nameof(lambdaExpression));

                        return GetPropertyName(memberExpression);
                    }

                case ExpressionType.Call:
                    {
                        MethodCallExpression methodCallExpression = lambdaExpression.Body as MethodCallExpression;
                        if (methodCallExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body.GetType()}", nameof(lambdaExpression));

                        return GetPropertyName(methodCallExpression);
                    }

                case ExpressionType.Constant:
                    {
                        ConstantExpression constantExpression = lambdaExpression.Body as ConstantExpression;
                        if (constantExpression == null) throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));

                        return constantExpression.Value?.ToString();
                    }

                default:
                    throw new ArgumentException($"Invalid expression body type {lambdaExpression.Body?.GetType()}", nameof(lambdaExpression));
            }
        }

        private static string GetFilterExpression(BinaryExpression binaryExpression)
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
                    throw new ArgumentException($"Invalid expression body type {binaryExpression.GetType()}", nameof(binaryExpression));
            }

            string left = null;
            switch (binaryExpression.Left.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = binaryExpression.Left as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Invalid expression body type {binaryExpression.Left.GetType()}", nameof(binaryExpression));

                        left = GetPropertyName(memberExpression);
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

                                    left = GetPropertyName(memberExpression);
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

                                    rightValue = fieldInfo.GetValue(expressionValue);
                                }

                                break;

                            case ExpressionType.Constant:
                                {
                                    ConstantExpression constantExpression = unaryExpression.Operand as ConstantExpression;
                                    if (constantExpression == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));

                                    rightValue = constantExpression.Value;
                                }

                                break;

                            case ExpressionType.Convert:
                                {
                                    UnaryExpression innerUnaryExpression = unaryExpression.Operand as UnaryExpression;
                                    if (innerUnaryExpression == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));

                                    switch (innerUnaryExpression.Operand.NodeType)
                                    {
                                        case ExpressionType.MemberAccess:
                                            {
                                                MemberExpression memberExpression = innerUnaryExpression.Operand as MemberExpression;
                                                if (memberExpression == null) throw new ArgumentException($"Invalid expression body type {innerUnaryExpression.Operand.GetType()}", nameof(binaryExpression));

                                                ConstantExpression constantExpression = memberExpression.Expression as ConstantExpression;
                                                if (constantExpression == null) throw new ArgumentException($"Invalid expression body type {innerUnaryExpression.Operand.GetType()}", nameof(binaryExpression));

                                                string fieldName = memberExpression.Member.Name;
                                                object expressionValue = constantExpression.Value;
                                                FieldInfo fieldInfo = expressionValue.GetType().GetField(fieldName, BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance);
                                                if (fieldInfo == null) throw new ArgumentException($"Invalid expression body type {innerUnaryExpression.Operand.GetType()}", nameof(binaryExpression));

                                                rightValue = fieldInfo.GetValue(expressionValue);
                                            }

                                            break;

                                        case ExpressionType.Constant:
                                            {
                                                ConstantExpression constantExpression = innerUnaryExpression.Operand as ConstantExpression;
                                                if (constantExpression == null) throw new ArgumentException($"Invalid expression body type {innerUnaryExpression.Operand.GetType()}", nameof(binaryExpression));

                                                rightValue = constantExpression.Value;
                                            }

                                            break;

                                        case ExpressionType.Convert:
                                            {
                                                ConstantExpression constantExpression = innerUnaryExpression.Operand as ConstantExpression;
                                                if (constantExpression == null) throw new ArgumentException($"Invalid expression body type {innerUnaryExpression.Operand.GetType()}", nameof(binaryExpression));

                                                rightValue = constantExpression.Value;
                                            }

                                            break;

                                        default:
                                            throw new ArgumentException($"Invalid expression body type {innerUnaryExpression.Operand.GetType()}", nameof(binaryExpression));
                                    }
                                }

                                break;

                            case ExpressionType.New:
                                {
                                    NewExpression newExpression = unaryExpression.Operand as NewExpression;
                                    if (newExpression == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));

                                    rightValue = Activator.CreateInstance(newExpression.Type, newExpression.Arguments.Select(a => (a as ConstantExpression).Value).ToArray());
                                }

                                break;

                            default:
                                throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(binaryExpression));
                        }
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

        private static string GetFilterExpression(UnaryExpression unaryExpression)
        {
            if (unaryExpression == null) throw new ArgumentNullException(nameof(unaryExpression));

            string operand = null;
            switch (unaryExpression.Operand.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression memberExpression = unaryExpression.Operand as MemberExpression;
                        if (memberExpression == null) throw new ArgumentException($"Invalid expression body type {unaryExpression.Operand.GetType()}", nameof(unaryExpression));

                        operand = GetPropertyName(memberExpression);
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

        private static string GetPropertyName(MemberExpression memberExpression)
        {
            if (memberExpression == null) throw new ArgumentNullException(nameof(memberExpression));

            string leafPropertyName = null;
            PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo != null)
            {
                leafPropertyName = GetPropertyName(propertyInfo);
            }
            else
            {
                FieldInfo fieldInfo = memberExpression.Member as FieldInfo;
                if (fieldInfo == null) throw new ArgumentException($"Invalid expression body type {memberExpression.Member.GetType()}", nameof(memberExpression));

                leafPropertyName = GetPropertyName(fieldInfo);
            }

            if (memberExpression.Expression == null) return leafPropertyName;

            switch (memberExpression.Expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression childMemberExpression = memberExpression.Expression as MemberExpression;
                        if (childMemberExpression == null) throw new ArgumentException($"Invalid expression body type {memberExpression.Expression.GetType()}", nameof(memberExpression));

                        return string.Concat(
                            GetPropertyName(childMemberExpression),
                            ODataMemberAccessOperator,
                            leafPropertyName);
                    }

                case ExpressionType.Call:
                    {
                        MethodCallExpression childMethodCallExpression = memberExpression.Expression as MethodCallExpression;
                        if (childMethodCallExpression == null) throw new ArgumentException($"Invalid expression body type {memberExpression.Expression.GetType()}", nameof(memberExpression));

                        return string.Concat(
                            GetPropertyName(childMethodCallExpression),
                            ODataMemberAccessOperator,
                            leafPropertyName);
                    }

                case ExpressionType.Parameter:
                    return leafPropertyName;

                default:
                    throw new ArgumentException($"Invalid expression body type {memberExpression.Expression.GetType()}", nameof(memberExpression));
            }
        }

        private static string GetPropertyName(MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression == null) throw new ArgumentNullException(nameof(methodCallExpression));

            IList<string> tokens = new List<string>();
            foreach (Expression argumentExpression in methodCallExpression.Arguments)
            {
                switch (argumentExpression.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        {
                            MemberExpression argumentMemberExpression = argumentExpression as MemberExpression;
                            if (argumentMemberExpression == null) throw new ArgumentException($"Invalid expression body type {argumentExpression.GetType()}", nameof(methodCallExpression));

                            tokens.Add(GetPropertyName(argumentMemberExpression));
                        }

                        break;

                    case ExpressionType.Lambda:
                        {
                            LambdaExpression argumentLambdaExpression = argumentExpression as LambdaExpression;
                            if (argumentLambdaExpression == null) throw new ArgumentException($"Invalid expression body type {argumentExpression.GetType()}", nameof(methodCallExpression));

                            tokens.Add(GetPropertyName(argumentLambdaExpression));
                        }

                        break;

                    default:
                        throw new ArgumentException($"Invalid expression body type {argumentExpression.GetType()}", nameof(methodCallExpression));
                }
            }

            return string.Join(ODataMemberAccessOperator, tokens);
        }

        private static string GetPropertyName(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            JsonPropertyAttribute jsonProperty = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>(true);

            if (jsonProperty == null)
            {
                return string.Concat(propertyInfo.Name.Substring(0, 1).ToLowerInvariant(), propertyInfo.Name.Substring(1));
            }
            else
            {
                return jsonProperty.PropertyName;
            }
        }

        private static string GetPropertyName(FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));

            JsonPropertyAttribute jsonProperty = fieldInfo.GetCustomAttribute<JsonPropertyAttribute>(true);

            if (jsonProperty == null)
            {
                return string.Concat(fieldInfo.Name.Substring(0, 1).ToLowerInvariant(), fieldInfo.Name.Substring(1));
            }
            else
            {
                return jsonProperty.PropertyName;
            }
        }
    }
}
