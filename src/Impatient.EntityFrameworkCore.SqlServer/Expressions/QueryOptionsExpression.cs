﻿using Impatient.Query.Expressions;
using Impatient.Query.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Impatient.EntityFrameworkCore.SqlServer.Expressions
{
    public class QueryOptionsExpression : AnnotationExpression
    {
        public QueryOptionsExpression(
            Expression expression,
            QueryTrackingBehavior queryTrackingBehavior,
            bool ignoreQueryFilters,
            bool useRelationalNullSemantics) : base(expression)
        {
            QueryTrackingBehavior = queryTrackingBehavior;
            IgnoreQueryFilters = ignoreQueryFilters;
            UseRelationalNullSemantics = useRelationalNullSemantics;
        }

        public QueryTrackingBehavior QueryTrackingBehavior { get; }

        public bool IgnoreQueryFilters { get; }

        public bool UseRelationalNullSemantics { get; }

        protected override Expression VisitChildren(ExpressionVisitor visitor)
        {
            var expression = visitor.Visit(Expression);

            if (expression != Expression)
            {
                return new QueryOptionsExpression(
                    expression, 
                    QueryTrackingBehavior, 
                    IgnoreQueryFilters, 
                    UseRelationalNullSemantics);
            }

            return this;
        }

        public override int GetSemanticHashCode(ExpressionEqualityComparer comparer)
        {
            unchecked
            {
                var hash = comparer.GetHashCode(Expression);

                hash = (hash * 16777619) ^ QueryTrackingBehavior.GetHashCode();
                hash = (hash * 16777619) ^ IgnoreQueryFilters.GetHashCode();
                hash = (hash * 16777619) ^ UseRelationalNullSemantics.GetHashCode();

                return hash;
            }
        }
    }
}
