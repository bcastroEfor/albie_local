using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActioBP.Linq.FilterLinq
{
    public class FilterDefinition
    {

        public static string GetFilterExpression(List<FilterCriteria> filters, FilterConditionJoin conditionJoin=FilterConditionJoin.And)
        {
            string filterExpression = String.Empty;

            if (filters != null)
            {
                StringBuilder filterExpressionBuilder = new StringBuilder();
                string conditionJoinString = conditionJoin.ToString();
                foreach (FilterCriteria f in filters.Where(p=>!string.IsNullOrEmpty(p.Value)))
                {
                    filterExpressionBuilder.Append(GetFilterExpression(f));
                    filterExpressionBuilder.Append(String.Format(" {0} ", conditionJoinString));
                }
                if (filterExpressionBuilder.Length > 0)
                    filterExpressionBuilder.Remove(filterExpressionBuilder.Length - conditionJoinString.Length - 2, conditionJoinString.Length + 2);
                filterExpression = filterExpressionBuilder.ToString();
            }
            return filterExpression;
        }

        public static string GetFilterExpression(FilterCriteria filter)
        {
            return GetFilterLinq(filter.Field, filter.Op, filter.Value, filter.Format);
        }


        private static string GetFilterLinq(string searchingName, FilterOperator searchingOperator, string searchingValue, FilterDBType format)
        {
            if (string.IsNullOrEmpty(searchingValue)) return string.Empty;

            string searchingOperatorString = String.Empty;

            if (format == FilterDBType.DateTime)
            {
                var valueDate = DateTime.ParseExact(searchingValue, "yyyy-MM-ddTHH:mm:ss.fffZ", null);
                var valueDateTime = valueDate.TimeOfDay;
                var dayBegin = valueDate.AddHours(-valueDateTime.Hours).AddMinutes(-valueDateTime.Minutes).AddSeconds(-valueDateTime.Seconds).AddMilliseconds(-valueDateTime.Milliseconds);
                var nextDayBegin = dayBegin.AddDays(1);
                switch (searchingOperator)
                {
                    case FilterOperator.Eq:
                        // ==
                        return String.Format("({0} {1} \"{2:MM/dd/yyyy}\" && {3} {4} \"{5:MM/dd/yyyy}\")", searchingName, ">=", dayBegin, searchingName, "<", nextDayBegin);
                    case FilterOperator.Ne:
                        // !=
                        return String.Format("({0} {1} \"{2:MM/dd/yyyy}\" && {3} {4} \"{5:MM/dd/yyyy}\")", searchingName, "<", dayBegin, searchingName, ">=", nextDayBegin);
                    case FilterOperator.Lt:
                        // <
                        return String.Format("{0} {1} \"{2:MM/dd/yyyy}\"", searchingName, "<", dayBegin);
                    case FilterOperator.Le:
                        // <=
                        return String.Format("{0} {1} \"{2:MM/dd/yyyy}\"", searchingName, "<=", dayBegin);
                    case FilterOperator.Gt:
                        // >
                        return String.Format("{0} {1} \"{2:MM/dd/yyyy}\"", searchingName, ">", nextDayBegin);
                    case FilterOperator.Ge:
                        // >=
                        return String.Format("{0} {1} \"{2:MM/dd/yyyy}\"", searchingName, ">=", dayBegin);
                    case FilterOperator.Nu: return String.Format("{0} is null", searchingName);
                    case FilterOperator.Nn: return String.Format("{0} is not null", searchingName);
                    default:
                        return String.Format("{0} {1} \"{2:MM/dd/yyyy}\"", searchingName, "=", valueDate);
                }
            }else{
                //FORMAT DATA. DEPENDING ON TYPE OF DATA
                if ((searchingOperator == FilterOperator.Eq || searchingOperator == FilterOperator.Ne)
                    && format == FilterDBType.String)
                {
                    searchingValue = string.Format("\"{0}\"", searchingValue);
                }
                //FORMAT DATA. DEPENDING ON TYPE OF DATA
                if (format == FilterDBType.Guid)
                {
                    var incomingIds = searchingValue.Split(',');
                    searchingValue = String.Join(",", incomingIds.Select(x => "\"" + Guid.Parse(x).ToString() + "\""));
                }
                switch (searchingOperator)
                {
                    case FilterOperator.Eq: return String.Format("{0} {1} {2}", searchingName, "=", searchingValue);
                    case FilterOperator.Ne: return String.Format("{0} {1} {2}", searchingName, "!=", searchingValue);
                    //case FilterOperator.EqText: return String.Format("{0} {1} \"{2}\"", searchingName, "=", searchingValue);
                    //case FilterOperator.NeText: return String.Format("{0} {1} \"{2}\"", searchingName, "!=", searchingValue);
                    case FilterOperator.Lt: return String.Format("{0} {1} {2}", searchingName, "<", searchingValue);
                    case FilterOperator.Le: return String.Format("{0} {1} {2}", searchingName, "<=", searchingValue);
                    case FilterOperator.Gt: return String.Format("{0} {1} {2}", searchingName, ">", searchingValue);
                    case FilterOperator.Ge: return String.Format("{0} {1} {2}", searchingName, ">=", searchingValue);
                    case FilterOperator.Bw: return String.Format("{0}.StartsWith(\"{1}\")", searchingName, searchingValue);
                    case FilterOperator.Bn: return String.Format("!{0}.StartsWith(\"{1}\")", searchingName, searchingValue);
                    case FilterOperator.Ew: return String.Format("{0}.EndsWith(\"{1}\")", searchingName, searchingValue);
                    case FilterOperator.En: return String.Format("!{0}.EndsWith(\"{1}\")", searchingName, searchingValue);
                    case FilterOperator.Cn: return String.Format("{0}.Contains(\"{1}\")", searchingName, searchingValue);
                    case FilterOperator.Nc: return String.Format("!{0}.Contains(\"{1}\")", searchingName, searchingValue);

                    //case FilterOperator.EqualOrNotEqual: return String.Format("!{0}.Contains(\"{1}\")", searchingName, searchingValue);
                    case FilterOperator.In: return String.Format("({0} {1} ({2}))", searchingName, "in", searchingValue);
                    case FilterOperator.Ni: return String.Format("({0} {1} ({2}))", searchingName, "not in", searchingValue);
                    case FilterOperator.Nu: return String.Format("({0} is null)", searchingName);
                    case FilterOperator.Nn: return String.Format("({0} is not null)", searchingName);
                    //case FilterOperator.TextOperators: return String.Format("!{0}.Contains(\"{1}\")", searchingName, searchingValue);
                    //case FilterOperator.NoTextOperators: return String.Format("!{0}.Contains(\"{1}\")", searchingName, searchingValue);
                    //case FilterOperator.NullOperators: return String.Format("!{0}.Contains(\"{1}\")", searchingName, searchingValue);

                    default: return String.Format("{0} {1} \"{2}\"", searchingName, "=", searchingValue);
                }
            }
        }

        private static string GetFilter(string searchingName, FilterOperator searchingOperator, string searchingValue)
        {
            return GetFilter(searchingName, searchingOperator.ToString().ToLower(), searchingValue);
        }

        private static string GetFilter(string searchField, string searchOper, string searchValue)
        {
            string querystring = string.Empty;
            switch (searchOper)
            {   //equal
                case "eq": querystring = String.Format("{0} = {1}", searchField, searchValue); break;
                //not equal
                case "ne": querystring = String.Format("{0} != {1}", searchField, searchValue); break;
                //less than
                case "lt": querystring = String.Format("{0} < {1}", searchField, searchValue); break;
                //less or equal
                case "le": querystring = String.Format("{0} <= {1}", searchField, searchValue); break;
                //greater or equal
                case "gt": querystring = String.Format("{0} > {1}", searchField, searchValue); break;
                //greater or equal
                case "ge": querystring = String.Format("{0} >= {1}", searchField, searchValue); break;
                // begins with
                case "bw": querystring = String.Format("{0} like '{1}%'", searchField, searchValue); break;
                //does not begin with
                case "bn": querystring = String.Format("{0} not like '{1}%'", searchField, searchValue); break;
                //is in
                case "in": querystring = String.Format("{0} in ({1})", searchField, searchValue); break;
                //is not in
                case "ni": querystring = String.Format("{0} not in ({1})", searchField, searchValue); break;
                //ends with
                case "ew": querystring = String.Format("{0} like '%{1}'", searchField, searchValue); break;
                //does not end with
                case "en": querystring = String.Format("{0} not like '%{1}'", searchField, searchValue); break;
                //contains
                case "cn": querystring = String.Format("{0} like '%{1}%'", searchField, searchValue); break;
                //does not contains
                case "nc": querystring = String.Format("{0} not like '%{1}%'", searchField, searchValue); break;
            }
            if (querystring.Length > 0) return querystring; //Para poder asignar el valor del Campo
            else return String.Empty;
        }
        
    }
}
