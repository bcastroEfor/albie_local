using Albie.Models;
using Microsoft.EntityFrameworkCore;

namespace Albie.Repository.Data
{
    public class RepoDB : DBContextBase<RepoDB>
    {
        #region DBSET
        public DbSet<ValueInt> ValueInt { get; set; }
        public DbSet<ValueString> ValueString { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProviderCategory> ProviderCategories { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<UnitMeasureProduct> UnitMeasureProducts { get; set; }
        public DbSet<CartList> Lists { get; set; }
        public DbSet<ProductList> ProductLists { get; set; }
        public DbSet<ProviderRate> ProviderRates { get; set; }
        public DbSet<Dimension> Dimensions { get; set; }
        public DbSet<Document> HeaderRequests { get; set; }
        public DbSet<Line> LineRequests { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CrossReference> CrossReferences { get; set; }
        public DbSet<Center> Centros { get; set; }
        public DbSet<Subcenter> Subcenters { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<InvoiceProviderDiscount> InvoiceProviderDiscounts { get; set; }
        public DbSet<DiscountLineInvoice> DiscountLineInvoices { get; set; }
        public DbSet<ZoneProvider> ZoneProviders { get; set; }
        public DbSet<FamilyProvider> FamilyProviders { get; set; }
        public DbSet<CustomerCenter> CustomerCenters { get; set; }
        public DbSet<CustomerRate> CustomerRates { get; set; }
        public DbSet<SalesCenter> SalesCenters { get; set; }
        public DbSet<SalesCountedCenter> SalesCountedCenters { get; set; }
        public DbSet<CashMovementCenter> CashMovementCenters { get; set; }
        public DbSet<AlbaranCompra> AlbaranCompras { get; set; }
        public DbSet<AlbaranLinea> AlbaranLineas { get; set; }
        public DbSet<PedVentaCab> PedVentaCabs { get; set; }
        public DbSet<PedVentaLinea> PedVentaLineas { get; set; }
        public DbSet<AlmacenZP> AlmacenZPs { get; set; }
        public DbSet<Zona> Zonas { get; set; }
        public DbSet<HojaRecuento> HojaRecuentos { get; set; }
        public DbSet<HistoricoPedido> HistoricoPedidos { get; set; }
        public DbSet<CabeceraRecuento> CabeceraRecuentos { get; set; }
        #endregion

        public RepoDB(DbContextOptions<RepoDB> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForSqlServerUseIdentityColumns();
            #region MODEL TOTABLE
            modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<UnitMeasureProduct>().ToTable("UnitMeasureProduct");
            modelBuilder.Entity<ProviderCategory>().ToTable("ProviderCategory").HasKey(o => new { o.Code, o.VendorNo });
            modelBuilder.Entity<Provider>().ToTable("Provider");
            modelBuilder.Entity<CartList>().ToTable("List");
            modelBuilder.Entity<ProductList>().ToTable("ListProduct");
            modelBuilder.Entity<ProviderRate>().ToTable("ProviderRate").HasKey(o => new { o.ProductNo, o.VendorNo, });
            modelBuilder.Entity<Dimension>().ToTable("Dimensions");
            modelBuilder.Entity<Document>().ToTable("Document");
            modelBuilder.Entity<Line>().ToTable("Line");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<CrossReference>().ToTable("CrossReference");
            modelBuilder.Entity<Center>().ToTable("Center");
            modelBuilder.Entity<Subcenter>().ToTable("Subcenter");
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<InvoiceProviderDiscount>().ToTable("InvoiceProviderDiscount");
            modelBuilder.Entity<DiscountLineInvoice>().ToTable("DiscountLineInvoice");
            modelBuilder.Entity<ZoneProvider>().ToTable("ZoneProvider");
            modelBuilder.Entity<FamilyProvider>().ToTable("FamilyProvider");
            modelBuilder.Entity<CustomerCenter>().ToTable("CustomerCenter");
            modelBuilder.Entity<CustomerRate>().ToTable("CustomerRate");
            modelBuilder.Entity<SalesCenter>().ToTable("SalesCenter");
            modelBuilder.Entity<SalesCountedCenter>().ToTable("SalesCountedCenter");
            modelBuilder.Entity<CashMovementCenter>().ToTable("CashMovementCenter");
            modelBuilder.Entity<AlbaranCompra>().ToTable("AlbaranCompra").HasKey(o => o.No);
            modelBuilder.Entity<AlbaranLinea>().ToTable("AlbaranLinea").HasKey(o => new { o.No, o.AlbaranCompraNo });
            modelBuilder.Entity<PedVentaCab>().ToTable("PedVentaCab");
            modelBuilder.Entity<PedVentaLinea>().ToTable("PedVentaLinea").HasKey(o => new { o.DocumentNo, o.LineNo });
            modelBuilder.Entity<AlmacenZP>().ToTable("AlmacenZonaProducto").HasKey(o => new { o.LocationCode, o.ProductNo });
            modelBuilder.Entity<Zona>().ToTable("Zona");
            modelBuilder.Entity<HojaRecuento>().ToTable("HojaRecuento").HasKey(o => o.EntryNo);
            modelBuilder.Entity<HistoricoPedido>().ToTable("HistoricoPedido");
            modelBuilder.Entity<CabeceraRecuento>().ToTable("CabeceraRecuento");
            #endregion

            #region RELATIONSHIPS
            modelBuilder.Entity<AlmacenZP>().HasOne(o => o.Location).WithMany(o => o.AlmacenZP).HasForeignKey(o => o.LocationCode);
            modelBuilder.Entity<AlmacenZP>().HasOne(o => o.Zonas).WithMany(o => o.AlmacenZP).HasForeignKey(o => o.Zona);
            modelBuilder.Entity<AlmacenZP>().HasOne(o => o.Product).WithMany(o => o.AlmacenZP).HasForeignKey(o => o.ProductNo);

            modelBuilder.Entity<CartList>().HasMany(o => o.ProductList).WithOne(o => o.List);

            modelBuilder.Entity<CashMovementCenter>().HasOne(o => o.Center);

            modelBuilder.Entity<Center>().HasOne(o => o.Location);
            modelBuilder.Entity<Center>().HasOne(o => o.DefaultCustomer);
            modelBuilder.Entity<Center>().HasOne(o => o.PrincipalCustomer);
            modelBuilder.Entity<Center>().HasOne(o => o.Centro).WithMany(o => o.Centers).HasForeignKey(o => o.GlobalDimension1Code);
            modelBuilder.Entity<Center>().HasOne(o => o.Zona).WithMany(o => o.ZoneCenter).HasForeignKey(o => o.GlobalDimension2Code);
            modelBuilder.Entity<Center>().HasMany(c => c.Subcenters).WithOne(o => o.Center).HasForeignKey(c => c.CenterCode).HasPrincipalKey(o => o.Code);

            modelBuilder.Entity<Customer>().HasOne(o => o.Centro).WithMany(o => o.CenterCustomers).HasForeignKey(o => o.GlobalDimension1Code);
            modelBuilder.Entity<Customer>().HasOne(o => o.Zona).WithMany(o => o.ZoneCustomers).HasForeignKey(o => o.GlobalDimension2Code);

            modelBuilder.Entity<CustomerCenter>().HasOne(o => o.Customer).WithMany(o => o.CustomerCenters).HasForeignKey(o => o.CustomerNo);
            modelBuilder.Entity<CustomerCenter>().HasOne(o => o.Center);

            modelBuilder.Entity<CustomerRate>().HasOne(o => o.Product).WithMany(o => o.CustomerRates).HasForeignKey(o => o.ItemNo);
            modelBuilder.Entity<CustomerRate>().HasOne(o => o.Customer).WithMany(o => o.CustomerRates).HasForeignKey(o => o.SalesCode);
            modelBuilder.Entity<CustomerRate>().HasOne(o => o.UnitMeasureProduct).WithMany(o => o.CustomerRates).HasForeignKey(o => o.UnitMeasureCode);
            modelBuilder.Entity<CustomerRate>().HasMany(c => c.SalesCenters).WithOne(s => s.CustomerRate).HasForeignKey(c => c.CustomerNo).HasPrincipalKey(s => s.SalesCode);

            modelBuilder.Entity<Document>().HasOne(o => o.Centros).WithMany(o => o.DocumentCenter).HasForeignKey(o => o.Centro);
            modelBuilder.Entity<Document>().HasOne(o => o.Zonas).WithMany(o => o.DocumentZone).HasForeignKey(o => o.Zona);
            modelBuilder.Entity<Document>().HasOne(o => o.Provider).WithMany(o => o.HeaderOrder).HasForeignKey(o => o.BuyFromVendorNo);

            modelBuilder.Entity<DiscountLineInvoice>().HasOne(o => o.Product).WithOne(o => o.DiscountLineInvoice).HasForeignKey<DiscountLineInvoice>(o => o.ItemNo);

            modelBuilder.Entity<FamilyProvider>().HasOne(o => o.Provider).WithMany(o => o.FamilyProviders).HasForeignKey(o => o.VendorNo);
            modelBuilder.Entity<FamilyProvider>().HasOne(o => o.ProviderCategory).WithMany(o => o.FamilyProviders).HasForeignKey(o => o.ItemCategoryCode);

            modelBuilder.Entity<HojaRecuento>().HasOne(o => o.Location).WithMany(o => o.HojaRecuentos).HasForeignKey(o => o.LocationCode);
            modelBuilder.Entity<HojaRecuento>().HasOne(o => o.Product).WithMany(o => o.HojaRecuentos).HasForeignKey(o => o.ProductNo);
            modelBuilder.Entity<HojaRecuento>().HasOne(o => o.Zona).WithMany(o => o.HojaRecuentos).HasForeignKey(o => o.Zone);

            modelBuilder.Entity<InvoiceProviderDiscount>().HasOne(o => o.Provider).WithOne(o => o.InvoiceProviderDiscount).HasForeignKey<InvoiceProviderDiscount>(o => o.Code);

            modelBuilder.Entity<Line>().HasOne(o => o.Order).WithMany(o => o.Lines).HasForeignKey(o => o.DocumentNo);
            modelBuilder.Entity<Line>().HasOne(o => o.Product).WithMany(o => o.Lines).HasForeignKey(o => o.No);
            modelBuilder.Entity<Line>().HasOne(o => o.UnitMeasureProduct).WithMany(o => o.Lines).HasForeignKey(o => o.UnitOfMeasure);
            modelBuilder.Entity<Line>().HasOne(o => o.Location).WithMany(o => o.Lines).HasForeignKey(o => o.LocationCode);

            modelBuilder.Entity<Product>().HasOne(o => o.UnitMeasureProduct).WithOne(o => o.Product).HasForeignKey<UnitMeasureProduct>(o => o.ProductNo);
            modelBuilder.Entity<Product>().HasMany(o => o.ProviderRates).WithOne(o => o.Product).HasForeignKey(o => o.ProductNo);

            modelBuilder.Entity<ProductCategory>().HasMany(o => o.Product).WithOne(o => o.ProductCategory);

            modelBuilder.Entity<ProductList>().HasOne(o => o.Product).WithMany(o => o.ProductLists).HasForeignKey(o => o.ProductId);
            modelBuilder.Entity<ProductList>().HasOne(o => o.ProviderRate).WithMany(o => o.ProductLists).HasForeignKey(o => new { o.ProductId, o.ProviderId });

            modelBuilder.Entity<Provider>().HasOne(o => o.ProviderRate).WithOne(o => o.Provider).HasForeignKey<ProviderRate>(o => o.VendorNo);
            modelBuilder.Entity<Provider>().HasOne(o => o.Centro).WithMany(o => o.CenterProvider).HasForeignKey(o => o.GlobalDimension1Code);
            modelBuilder.Entity<Provider>().HasOne(o => o.Zona).WithMany(o => o.ZoneProvider).HasForeignKey(o => o.GlobalDimension2Code);
            modelBuilder.Entity<Provider>().HasMany(o => o.DiscountLineInvoice).WithOne(o => o.Provider);

            modelBuilder.Entity<ProviderCategory>().HasOne(o => o.Provider).WithOne(o => o.ProviderCategory).HasForeignKey<ProviderCategory>(o => o.VendorNo);

            modelBuilder.Entity<ProviderRate>().HasOne(o => o.Product).WithMany(o => o.ProviderRates).HasForeignKey(o => o.ProductNo);
            modelBuilder.Entity<ProviderRate>().HasOne(o => o.Provider).WithOne(o => o.ProviderRate).HasForeignKey<ProviderRate>(o => o.VendorNo);
            modelBuilder.Entity<ProviderRate>().HasOne(o => o.UnitMeasureProduct).WithOne(o => o.ProviderRate).HasForeignKey<ProviderRate>(o => o.UnitMeasureCode);

            modelBuilder.Entity<SalesCenter>().HasOne(s => s.Center).WithMany(c => c.SaleCenters).HasForeignKey(s => s.CenterCode).HasPrincipalKey(c => c.Code);
            modelBuilder.Entity<SalesCenter>().HasOne(s => s.Customer).WithMany(c => c.SalesCenters).HasForeignKey(s => s.CustomerNo).HasPrincipalKey(c => c.No);
            modelBuilder.Entity<SalesCenter>().HasOne(s => s.CustomerRate).WithMany(c => c.SalesCenters).HasForeignKey(s => s.CustomerNo).HasPrincipalKey(c => c.SalesCode);

            modelBuilder.Entity<SalesCountedCenter>().HasOne(o => o.Center);
            modelBuilder.Entity<SalesCountedCenter>().HasOne(o => o.Subcenter);

            modelBuilder.Entity<Subcenter>().HasOne(o => o.Center);
            modelBuilder.Entity<Subcenter>().HasOne(o => o.Centro).WithMany(o => o.SubCenterCenter).HasForeignKey(o => o.GlobalDimension1Code);
            modelBuilder.Entity<Subcenter>().HasOne(o => o.Zona).WithMany(o => o.SubCenterZone).HasForeignKey(o => o.GlobalDimension2Code);
            modelBuilder.Entity<Subcenter>().HasMany(o => o.SalesCountedCenters).WithOne(s => s.Subcenter).HasForeignKey(o => o.SubcenterCode).HasPrincipalKey(s => s.Code);

            modelBuilder.Entity<UnitMeasureProduct>().HasOne(o => o.ProviderRate).WithOne(o => o.UnitMeasureProduct).HasForeignKey<ProviderRate>(o => o.UnitMeasureCode);
            modelBuilder.Entity<UnitMeasureProduct>().HasOne(o => o.Product);

            modelBuilder.Entity<ZoneProvider>().HasOne(o => o.Provider).WithMany(o => o.ZoneProviders).HasForeignKey(o => o.VendorNo);
            modelBuilder.Entity<ZoneProvider>().HasOne(o => o.Centro);
            #endregion
        }
    }
}
