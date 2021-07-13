using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace tonkica
{
    internal static class QueryExtensions
    {
        internal static string RemoveDiacriticsAndNormalize(this string text)
        {
            var normalizedString = text
                .ToUpperInvariant()
                .Normalize(NormalizationForm.FormD);

            var stringBuilder = new StringBuilder(normalizedString.Length);

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        internal static IQueryable<T> IncludeHiddenDb<T>(this IQueryable<T> query, bool includeHidden, string isHiddenProperty)
        {
            if (includeHidden)
                return query;
            else
                return query.Where(o => EF.Property<object>(o, isHiddenProperty) == null);
        }
        internal static IQueryable<T> IncludeHidden<T>(this IQueryable<T> query, bool includeHidden, string isHiddenProperty)
        {
            if (includeHidden)
                return query;
            else
            {
                var propertyInfo = typeof(T).GetProperty(isHiddenProperty);
                return query.Where(o => propertyInfo!.GetValue(o) == null);
            }
        }
        internal static IQueryable<T> PageDb<T>(this IQueryable<T> query, int skip, int take)
            => query
                .Skip(skip)
                .Take(take);
        internal static IQueryable<T> Page<T>(this IQueryable<T> query, int skip, int take)
            => query
                .Skip(skip)
                .Take(take);
        internal static IQueryable<T> FilterDb<T>(this IQueryable<T> query, string? term, params string[] properties)
        {
            if (string.IsNullOrWhiteSpace(term))
                return query;

            /*
                SQLite default LIKE is case insensitive for ANSI chars, but case sensitive for other chars.
                To search all in insensitive manner, add additional searchable column NORMALIZED
                which converts accented chars to non accented and upper case. Then the same must be done for
                the search term. 
            */

            //var searchTerm = $"%{term.RemoveDiacriticsAndNormalize()}%";
            var searchTerm = $"%{term}%";

            var pred = PredicateBuilder.False<T>();
            foreach (var property in properties)
                pred = pred.Or(o => EF.Functions.Like(EF.Property<string>(o, property), searchTerm));

            return query.Where(pred);
        }
        internal static IQueryable<T> Filter<T>(this IQueryable<T> query, string? term, params string[] properties)
        {
            if (string.IsNullOrWhiteSpace(term))
                return query;

            var searchTerm = term.ToUpper();

            var pred = PredicateBuilder.False<T>();
            foreach (var property in properties)
            {
                var propertyInfo = typeof(T).GetProperty(property);
                pred = pred.Or(o => propertyInfo!.GetValue(o)!.ToString()!.Contains(searchTerm));
            }

            return query.Where(pred);
        }
        internal static IQueryable<T> SortDb<T>(this IQueryable<T> query, string? sortBy, bool descending, string? defaultProperty = default)
        {
            string? property = null;
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                // Json formatters converts PascalCase to camelCase so we need to put it back to PascalCase
                // char[] a = sortBy.ToCharArray();
                // a[0] = char.ToUpper(a[0]);
                // property = new string(a);
                property = sortBy;
            }
            else if (!string.IsNullOrWhiteSpace(defaultProperty))
                property = defaultProperty;
            else
                return query;

            if (typeof(T).GetProperty(property) == null)
                return query;

            return query = descending ?
                  query.OrderByDescending(o => EF.Property<object>(o, property)) :
                  query.OrderBy(o => EF.Property<object>(o, property));
        }
        internal static IQueryable<T> Sort<T>(this IQueryable<T> query, string? sortBy, bool descending, string? defaultProperty = default)
        {
            string? property = null;
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                // Json formatters converts PascalCase to camelCase so we need to put it back to PascalCase
                // char[] a = sortBy.ToCharArray();
                // a[0] = char.ToUpper(a[0]);
                // property = new string(a);
                property = sortBy;
            }
            else if (!string.IsNullOrWhiteSpace(defaultProperty))
                property = defaultProperty;
            else
                return query;

            var propertyInfo = typeof(T).GetProperty(property);
            if (propertyInfo == null)
                return query;

            return query = descending ?
                  query.OrderByDescending(o => propertyInfo.GetValue(o)) :
                  query.OrderBy(o => propertyInfo.GetValue(o));
        }
    }

    /// <summary>
    /// Enables the efficient, dynamic composition of query predicates.
    /// </summary>
    internal static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that evaluates to true.
        /// </summary>
        internal static Expression<Func<T, bool>> True<T>() { return param => true; }

        /// <summary>
        /// Creates a predicate that evaluates to false.
        /// </summary>
        internal static Expression<Func<T, bool>> False<T>() { return param => false; }

        /// <summary>
        /// Creates a predicate expression from the specified lambda expression.
        /// </summary>
        internal static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

        /// <summary>
        /// Combines the first predicate with the second using the logical "and".
        /// </summary>
        internal static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// Combines the first predicate with the second using the logical "or".
        /// </summary>
        internal static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>
        /// Negates the predicate.
        /// </summary>
        internal static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> map;

            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            internal static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {

                if (map.TryGetValue(p, out var replacement))
                    p = replacement;

                return base.VisitParameter(p);
            }
        }
    }
}