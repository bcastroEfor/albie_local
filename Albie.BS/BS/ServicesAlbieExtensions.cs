using Albie.BS.Interfaces;
using Albie.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Albie.BS
{
    public static class ServicesAlbieExtensions
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RepoDB>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services.AddInspectionServices();
        }

        public static IServiceCollection AddInspectionServices(this IServiceCollection services)
        {
            services.AddScoped<ICartBS, CartBS>();
            services.AddScoped<ICashMovementCenterBS, CashMovementCenterBS>();
            services.AddScoped<ICenterBS, CenterBS>();
            services.AddScoped<ICrossReferenceBS, CrossReferenceBS>();
            services.AddScoped<ICustomerBS, CustomerBS>();
            services.AddScoped<ICustomerCenterBS, CustomerCenterBS>();
            services.AddScoped<ICustomerRateBS, CustomerRateBS>();
            services.AddScoped<IDimensionBS, DimensionBS>();
            services.AddScoped<IDateBS, DateBS>();
            services.AddScoped<IFamilyProviderBS, FamilyProviderBS>();
            services.AddScoped<IHeaderOrderBS, HeaderOrderBS>();
            services.AddScoped<IInvoiceLineDiscountBS, InvoiceLineDiscountBS>();
            services.AddScoped<IInvoiceProviderDiscountBS, InvoiceProviderDiscountBS>();
            services.AddScoped<IListBS, ListBS>();
            services.AddScoped<ILineBS, LineBS>();
            services.AddScoped<ILocationBS, LocationBS>();
            services.AddScoped<IProductBS, ProductBS>();
            services.AddScoped<IProductCategoryBS, ProductCategoryBS>();
            services.AddScoped<IProviderBS, ProviderBS>();
            services.AddScoped<IProviderRateBS, ProviderRateBS>();
            services.AddScoped<ISalesCenterBS, SalesCenterBS>();
            services.AddScoped<ISalesCountedCenterBS, SalesCountedCenterBS>();
            services.AddScoped<ISubCentreBS, SubCentreBS>();
            services.AddScoped<IUnitMeasureProductBS, UnitMeasureProductBS>();
            services.AddScoped<IZoneProviderBS, ZoneProviderBS>();
            services.AddScoped<IAlbaranCompraBS, AlbaranCompraBS>();
            services.AddScoped<IAlbaranLineaBS, AlbaranLineaBS>();
            services.AddScoped<IPedVentaCabBS, PedVentaCabBS>();
            services.AddScoped<IPedVentaLineaBS, PedVentaLineaBS>();
            services.AddScoped<IAlmacenZPBS, AlmacenZPBS>();
            services.AddScoped<IZonaBS, ZonaBS>();
            services.AddScoped<IHistoricoPedidoBS, HistoricoPedidoBS>();
            services.AddScoped<IHojaRecuentoBS, HojaRecuentoBS>();
            services.AddScoped<ICabeceraRecuentoBS, CabeceraRecuentoBS>();
            
            return services;
        }
    }
}
