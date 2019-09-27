using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class Product
    {
        [Key]
        public string ProductNo { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string BaseUnitOfMeasure { get; set; }
        public int? Type { get; set; }
        public decimal? UnitPrice { get; set; }
        public string VATProdPostingGroup { get; set; }
        public string SalesUnitOfMeasure { get; set; }
        public string PurchUnitOfMeasure { get; set; }
        [Column("ItemCategoryCode")]
        public string ProductCategoryCode { get; set; }
        public string Photo { get; set; }
        public decimal? StockActual { get; set; }
        public decimal? StockAnterior { get; set; }
        public decimal? ReceptionMAxPct { get; set; }
        [NotMapped]
        public UnitMeasureProduct UnitMeasureProduct { get; set; }
        [NotMapped]
        public ProductCategory ProductCategory { get; set; }
        [NotMapped]
        public decimal TotalUnits { get; set; }
        [NotMapped]
        public decimal TotalPrice { get; set; }
        [NotMapped]
        public Guid ProviderRateId { get; set; }
        [NotMapped]
        public ICollection<ProviderRate> ProviderRates { get; set; }
        [NotMapped]
        public string ProviderId { get; set; }
        [NotMapped]
        public ICollection<CustomerRate> CustomerRates { get; set; }
        [NotMapped]
        public ICollection<Line> Lines { get; set; }
        [NotMapped]
        public DiscountLineInvoice DiscountLineInvoice { get; set; }
        [NotMapped]
        public ICollection<ProductList> ProductLists { get; set; }
        public ICollection<AlmacenZP> AlmacenZP { get; set; }
        public ICollection<HojaRecuento> HojaRecuentos { get; set; }
    }
}
