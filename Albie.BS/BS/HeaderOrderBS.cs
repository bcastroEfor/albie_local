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
    public class HeaderOrderBS : EntityBS, IHeaderOrderBS
    {
        private readonly IConfiguration _conf;
        private readonly IProviderBS pBS;
        public HeaderOrderBS( IProviderBS provider, RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
            pBS = provider;
        }

        #region GET
        public CollectionList<Document> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetCentroCount(filter, filterArr);

            if (total == 0) return new CollectionList<Document>();

            var items = GetCentroList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<Document>
            {
                Items = items,
                Total = total
            };
        }

        public CollectionList<Document> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {

            var total = GetCentroCount(filter, filterArr, readingDate, filterReadingDate);

            if (total == 0) return new CollectionList<Document>();

            var items = GetCentroList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, readingDate, filterReadingDate);

            return new CollectionList<Document>
            {
                Items = items,
                Total = total
            };
        }

        public int GetCentroCount(string filter = "", List<FilterCriteria> filterArr = null, DateTimeOffset? readingDate = null, string filterReadingDate = "", string fieldFilter = nameof(Document.BuyFromVendorName))
        {
            IQueryable<Document> lista = db.HeaderRequests
                                           .WhereAct(filterArr, filter, fieldFilter: fieldFilter, opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<Document> GetCentroList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "", string fieldFilter = nameof(Document.BuyFromVendorName))
        {
            IQueryable<Document> lista = db.HeaderRequests
                                           .Include(o => o.Provider).Include(o => o.Zonas).Include(o => o.Centros)
                                           .Include(o => o.Lines).ThenInclude(o => o.Product).ThenInclude(o => o.ProviderRates)
                                           .WhereAct(filterArr, filter, fieldFilter: fieldFilter, opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public IQueryable<Document> FilterReadingDate(IQueryable<Document> orders, DateTimeOffset? readingDate, string readingDateFilter)
        {
            if (readingDateFilter == "<") return orders = orders.Where(o => o.ReadingDate < readingDate);
            else if (readingDateFilter == "<=") return orders = orders.Where(o => o.ReadingDate <= readingDate);
            else if (readingDateFilter == "=") return orders = orders.Where(o => o.ReadingDate == readingDate);
            else if (readingDateFilter == ">") return orders = orders.Where(o => o.ReadingDate > readingDate);
            else if (readingDateFilter == ">=") return orders = orders.Where(o => o.ReadingDate >= readingDate);
            return orders = orders.Where(o => o.ReadingDate != null);
        }

        public Document Get(int id)
        {
            return db.HeaderRequests
                     .Include(o => o.Provider)
                     .Include(o => o.Zonas)
                     .Include(o => o.Centros)
                     .Include(o => o.Lines)
                     .SingleOrDefault(o => o.No == id);
        }
        #endregion

        #region POST
        public ResultAndError<bool> CreateOrderByProvider(CartList listado)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                var grupo = listado.ProductList.GroupBy(o => o.ProviderId).Select(o => new { ProviderId = o.Key, Items = o.ToList() });

                foreach (var providerList in grupo)
                {
                    //Obtenemos el proveedor
                    Provider oProvider = pBS.Get(providerList.ProviderId);
                    //Generamos un objeto de Cabecera pedido
                    Document oCabecera = new Document
                    {
                        BuyFromVendorNo = oProvider.VendorNo,
                        BuyFromVendorName = oProvider.Name,
                        BuyFromVendorName2 = oProvider.Name2,
                        BuyFromAddress = oProvider.Address,
                        BuyFromAddress2 = oProvider.Address2,
                        BuyFromCity = oProvider.City,
                        BuyFromContact = oProvider.Contact,
                        BuyFromCounty = oProvider.County,
                        BuyFromPostCode = oProvider.PostCode,
                        OrderDate = DateTime.Now,
                        Estado = 1,//Confirmado
                        OrigenPedido = "WEB",
                        Amount = 0,
                        Centro = "",//Se tiene que rellenar con el centro seleccionado TODO
                        Zona = ""//Se rellena con la zona asociada al centro TODO

                    };
                    //Creamos la cabecera
                    ResultAndError<Document> cabecera = Update(oCabecera, true);
                    if (cabecera.HasErrors) return result.AddError(cabecera.Errors, HttpStatusCode.InternalServerError);
                    decimal? totalAmount = 0, totalAmountVAT = 0;
                    //Recorremos los productos para crear las lineas
                    foreach (ProductList product in providerList.Items)
                    {
                        decimal? unitPrice = 0;
                        if (product.Product.ProviderRates != null)
                        {
                            unitPrice = product.Product.ProviderRates.SingleOrDefault(o => o.VendorNo == providerList.ProviderId).DirectUnitCost;
                            if (product.Product.DiscountLineInvoice != null && product.Product.DiscountLineInvoice.LineDiscount > 0)
                                unitPrice *= (1 - (product.Product.DiscountLineInvoice.LineDiscount / 100));
                        }
                        decimal VAT = Convert.ToDecimal(product.Product.VATProdPostingGroup.Replace(".", ","));
                        decimal? providerDiscount = oProvider.InvoiceProviderDiscount.Discount;
                        decimal? amount = (product.Quantity * unitPrice) * (1 - (providerDiscount / 100));
                        decimal? amountVAT = amount * ((VAT / 100) + 1);
                        totalAmount += amount;
                        totalAmountVAT += amountVAT;
                        //Generamos la linea
                        Line oLine = new Line()
                        {
                            DocumentNo = Convert.ToInt32(oCabecera.No),
                            No = product.ProductId,
                            LocationCode = "",
                            Description = product.Product.Description,
                            UnitOfMeasure = product.Product.PurchUnitOfMeasure,
                            Quantity = product.Quantity,
                            DirectUnitCost = unitPrice,
                            VAT = VAT,
                            LineDiscount = providerDiscount,
                            Amount = amount,
                            AmountIncludingVAT = amountVAT,
                            ExpectedReceiptDate = null,//Fecha la mete el usuario
                            OutstandingQuantity = product.Quantity,
                            QuantityToReceive = product.Quantity,
                            QuantityReceived = null
                        };
                        db.LineRequests.Add(oLine);
                    }
                    db.SaveChanges();
                    //Actualizamos estos campos de la cabecera
                    oCabecera.Amount = totalAmount;
                    oCabecera.AmountIncludingVAT = totalAmountVAT;
                    Update(oCabecera);
                }
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<Document> Add(Document c)
        {
            ResultAndError<Document> result = new ResultAndError<Document>();
            try
            {
                db.HeaderRequests.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<Document> Update(Document c, bool insertIfNoExists = false)
        {
            ResultAndError<Document> result = new ResultAndError<Document>();
            try
            {
                Document old = Get(c.No);
                if (old == null && insertIfNoExists) return Add(c);
                db.Entry(old).CurrentValues.SetValues(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<bool> UpdateReadingDate(IEnumerable<int> centersNo, DateTimeOffset readingDate)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                foreach (int no in centersNo)
                {
                    Document oSalesCenter = Get(no);
                    oSalesCenter.ReadingDate = readingDate;
                    db.SaveChanges();
                }
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<Document> oHeaders, bool insertIfNoExists = false)
        {
            foreach (Document centro in oHeaders)
            {
                Document old = Get(centro.No);
                if (old == null && insertIfNoExists) Add(centro);
                else db.Entry(old).CurrentValues.SetValues(centro);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(int id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                Document header = Get(id);
                if (header == null) return result.AddError("No se encontro la cabecera con el id " + id);
                db.HeaderRequests.Remove(header);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<int> cabeceras)
        {
            List<Document> oCabeceras = new List<Document>();
            foreach (int CentroNo in cabeceras)
            {
                Document oCentro = Get(CentroNo);
                if (oCentro != null) oCabeceras.Add(oCentro);
            }
            db.HeaderRequests.RemoveRange(oCabeceras);
            db.SaveChanges();
            return true;
        }
        #endregion
    }
}
