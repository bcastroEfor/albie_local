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
    public class LineBS : EntityBS, ILineBS
    {
        private readonly IConfiguration _conf;
        public LineBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<Line> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetCentroCount(filter, filterArr);

            if (total == 0) return new CollectionList<Line>();

            var items = GetCentroList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<Line>
            {
                Items = items,
                Total = total
            };
        }

        public CollectionList<Line> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {

            var total = GetCentroCount(filter, filterArr, readingDate, filterReadingDate);

            if (total == 0) return new CollectionList<Line>();

            var items = GetCentroList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, readingDate, filterReadingDate);

            return new CollectionList<Line>
            {
                Items = items,
                Total = total
            };
        }

        public int GetCentroCount(string filter = "", List<FilterCriteria> filterArr = null, DateTimeOffset? readingDate = null, string filterReadingDate = "", string fieldFilter = nameof(Line.Description))
        {
            IQueryable<Line> lista = db.LineRequests
                                           .WhereAct(filterArr, filter, fieldFilter: fieldFilter, opFilter: FilterOperator.Cn);

            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            return lista.Count();
        }

        public IEnumerable<Line> GetCentroList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "", string fieldFilter = nameof(Line.Description))
        {
            IQueryable<Line> lista = db.LineRequests
                                           .WhereAct(filterArr, filter, fieldFilter: fieldFilter, opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public IQueryable<Line> FilterReadingDate(IQueryable<Line> lines, DateTimeOffset? readingDate, string readingDateFilter)
        {
            if (readingDateFilter == "<") return lines = lines.Where(o => o.ReadingDate < readingDate);
            else if (readingDateFilter == "<=") return lines = lines.Where(o => o.ReadingDate <= readingDate);
            else if (readingDateFilter == "=") return lines = lines.Where(o => o.ReadingDate == readingDate);
            else if (readingDateFilter == ">") return lines = lines.Where(o => o.ReadingDate > readingDate);
            else if (readingDateFilter == ">=") return lines = lines.Where(o => o.ReadingDate >= readingDate);
            return lines = lines.Where(o => o.ReadingDate != null);
        }

        public Line Get(int no)
        {
            return db.LineRequests.SingleOrDefault(o => o.LineNo == no);
        }
        #endregion

        #region POST
        public ResultAndError<Line> Add(Line c)
        {
            ResultAndError<Line> result = new ResultAndError<Line>();
            try
            {
                db.LineRequests.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<Line> Update(Line cr, bool insertIfNoExists = false)
        {
            ResultAndError<Line> result = new ResultAndError<Line>();
            try
            {
                Line old = Get(cr.LineNo);
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

        public ResultAndError<bool> UpdateReadingDate(IEnumerable<int> linesNo, DateTimeOffset readingDate)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                foreach (int no in linesNo)
                {
                    Line oLine = Get(no);
                    oLine.ReadingDate = readingDate;
                    db.SaveChanges();
                }
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }
        #endregion
    }
}
