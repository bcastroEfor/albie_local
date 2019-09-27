using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.Models;
using System.Collections.Generic;

namespace Albie.BS.Interfaces
{
    public interface IDimensionBS
    {
        IEnumerable<Dimension> GetDimensionList(string dimensionCode = "", string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
        Dimension Get(string code, string dimensionCode);
        ResultAndError<Dimension> Add(Dimension c);
        ResultAndError<Dimension> Update(Dimension cr, bool insertIfNoExists = false);
        bool UpdateMulti(IEnumerable<Dimension> oDimensions, bool insertIfNoExists = false);
        ResultAndError<bool> Delete(string code, string dimensionCode);
    }
}
