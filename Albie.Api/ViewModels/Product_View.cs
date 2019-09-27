using Albie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albie.Api.ViewModels
{
    public class Product_View
    {
        public string ProductNo { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string BaseUnitOfMeasure { get; set; }
        public int? Type { get; set; }
        public decimal? UnitPrice { get; set; }
        public string VATProdPostingGroup { get; set; }
        public string SalesUnitOfMeasure { get; set; }
        public string PurchUnitOfMeasure { get; set; }
        public string ProductCategoryCode { get; set; }
        public string Photo { get; set; }
        public UnitMeasureProduct UnitMeasureProduct { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public decimal TotalUnits { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? ReceptionMAxPct { get; set; }
        public Guid ProviderRateId { get; set; }
        public IEnumerable<ProviderRate> ProviderRates { get; set; }
        public string ProviderId { get; set; }
        public ICollection<CustomerRate> CustomerRates { get; set; }
        public ICollection<Line> Lines { get; set; }
        public DiscountLineInvoice DiscountLineInvoice { get; set; }
        public ICollection<ProductList> ProductLists { get; set; }
        public Product_View(Product oProduct)
        {
            ProductNo = oProduct.ProductNo;
            Description = oProduct.Description ?? "";
            Description2 = oProduct.Description2 ?? "";
            BaseUnitOfMeasure = oProduct.BaseUnitOfMeasure ?? "";
            Type = oProduct.Type ?? 0;
            UnitPrice = oProduct.ProviderRates.Count() == 0 ? 0 : oProduct.ProviderRates.OrderBy(o => o.DirectUnitCost).First().DirectUnitCost ?? 0;
            VATProdPostingGroup = oProduct.VATProdPostingGroup ?? "";
            SalesUnitOfMeasure = oProduct.SalesUnitOfMeasure ?? "";
            PurchUnitOfMeasure = oProduct.PurchUnitOfMeasure ?? "";
            ProductCategoryCode = oProduct.ProductCategoryCode ?? "";
            Photo = oProduct.Photo ?? "";
            UnitMeasureProduct = oProduct.UnitMeasureProduct;
            ProductCategory = oProduct.ProductCategory;
            TotalUnits = oProduct.TotalUnits;
            TotalPrice = oProduct.TotalPrice;
            ProviderRateId = oProduct.ProviderRateId;
            ProviderRates = oProduct.ProviderRates.OrderBy(o => o.DirectUnitCost);
            ReceptionMAxPct = oProduct.ReceptionMAxPct ?? 0;
        }
    }
}
