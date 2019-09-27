using Albie.Repository.Data;

namespace Albie.BS
{
    public class EntityBS
    {
        protected RepoDB db = null;

        #region constructor

        public EntityBS(RepoDB dbBBDD)
        {
            if (db == null) this.db = dbBBDD;
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
        #endregion

    }
}