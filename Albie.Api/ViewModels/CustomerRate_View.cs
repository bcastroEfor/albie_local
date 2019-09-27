using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Albie.Models;

namespace Albie.Api.ViewModels
{
    public class CustomerRate_View
    {
        public string ItemNo { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
        public DateTimeOffset? StartingDate { get; set; }
        public DateTimeOffset? EndingDate { get; set; }
        public decimal? UnitPrice { get; set; }
        public string SalesType { get; set; }
        public decimal? MinimumQuantity { get; set; }
        public string UnitMeasureCode { get; set; }
        public UnitMeasureProduct UnitMeasureProduct { get; set; }
        public string SalesCode { get; set; }
        public IEnumerable<SalesCenter> SalesCenters { get; set; }
        public decimal? ServiciosTotales { get; set; }
        public decimal? VentasTotales { get; set; }

        public CustomerRate_View(CustomerRate c)
        {
            ItemNo = c.ItemNo;
            Product = c.Product;
            Customer = c.Customer;
            StartingDate = c.StartingDate;
            EndingDate = c.EndingDate;
            UnitPrice = c.UnitPrice;
            SalesType = c.SalesType;
            MinimumQuantity = c.MinimumQuantity;
            UnitMeasureProduct = c.UnitMeasureProduct;
            UnitMeasureCode = c.UnitMeasureCode;
            SalesCode = c.SalesCode;
            SalesCenters = c.SalesCenters;
            ServiciosTotales = CalcServiciosTotales(c.SalesCenters);
            VentasTotales = Convert.ToDecimal((ServiciosTotales * c.UnitPrice).Value.ToString("N2"));
        }

        public decimal? CalcServiciosTotales(IEnumerable<SalesCenter> sales)
        {
            decimal? total = 0;
            foreach (SalesCenter salesCenter in sales)
            {
                total += salesCenter.Quantity ?? 0;
            }
            return Convert.ToDecimal(total.Value.ToString("N2"));
        }
    }
}
