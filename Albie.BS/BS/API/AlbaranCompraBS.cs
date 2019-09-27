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
    public class AlbaranCompraBS : EntityBS, IAlbaranCompraBS
    {
        private readonly IConfiguration _conf;
        private readonly IProviderRateBS pBS;
        private readonly ILineBS iBS;
        private readonly IAlbaranLineaBS aBS;
        private readonly IProductBS prBS;
        private readonly IHistoricoPedidoBS hBS;
        private readonly IHeaderOrderBS hoBS;
        public AlbaranCompraBS(RepoDB db,
            ILineBS line,
            IProductBS product,
            IProviderRateBS providerRate,
            IAlbaranLineaBS albaranLinea,
            IHistoricoPedidoBS historicoPedido,
            IHeaderOrderBS headerOrder,
            IConfiguration conf) : base(db)
        {
            _conf = conf;
            iBS = line;
            prBS = product;
            pBS = providerRate;
            aBS = albaranLinea;
            hBS = historicoPedido;
            hoBS = headerOrder;
        }

        #region GET
        public CollectionList<AlbaranCompra> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetAlbaranComprasCount(filter, filterArr);

            if (total == 0) return new CollectionList<AlbaranCompra>();

            var items = GetAlbaranComprasList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<AlbaranCompra>
            {
                Items = items,
                Total = total
            };
        }

        public CollectionList<AlbaranCompra> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {

            var total = GetAlbaranComprasCount(filter, filterArr, readingDate, filterReadingDate);

            if (total == 0) return new CollectionList<AlbaranCompra>();

            var items = GetAlbaranComprasList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, readingDate, filterReadingDate);

            return new CollectionList<AlbaranCompra>
            {
                Items = items,
                Total = total
            };
        }

        public int GetAlbaranComprasCount(string filter = "", List<FilterCriteria> filterArr = null, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<AlbaranCompra> lista = db.AlbaranCompras
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn);
            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            return lista.Count();
        }

        public IEnumerable<AlbaranCompra> GetAlbaranComprasList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<AlbaranCompra> lista = db.AlbaranCompras
                                           .Include(o => o.AlbaranLineas)
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);
            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public IQueryable<AlbaranCompra> FilterReadingDate(IQueryable<AlbaranCompra> AlbaranComprass, DateTimeOffset? readingDate, string readingDateFilter)
        {
            if (readingDateFilter == "<") return AlbaranComprass = AlbaranComprass.Where(o => o.ReadingDate < readingDate);
            else if (readingDateFilter == "<=") return AlbaranComprass = AlbaranComprass.Where(o => o.ReadingDate <= readingDate);
            else if (readingDateFilter == "=") return AlbaranComprass = AlbaranComprass.Where(o => o.ReadingDate == readingDate);
            else if (readingDateFilter == ">") return AlbaranComprass = AlbaranComprass.Where(o => o.ReadingDate > readingDate);
            else if (readingDateFilter == ">=") return AlbaranComprass = AlbaranComprass.Where(o => o.ReadingDate >= readingDate);
            return AlbaranComprass = AlbaranComprass.Where(o => o.ReadingDate != null);
        }

        public AlbaranCompra Get(string id)
        {
            return db.AlbaranCompras
                .SingleOrDefault(o => o.No == id);
        }
        #endregion

        #region POST
        public ResultAndError<AlbaranCompra> Add(AlbaranCompra c)
        {
            ResultAndError<AlbaranCompra> result = new ResultAndError<AlbaranCompra>();
            try
            {
                db.AlbaranCompras.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<AlbaranCompra> Update(AlbaranCompra cr, bool insertIfNoExists = false)
        {
            ResultAndError<AlbaranCompra> result = new ResultAndError<AlbaranCompra>();
            try
            {
                AlbaranCompra old = Get(cr.No);
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

        public ResultAndError<bool> UpdateReadingDate(IEnumerable<string> centersNo, DateTimeOffset readingDate)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                foreach (string no in centersNo)
                {
                    AlbaranCompra oAlbaranCompras = Get(no);
                    oAlbaranCompras.ReadingDate = readingDate;
                    db.SaveChanges();
                }
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<AlbaranCompra> oAlbaranCompras, bool insertIfNoExists = false)
        {
            foreach (AlbaranCompra albaran in oAlbaranCompras)
            {
                AlbaranCompra old = Get(albaran.No);
                if (old == null && insertIfNoExists) Add(albaran);
                else db.Entry(old).CurrentValues.SetValues(albaran);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                AlbaranCompra AlbaranCompras = Get(id);
                if (AlbaranCompras == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.AlbaranCompras.Remove(AlbaranCompras);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> AlbaranCompras)
        {
            List<AlbaranCompra> oAlbaran = new List<AlbaranCompra>();
            foreach (string AlbaranComprasNo in AlbaranCompras)
            {
                AlbaranCompra oAlbaranCompras = Get(AlbaranComprasNo);
                if (oAlbaranCompras != null) oAlbaran.Add(oAlbaranCompras);
            }
            db.AlbaranCompras.RemoveRange(oAlbaran);
            db.SaveChanges();
            return true;
        }

        public void RecepcionMercancia(Document oOrder, DateTimeOffset albaranDate, bool nonConform)
        {
            //Comprobamos los precios de los productos con la fecha del albaran
            UpdateProductPrice(oOrder.Lines, oOrder.BuyFromVendorNo, albaranDate);

            //TODO Cuando el usuario valide la información, el sistema comprobará que existe el número de albarán y la fecha del albarán en la cabecera del pedido o lo solicitará 
            AlbaranCompra oAlbaran = new AlbaranCompra()
            {
                Amount = oOrder.Amount,
                BuyFromAddress = oOrder.BuyFromAddress,
                BuyFromAddress2 = oOrder.BuyFromAddress2,
                BuyFromCity = oOrder.BuyFromCity,
                BuyFromContact = oOrder.BuyFromContact,
                BuyFromCounty = oOrder.BuyFromCounty,
                BuyFromPostCode = oOrder.BuyFromPostCode,
                BuyFromVendorName = oOrder.BuyFromVendorName,
                BuyFromVendorName2 = oOrder.BuyFromVendorName2,
                BuyFromVendorNo = oOrder.BuyFromVendorNo,
                OrderDate = oOrder.OrderDate,
                PostingDate = albaranDate,
                ShortcutDimension1Code = oOrder.Centro,
                ShortcutDimension2Code = oOrder.Zona,
                VendorShipmentNo = oOrder.VendorShipmentNo,
                OrderNo = oOrder.No.ToString(),
                No = "A" + oOrder.No,//añadir numerico
                NonConform = nonConform
            };
            //Creamos el albaran compra
            Update(oAlbaran, true);
            //Actualizamos la cantidad recibida de las lineas
            decimal? totalQuantity = 0;
            foreach (Line line in oOrder.Lines)
            {
                line.OutstandingQuantity -= line.QuantityReceived;
                line.Product.StockActual = line.OutstandingQuantity;
                totalQuantity += line.OutstandingQuantity;

                //Añadimos el stock al producto
                prBS.Update(line.Product);
                AlbaranLinea oAlbaranLinea = new AlbaranLinea()
                {
                    AlbaranCompraNo = oAlbaran.No,
                    Amount = line.Amount,
                    AmountIncludingVAT = line.AmountIncludingVAT,
                    Description = line.Description,
                    DirectUnitCost = line.DirectUnitCost,
                    ExpectedReceiptdate = line.ExpectedReceiptDate,
                    LineDiscount = line.LineDiscount,
                    OrderLineNo = line.LineNo.ToString(),
                    OrderNo = line.DocumentNo.ToString(),
                    ProductNo = line.No,
                    Quantity = line.Quantity,
                    QuantityReceived = line.QuantityReceived,
                    UnitOfMeasure = line.UnitOfMeasure,
                    VAT = line.VAT,
                    //Comprobamos que la cantidad recibida es mayor o inferior a la pedida
                    ExcessReception = CalculateExcessReception(line)
                };
                //POnemos a 0 la cantidad recibida de la linea
                line.QuantityReceived = 0;
                //Actualizamos la linea
                ResultAndError<Line> oLine = iBS.Update(line);
                //Creamos el albaranlinea
                ResultAndError<AlbaranLinea> albaranLinea = aBS.Update(oAlbaranLinea, true);
            }
            //TODO Si la cantidad pendiente de todas las líneas del pedido no es cero después de crear el albarán, el sistema dejará el pedido como pendiente, y en el caso de que no vaya a recibirla, el usuario podrá cerrar el pedido, llevándolo entonces al histórico de pedidos.
            if (totalQuantity == 0)
            {
                //Si el total es 0 mandamos el pedido a historico
                hBS.CloseOrder(oOrder);
            }
            else
            {
                //Si no es 0 dejamos el pedido como pendiente y dejamos que el usuario decida si cerrarlo o no
                oOrder.Estado = 0;
                hoBS.Update(oOrder);
            }
        }

        public IEnumerable<Line> UpdateProductPrice(IEnumerable<Line> products, string providerNo, DateTimeOffset date)
        {
            foreach (Line product in products)
            {
                ProviderRate oRate = pBS.Get(product.No, providerNo);
                //TODO falta comprobar por fecha
                if (product.DirectUnitCost != oRate.DirectUnitCost) product.DirectUnitCost = oRate.DirectUnitCost;
            }
            return products;
        }

        public bool CalculateExcessReception(Line oLinea)
        {
            decimal? receptionPer = (oLinea.Product.ReceptionMAxPct / 100) + 1;
            if (oLinea.QuantityReceived > Math.Round((oLinea.Quantity * receptionPer).Value)) return true;
            else return false;
        }

        public void AnularAlbaran(AlbaranCompra albaran)
        {
            //Actualizamos el albaran para ponerlo anulado
            albaran.Anulado = true;
            Update(albaran);
            //Obtenemos el pedido mediante OrderNo
            Document oPedido = hoBS.Get(Convert.ToInt32(albaran.OrderNo));
            //Establecemos el pedido a cerrado
            oPedido.Estado = 0; //Preguntar que estado seria
            foreach (Line line in oPedido.Lines)
            {
                foreach (AlbaranLinea albaranLinea in albaran.AlbaranLineas)
                {
                    if (line.LineNo.ToString() == albaranLinea.OrderLineNo)
                    {
                        //sumamos la cantidad recibida a la cantidad pendiente
                        line.OutstandingQuantity += albaranLinea.QuantityReceived;
                    }
                }
                iBS.Update(line);
            }
        }
        #endregion
    }
}
