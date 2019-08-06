using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Helpers;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    public class SuggestParametersBuilder<TModel> : ParametersBuilder<TModel, SuggestParameters>, ISuggestParametersBuilder<TModel>, IOrderedSuggestParametersBuilder<TModel>
    {
        private IList<string> _orderBy;
        private IList<string> _select;

        private SuggestParametersBuilder()
        {
        }

        public IEnumerable<string> OrderBy { get => this._orderBy; }

        public IEnumerable<string> Select { get => this._select; }

        public bool UseFuzzyMatching { get; private set; }

        public static ISuggestParametersBuilder<TModel> Create() => new SuggestParametersBuilder<TModel>();

        public override SuggestParameters Build()
        {
            return new SuggestParameters()
            {
                Filter = this.Filter,
                HighlightPostTag = this.HighlightPostTag,
                HighlightPreTag = this.HighlightPreTag,
                MinimumCoverage = this.MinimumCoverage,
                OrderBy = this.OrderBy?.ToList(),
                SearchFields = this.SearchFields?.ToList(),
                Select = this.Select?.ToList(),
                Top = this.Top,
                UseFuzzyMatching = this.UseFuzzyMatching,
            };
        }

        public IOrderedSuggestParametersBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            this._orderBy = new List<string>();

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._orderBy.Add($"{orderBy} asc");
            return this;
        }

        public IOrderedSuggestParametersBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            this._orderBy = new List<string>();

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }

        public ISuggestParametersBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._select == null)
            {
                this._select = new List<string>();
            }

            string selectField = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._select.Add(selectField);
            return this;
        }

        public ISuggestParametersBuilder<TModel> WithUseFuzzyMatching(bool useFuzzyMatching)
        {
            this.UseFuzzyMatching = useFuzzyMatching;
            return this;
        }

        public IOrderedSuggestParametersBuilder<TModel> WithThenBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));
            if (this._orderBy == null || this._orderBy.Count < 1) throw new Exception();

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._orderBy.Add($"{orderBy} asc");
            return this;
        }

        public IOrderedSuggestParametersBuilder<TModel> WithThenByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));
            if (this._orderBy == null || this._orderBy.Count < 1) throw new Exception();

            string orderBy = PropertyNameUtility.GetPropertyName(lambdaExpression, false);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }
    }
}
