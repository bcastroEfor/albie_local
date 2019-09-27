using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Albie.Repository.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Albie.BS
{
    public class CrossReferenceBS : EntityBS, ICrossReferenceBS
    {
        private readonly IConfiguration _conf;
        public CrossReferenceBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<CrossReference> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetCrossReferenceCount(filter, filterArr);

            if (total == 0) return new CollectionList<CrossReference>();

            var items = GetCrossReferenceList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<CrossReference>
            {
                Items = items,
                Total = total
            };
        }

        public int GetCrossReferenceCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<CrossReference> lista = db.CrossReferences
                                           .WhereAct(filterArr, filter, fieldFilter: "no", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<CrossReference> GetCrossReferenceList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<CrossReference> lista = db.CrossReferences
                                           .WhereAct(filterArr, filter, fieldFilter: "No", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public CrossReference Get(string id)
        {
            return db.CrossReferences.SingleOrDefault(o => o.ItemNo == id);
        }

        public CrossReference Get(string itemNo, string unitOfMeasure, string typeNo, string no)
        {
            return db.CrossReferences.SingleOrDefault(o => o.ItemNo == itemNo 
                                                     && o.UnitOfMeasure == unitOfMeasure 
                                                     && o.CrossReferenceTypeNo == typeNo 
                                                     && o.CrossReferenceNo == no);
        }
        #endregion

        #region POST
        public ResultAndError<CrossReference> Add(CrossReference c)
        {
            ResultAndError<CrossReference> result = new ResultAndError<CrossReference>();
            try
            {
                db.CrossReferences.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<CrossReference> Update(CrossReference cr, bool insertIfNoExists = false)
        {
            ResultAndError<CrossReference> result = new ResultAndError<CrossReference>();
            try
            {
                CrossReference old = Get(cr.ItemNo);
                if (old == null && insertIfNoExists) return Add(cr);
                db.Entry(old).CurrentValues.SetValues(cr);
                db.SaveChanges();
                return result.AddResult(cr);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<CrossReference> oCrossReferences, bool insertIfNoExists = false)
        {
            foreach (CrossReference CrossReference in oCrossReferences)
            {
                CrossReference old = Get(CrossReference.ItemNo);
                if (old == null && insertIfNoExists) Add(CrossReference);
                else db.Entry(old).CurrentValues.SetValues(CrossReference);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                CrossReference CrossReference = Get(id);
                if (CrossReference == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.CrossReferences.Remove(CrossReference);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> CrossReferences)
        {
            List<CrossReference> oCrossReferences = new List<CrossReference>();
            foreach (string CrossReferenceNo in CrossReferences)
            {
                CrossReference oCrossReference = Get(CrossReferenceNo);
                if (oCrossReference != null) oCrossReferences.Add(oCrossReference);
            }
            db.CrossReferences.RemoveRange(oCrossReferences);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
