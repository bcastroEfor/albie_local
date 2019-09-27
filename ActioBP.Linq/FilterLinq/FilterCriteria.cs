namespace ActioBP.Linq.FilterLinq
{
    public class FilterCriteria
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public FilterOperator Op { get; set; }
        public FilterDBType Format { get; set; }
    } 
}
