using ActioBP.Linq.FilterLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActioBP.Linq.Nav.Datatable
{
    public class NavConverter<TFilter, TField>
                                            where TFilter: class
                                            where TField : struct, IConvertible
    {
        public List<TFilter> BuildNavFilter(List<FilterCriteria> filter = null)
        {
            var tfield = typeof(TField);
            if (!tfield.IsEnum)
            {
                throw new ArgumentException("TField must be an enumerated type");
            }

            if (filter == null) return null;

            var filterNav = new List<NavFilter<TField>>();
            foreach (var f in filter)
            {
                var c = BuildCondition(f);
                if (c != null) filterNav.Add(c);
            }
            return ConvertToFilter(filterNav);
        }

        public static List<TFilter> ConvertToFilter(IList<NavFilter<TField>> filter)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<TFilter>>(
                Newtonsoft.Json.JsonConvert.SerializeObject(filter));
        }

        public static TFilter ConvertToFilter(NavFilter<TField> filter)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TFilter>(
                Newtonsoft.Json.JsonConvert.SerializeObject(filter));
        }

        public NavFilter<TField> BuildCondition(FilterCriteria filter)
        {
            if (filter == null) return null;
            try
            {
                return new NavFilter<TField>
                {
                    Field = (TField)Enum.Parse(typeof(TField), filter.Field),
                    Criteria = BuildCriteriaCondition(filter)
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string BuildCriteriaCondition(FilterCriteria filter)
        {
            return BuildCriteriaCondition(filter.Op,filter.Format, filter.Value);
        }
        public string BuildCriteriaCondition(FilterOperator op,FilterDBType format, string value)
        {
            string criteria = string.Empty;
            switch (format)
            {
                case FilterDBType.Literal:
                case FilterDBType.Guid:
                case FilterDBType.String:
                default:
                                        return BuildCriteriaConditionString(op,value);
                case FilterDBType.DateTime: return BuildCriteriaConditionDatetime(op,value);
            }
        }
        public string BuildCriteriaConditionDatetime(FilterOperator op, string value)
        {
            string criteria = string.Empty;
            switch (op)
            {
                // necesario formatear para fechas
                //TODO
                case FilterOperator.Eq:
                default: return value;
                case FilterOperator.Ge: return string.Format(">={0}", value);
                case FilterOperator.Gt: return string.Format("{0}..", value);
                case FilterOperator.Le: return string.Format("..{0}", value);
                case FilterOperator.Lt: return string.Format("<={0}", value);
                    //case FilterOperator.Nu: 
                    //case FilterOperator.Nn: 
            }
        }
        public string BuildCriteriaConditionString(FilterOperator op, string value)
        {
            string criteria = string.Empty;

            switch (op)
            {
                case FilterOperator.Eq:
                default: return value;
                case FilterOperator.Ne: return string.Format("<>{0}", value);
                case FilterOperator.Cn: return string.Format("*{0}*", value);
                case FilterOperator.Nc: return string.Format("<>*{0}*", value);
                case FilterOperator.Bw: return string.Format("{0}*", value);
                case FilterOperator.Bn: return string.Format("<>{0}*", value);
                case FilterOperator.Ew: return string.Format("*{0}", value);
                case FilterOperator.En: return string.Format("<>*{0}", value);
                //case FilterOperator.In: 
                //case FilterOperator.Ni:
                case FilterOperator.Ge: return string.Format(">={0}", value);
                case FilterOperator.Gt: return string.Format(">{0}", value);
                case FilterOperator.Le: return string.Format("<{0}", value);
                case FilterOperator.Lt: return string.Format("<={0}", value);
                    //case FilterOperator.Nu: 
                    //case FilterOperator.Nn: 
            }
        }
    }   
}
