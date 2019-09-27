using ActioBP.Linq.FilterLinq;
using Albie.Models;
using System.Collections.Generic;

namespace Albie.BS.Interfaces
{
    public interface ICustomerRateBS : IEntityAlbieBS<CustomerRate>
    {
        IEnumerable<CustomerRate> GetCustomerRateList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "product.description", bool sortDescending = false);
        
    }
}