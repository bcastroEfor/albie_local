using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Albie.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Albie.BS
{
    public class HistoricoPedidoBS : EntityBS, IHistoricoPedidoBS
    {
        private readonly IConfiguration _conf;
        public HistoricoPedidoBS(RepoDB db,
            IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<HistoricoPedido> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetHistoricoPedidosCount(filter, filterArr);

            if (total == 0) return new CollectionList<HistoricoPedido>();

            var items = GetHistoricoPedidosList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<HistoricoPedido>
            {
                Items = items,
                Total = total
            };
        }


        public int GetHistoricoPedidosCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<HistoricoPedido> lista = db.HistoricoPedidos
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<HistoricoPedido> GetHistoricoPedidosList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<HistoricoPedido> lista = db.HistoricoPedidos
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }


        public HistoricoPedido Get(int id)
        {
            return db.HistoricoPedidos
                .SingleOrDefault(o => o.No == id);
        }

        public HistoricoPedido GetHistoricoFromOrder(Document order)
        {
            return new HistoricoPedido()
            {
                No = order.No,
                OrderDate = order.OrderDate,
                ExpectedReceiptDate = order.ExpectedReceiptDate ?? DateTimeOffset.MinValue,
                Centro = order.Centro,
                Zona = order.Zona,
                BuyFromVendorName = order.BuyFromVendorName ?? "",
                BuyFromVendorName2 = order.BuyFromVendorName2 ?? "",
                BuyFromAddress = order.BuyFromAddress ?? "",
                BuyFromAddress2 = order.BuyFromAddress2 ?? "",
                BuyFromPostCode = order.BuyFromPostCode ?? "",
                BuyFromCounty = order.BuyFromCounty ?? "",
                BuyFromCity = order.BuyFromCity ?? "",
                BuyFromVendorNo = order.BuyFromVendorNo ?? "",
                BuyFromContact = order.BuyFromContact ?? "",
                Amount = order.Amount,
                VendorOrderNo = order.VendorOrderNo ?? "",
                PostingDate = order.PostingDate ?? DateTimeOffset.MinValue,
                VendorShipmentNo = order.VendorShipmentNo ?? "",
                Estado = order.Estado,
                OrigenPedido = order.OrigenPedido ?? "",
                AmountIncludingVAT = order.AmountIncludingVAT ?? 0,
                ReadingDate = order.ReadingDate ?? DateTimeOffset.MinValue
            };
        }
        #endregion

        #region POST
        public ResultAndError<HistoricoPedido> Add(HistoricoPedido c)
        {
            ResultAndError<HistoricoPedido> result = new ResultAndError<HistoricoPedido>();
            try
            {
                db.HistoricoPedidos.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<HistoricoPedido> Update(HistoricoPedido cr, bool insertIfNoExists = false)
        {
            ResultAndError<HistoricoPedido> result = new ResultAndError<HistoricoPedido>();
            try
            {
                HistoricoPedido old = Get(cr.No);
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

        public bool UpdateMulti(IEnumerable<HistoricoPedido> oHistoricoPedidos, bool insertIfNoExists = false)
        {
            foreach (HistoricoPedido albaran in oHistoricoPedidos)
            {
                HistoricoPedido old = Get(albaran.No);
                if (old == null && insertIfNoExists) Add(albaran);
                else db.Entry(old).CurrentValues.SetValues(albaran);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(int id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                HistoricoPedido HistoricoPedidos = Get(id);
                if (HistoricoPedidos == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.HistoricoPedidos.Remove(HistoricoPedidos);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<int> HistoricoPedidos)
        {
            List<HistoricoPedido> oAlbaran = new List<HistoricoPedido>();
            foreach (int HistoricoPedidosNo in HistoricoPedidos)
            {
                HistoricoPedido oHistoricoPedidos = Get(HistoricoPedidosNo);
                if (oHistoricoPedidos != null) oAlbaran.Add(oHistoricoPedidos);
            }
            db.HistoricoPedidos.RemoveRange(oAlbaran);
            db.SaveChanges();
            return true;
        }

        public ResultAndError<HistoricoPedido> CloseOrder(Document order)
        {
            ResultAndError<HistoricoPedido> result = new ResultAndError<HistoricoPedido>();
            try
            {
                HistoricoPedido oHistorico = GetHistoricoFromOrder(order);
                return result = Update(oHistorico, true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }
        #endregion
    }
}
