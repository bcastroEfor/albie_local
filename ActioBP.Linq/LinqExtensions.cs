using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using ActioBP.Linq.FilterLinq;

namespace System.Linq
{
    public static class LinqExtensions
    {

        #region Order BY
        public static IOrderedQueryable<TSource> OrderByAct<TSource>(this IQueryable<TSource> source, string sortName, bool sortDescending)
        {
            if (string.IsNullOrEmpty(sortName)) sortName = "";

            return source.OrderBy(sortName + (sortDescending ? " desc" : " asc"));
        }

        #endregion
        #region Where

        public static IQueryable<TSource> WhereAct<TSource>(this IQueryable<TSource> source, List<FilterCriteria> filterArr = null, string valueFilter = "", string fieldFilter = "Name", FilterOperator opFilter = FilterOperator.Eq)
        {
            if (filterArr != null)
            {
                string filterFormat = FilterDefinition.GetFilterExpression(filterArr);
                if (string.IsNullOrEmpty(filterFormat))
                    return source;
                else

                    return source.Where(filterFormat);
            }
            else if (!string.IsNullOrEmpty(valueFilter))
            {
                string filterFormat = FilterDefinition.GetFilterExpression(
                                           new FilterCriteria { Field = fieldFilter, Op = opFilter, Value = valueFilter });

                if (string.IsNullOrEmpty(filterFormat))
                    return source;
                else
                    return source.Where(filterFormat);
            }
            else
                return source;
        }

        #endregion
    }
}
