using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using System.Collections.Generic;

namespace Albie.BS.Interfaces
{
    public interface IEntityAlbieBS<TEntity> : IEntityAlbieBS<TEntity, string>
    {
        
    }
    public interface IEntityAlbieBS<TEntity, TKey>
       // where TEntity: Albie.Models.EntityBase
    {
        CollectionList<TEntity> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
        TEntity Get(TKey id);
        ResultAndError<TEntity> Add(TEntity m);
        ResultAndError<TEntity> Update(TEntity m, bool insertIfNoExist = false);
        ResultAndError<bool> Delete(TKey id);
        bool UpdateMulti(IEnumerable<TEntity> m, bool insertIfNoExists = false);
        bool DeleteMulti(IEnumerable<TKey> m);
    }
}
