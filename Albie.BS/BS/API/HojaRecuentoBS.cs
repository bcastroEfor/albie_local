using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Albie.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace Albie.BS
{
    public class HojaRecuentoBS : EntityBS, IHojaRecuentoBS
    {
        private readonly IConfiguration _conf;
        private readonly IAlmacenZPBS aBS;
        private readonly ICabeceraRecuentoBS cBS;
        private readonly ICenterBS cnBS;
        public HojaRecuentoBS(RepoDB db, IAlmacenZPBS almacen, ICabeceraRecuentoBS cabecera, ICenterBS center, IConfiguration conf) : base(db)
        {
            _conf = conf;
            aBS = almacen;
            cBS = cabecera;
            cnBS = center;
        }

        

        #region GET
        public CollectionList<HojaRecuento> GetCollectionList(string idRecuento, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            var total = GetHojaRecuentoCount(filter, filterArr, idRecuento);

            if (total == 0) return new CollectionList<HojaRecuento>();

            var items = GetHojaRecuentoList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, idRecuento);

            return new CollectionList<HojaRecuento>
            {
                Items = items,
                Total = total
            };
        }

        public int GetHojaRecuentoCount(string filter = "", List<FilterCriteria> filterArr = null, string idRecuento = "")
        {
            IQueryable<HojaRecuento> lista = db.HojaRecuentos
                                            .Where(o => o.Codigo == idRecuento)
                                            .WhereAct(filterArr, filter, fieldFilter: "EntryNo", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<HojaRecuento> GetHojaRecuentoList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, string idRecuento = "")
        {
            IQueryable<HojaRecuento> lista = db.HojaRecuentos
                                           .Where(o => o.Codigo == idRecuento)
                                           .Include(o => o.Location).Include(o => o.Product).Include(o => o.Zona)
                                           .WhereAct(filterArr, filter, fieldFilter: "EntryNo", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public List<HojaRecuentoGroup> GetHojaRecuentoByCodigo()
        {
            var oHojas = db.HojaRecuentos.GroupBy(o => o.Codigo)
                .Select(o => new HojaRecuentoGroup
                {
                    Name = o.Key,
                    Items = o
                }).ToList();
            return oHojas;
        }

        public HojaRecuento Get(int id)
        {
            return db.HojaRecuentos.SingleOrDefault(o => o.EntryNo == id);
        }

        
        #endregion

        #region POST
        public ResultAndError<HojaRecuento> Add(HojaRecuento pr)
        {
            ResultAndError<HojaRecuento> result = new ResultAndError<HojaRecuento>();
            try
            {
                db.HojaRecuentos.Add(pr);
                db.SaveChanges();
                return result.AddResult(pr);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<HojaRecuento> Update(HojaRecuento pr, bool insertIfNoExists = false)
        {
            ResultAndError<HojaRecuento> result = new ResultAndError<HojaRecuento>();
            try
            {
                HojaRecuento old = Get(pr.EntryNo);
                if (old == null && insertIfNoExists) return Add(pr);
                db.Entry(old).CurrentValues.SetValues(pr);
                db.SaveChanges();
                return result.AddResult(pr);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<HojaRecuento> oHojaRecuentos, bool insertIfNoExists = false)
        {
            foreach (HojaRecuento HojaRecuento in oHojaRecuentos)
            {
                HojaRecuento old = Get(HojaRecuento.EntryNo);
                if (old == null && insertIfNoExists) Add(HojaRecuento);
                else db.Entry(old).CurrentValues.SetValues(HojaRecuento);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(int id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                HojaRecuento oHojaRecuento = Get(id);
                if (oHojaRecuento == null) return result.AddError("No se encontro la hoja con el id " + id);
                db.HojaRecuentos.Remove(oHojaRecuento);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<int> HojaRecuentos)
        {
            List<HojaRecuento> oHojaRecuentos = new List<HojaRecuento>();
            foreach (int HojaRecuentoNo in HojaRecuentos)
            {
                HojaRecuento oHojaRecuento = Get(HojaRecuentoNo);
                if (oHojaRecuento != null) oHojaRecuentos.Add(oHojaRecuento);
            }
            db.HojaRecuentos.RemoveRange(oHojaRecuentos);
            db.SaveChanges();
            return true;
        }

        public ResultAndError<HojaRecuento> AddProductToAlmacen(Product product, string almacen, string zona)
        {
            HojaRecuento oAlmacen = new HojaRecuento()
            {
                LocationCode = almacen,
                ProductNo = product.ProductNo,
                Zone = zona
            };
            return Add(oAlmacen);
        }

        public ResultAndError<Recuento> IniciarRecuento(string center, string almacen, string product, string zona)
        {
            Center oCenter = cnBS.Get("088");//TODO modificar cuando este el login
            ResultAndError<Recuento> recount = new ResultAndError<Recuento>();
            var almacenes = db.AlmacenZPs
                 .Include(o => o.Location)
                 .Include(o => o.Zonas)
                 .Include(o => o.Product)
                 .Where(o => o.LocationCode == almacen)
                 .GroupBy(o => o.LocationCode)
                 .ToList();
            if (almacenes.Count() == 0) return recount.AddError("No hay datos");

            List<string> zonas = zona.Split(',').ToList();

            recount = CheckZonaRecuento(zonas, recount);

            if (recount.HasErrors) return recount;

            CabeceraRecuento oCabecera = new CabeceraRecuento() { Status = 0 };

            cBS.Update(oCabecera, true);

            Recuento oRecuento = new Recuento();
            //Recorremos el group by del almacen
            foreach (var group in almacenes)
            {
                //Aqui recorremos los items que esten agrupados en este almacen
                foreach (AlmacenZP alm in group.ToList())
                {
                    oRecuento.Almacen = alm.Location.Name;
                    List<Zonas> zonaAlmacen = new List<Zonas>();
                    //Recorremos las zonas seleccionadas
                    foreach (string z in zonas)
                    {
                        var tempAlmacenes = group.Where(o => o.Zona == z).ToList();
                        if (tempAlmacenes.Count() == 0) continue; //Si no hay zona saltamos a la siguiente vuelta
                        Zonas oZona = new Zonas();
                        foreach (AlmacenZP zP in tempAlmacenes)//Recorremos los productos
                        {
                            oZona = new Zonas()
                            {
                                Zona = zP.Zonas.Name,
                                Productos = tempAlmacenes.OrderBy(o => o.Product.Description).ToList()
                            };//Creamos el objeto Zonas
                            recount = CheckProductoRecuento(zP.ProductNo, recount);
                            if (recount.HasErrors) return recount;
                            //Creamos el objeto HojaRecuento
                            HojaRecuento oHoja = new HojaRecuento()
                            {
                                Codigo = "HR" + oCabecera.IdRecuento,
                                Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)),
                                CenterCode = center,
                                LocationCode = zP.LocationCode,
                                Zone = zP.Zona,
                                ProductNo = zP.ProductNo,
                                Quantity = zP.Product.StockActual
                            };
                            Update(oHoja, true);//Creamos la hoja del producto
                        }
                        zonaAlmacen.Add(oZona);
                    }
                    oRecuento.Zonas = zonaAlmacen.OrderBy(o => o.Zona).ToList();
                    break;
                }
            }

            recount.AddResult(oRecuento);
            return recount;
        }

        public ResultAndError<Recuento> CheckZonaRecuento(List<string> zonas, ResultAndError<Recuento> recount)
        {
            foreach (string zona in zonas)
            {
                IEnumerable<HojaRecuento> oHojas = db.HojaRecuentos.Where(o => o.Zone == zona && o.Date.Value.Month == DateTime.Now.Month);
                if (oHojas.Count() > 0) return recount.AddError("La zona " + zona + " ya esta en el recuento " + oHojas.First().Codigo);
            }
            return recount;
        }

        public ResultAndError<Recuento> CheckProductoRecuento(string product, ResultAndError<Recuento> recount)
        {
            IEnumerable<HojaRecuento> oHojas = db.HojaRecuentos.Where(o => o.ProductNo == product && o.Date.Value.Month == DateTime.Now.Month);
            if (oHojas.Count() > 0) return recount.AddError("El producto " + product + " ya esta en un recuento" + oHojas.First().Codigo);
            return recount;
        }

        public CollectionList<HojaRecuento> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
