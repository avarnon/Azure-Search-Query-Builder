using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AzureSearchQueryBuilder.Models;
using Microsoft.Azure.Search.Models;

namespace AzureSearchQueryBuilder.Builders
{
    public class SearchParametersBuilder<TModel> : ParametersBuilder<TModel, SearchParameters>, ISearchParametersBuilder<TModel>, IOrderedSearchParametersBuilder<TModel>
        where TModel : SearchModel
    {
        private IList<string> _facets;
        private IList<string> _highlightFields;
        private IList<string> _orderBy;
        private IList<ScoringParameter> _scoringParameters;
        private IList<string> _select;

        private SearchParametersBuilder()
        {
        }

        public IEnumerable<string> Facets { get => this._facets; }

        public IEnumerable<string> HighlightFields { get => this._highlightFields; }

        public bool IncludeTotalResultCount { get; private set; }

        public IEnumerable<string> OrderBy { get => this._orderBy; }

        public QueryType QueryType { get; private set; }

        public IEnumerable<ScoringParameter> ScoringParameters { get => this._scoringParameters; }

        public string ScoringProfile { get; private set; }

        public SearchMode SearchMode { get; private set; }

        public IEnumerable<string> Select { get => this._select; }

        public int? Skip { get; private set; }

        public static ISearchParametersBuilder<TModel> Create() => new SearchParametersBuilder<TModel>();

        public override SearchParameters Build()
        {
            return new SearchParameters()
            {
                Facets = this.Facets?.ToList(),
                Filter = this.Filter,
                HighlightFields = this.HighlightFields?.ToList(),
                HighlightPostTag = this.HighlightPostTag,
                HighlightPreTag = this.HighlightPreTag,
                IncludeTotalResultCount = this.IncludeTotalResultCount,
                MinimumCoverage = this.MinimumCoverage,
                OrderBy = this.OrderBy?.ToList(),
                QueryType = this.QueryType,
                ScoringParameters = this.ScoringParameters?.ToList(),
                ScoringProfile = this.ScoringProfile,
                SearchFields = this.SearchFields?.ToList(),
                SearchMode = this.SearchMode,
                Select = this.Select?.ToList(),
                Skip = this.Skip,
                Top = this.Top,
            };
        }

        public ISearchParametersBuilder<TModel> WithFacet<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._facets == null)
            {
                this._facets = new List<string>();
            }

            string facet = GetPropertyName(lambdaExpression);
            this._facets.Add(facet);
            return this;
        }

        public ISearchParametersBuilder<TModel> WithHighlightField<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._highlightFields == null)
            {
                this._highlightFields = new List<string>();
            }

            string highlightField = GetPropertyName(lambdaExpression);
            this._highlightFields.Add(highlightField);
            return this;
        }

        public ISearchParametersBuilder<TModel> WithIncludeTotalResultCount(bool includeTotalResultCount)
        {
            this.IncludeTotalResultCount = includeTotalResultCount;
            return this;
        }

        public IOrderedSearchParametersBuilder<TModel> WithOrderBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            this._orderBy = new List<string>();

            string orderBy = GetPropertyName(lambdaExpression);
            this._orderBy.Add($"{orderBy} asc");
            return this;
        }

        public IOrderedSearchParametersBuilder<TModel> WithOrderByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            this._orderBy = new List<string>();

            string orderBy = GetPropertyName(lambdaExpression);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }

        public ISearchParametersBuilder<TModel> WithQueryType(QueryType queryType)
        {
            this.QueryType = queryType;
            return this;
        }

        public ISearchParametersBuilder<TModel> WithScoringParameter(ScoringParameter scoringParameter)
        {
            if (scoringParameter == null) throw new ArgumentNullException(nameof(scoringParameter));

            if (this._scoringParameters == null)
            {
                this._scoringParameters = new List<ScoringParameter>();
            }

            this._scoringParameters.Add(scoringParameter);
            return this;
        }

        public ISearchParametersBuilder<TModel> WithScoringProfile(string scoringProfile)
        {
            this.ScoringProfile = scoringProfile;
            return this;
        }

        public ISearchParametersBuilder<TModel> WithSearchMode(SearchMode searchMode)
        {
            this.SearchMode = searchMode;
            return this;
        }

        public ISearchParametersBuilder<TModel> WithSelect<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            if (this._select == null)
            {
                this._select = new List<string>();
            }

            string selectField = GetPropertyName(lambdaExpression);
            this._select.Add(selectField);
            return this;
        }

        public ISearchParametersBuilder<TModel> WithSkip(int? skip)
        {
            this.Skip = skip;
            return this;
        }

        public IOrderedSearchParametersBuilder<TModel> WithThenBy<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));
            if (this._orderBy == null || this._orderBy.Count < 1) throw new Exception();

            string orderBy = GetPropertyName(lambdaExpression);
            this._orderBy.Add($"{orderBy} asc");
            return this;
        }

        public IOrderedSearchParametersBuilder<TModel> WithThenByDescending<TProperty>(Expression<PropertyLambdaDelegate<TModel, TProperty>> lambdaExpression)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));
            if (this._orderBy == null || this._orderBy.Count < 1) throw new Exception();

            string orderBy = GetPropertyName(lambdaExpression);
            this._orderBy.Add($"{orderBy} desc");
            return this;
        }
    }
}
