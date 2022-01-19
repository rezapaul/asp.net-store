using EndPoint.Website.HangFireOptions;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Application.Services.Address.Commands.IAddAddress;
using MyStore.Application.Services.Address.Commands.IRemoveAddress;
using MyStore.Application.Services.Address.Queries.IGetAddresses;
using MyStore.Application.Services.Carts.Commands.IAddCartitemCount;
using MyStore.Application.Services.Carts.Commands.IAddtocart;
using MyStore.Application.Services.Carts.Commands.ILowCartitemCount;
using MyStore.Application.Services.Carts.Commands.IRemoveFromCart;
using MyStore.Application.Services.Carts.Queries.IGetCartService;
using MyStore.Application.Services.Categories.Commands.IAddCategoryService;
using MyStore.Application.Services.Categories.Queries;
using MyStore.Application.Services.Categories.Queries.IAllCategoryListService;
using MyStore.Application.Services.Common.Queries.IGetMenu;
using MyStore.Application.Services.Contacts.Commands.ISendConatct;
using MyStore.Application.Services.Contacts.Queries.IDetailContactAdmin;
using MyStore.Application.Services.Contacts.Queries.IGetContactsAdmin;
using MyStore.Application.Services.Orders.Commands.IAddOrder;
using MyStore.Application.Services.Orders.Commands.IChangeStatus;
using MyStore.Application.Services.Orders.Queries.IGetOrderDeatil;
using MyStore.Application.Services.Orders.Queries.IGetOrderDetailAdmin;
using MyStore.Application.Services.Orders.Queries.IGetOrdersForAdmin;
using MyStore.Application.Services.Orders.Queries.IGetUserOrders;
using MyStore.Application.Services.Products.Commands.IAddProductAdminService;
using MyStore.Application.Services.Products.Commands.IDeleteProductAdmin;
using MyStore.Application.Services.Products.Commands.IEditProductDisplaying;
using MyStore.Application.Services.Products.Queries.IProductDetailForAdmin;
using MyStore.Application.Services.Products.Queries.IProductDetailForWebsite;
using MyStore.Application.Services.Products.Queries.IProductListForAdmin;
using MyStore.Application.Services.Products.Queries.IProductsForWebsite;
using MyStore.Application.Services.Wishlists.Commands.IAddToWishlistService;
using MyStore.Application.Services.Wishlists.Commands.IDeleteFromWishList;
using MyStore.Application.Services.Wishlists.Queries.IGetWishlistsService;
using MyStore.Common.Persian_Translations;
using MyStore.Persistence;
using MyStore.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddScoped<IChangeStatus, ChangeStatus>();
            services.AddScoped<IGetOrderDetailAdmin, GetOrderDetailAdmin>();
            services.AddScoped<IGetOrdersForAdmin, GetOrdersForAdmin>();
            services.AddScoped<IGetOrderDeatil, GetOrderDeatil>();
            services.AddScoped<IGetUserOrders, GetUserOrders>();
            services.AddScoped<IAddOrder, AddOrder>();
            services.AddScoped<IGetAddresses, GetAddresses>();
            services.AddScoped<IRemoveAddress, RemoveAddress>();
            services.AddScoped<IAddAddress, AddAddress>();
            services.AddScoped<ILowCartitemCount, LowCartitemCount>();
            services.AddScoped<IAddCartitemCount, AddCartitemCount>();
            services.AddScoped<IRemoveFromCart, RemoveFromCart>();
            services.AddScoped<IGetCartService, GetCartService>();
            services.AddScoped<IAddtocartService, AddtocartService>();
            services.AddScoped<IDetailContactAdmin, DetailContactAdmin>();
            services.AddScoped<IGetContactsAdmin, GetContactsAdmin>();
            services.AddScoped<ISendContact, SendContact>();
            services.AddScoped<IGetMenu, GetMenu>();
            services.AddScoped<IDeleteFromWishList, DeleteFromWishList>();
            services.AddScoped<IGetWishlistsService, GetWishlistsService>();
            services.AddScoped<IAddToWishlistService, AddToWishlistService>();
            services.AddScoped<IProductDetailForWebsite, ProductDetailForWebsite>();
            services.AddScoped<IProductsForWebsite, ProductsForWebsite>();
            services.AddScoped<IProductDetailForAdmin, ProductDetailForAdmin>();
            services.AddScoped<IDeleteProductAdmin, DeleteProductAdmin>();
            services.AddScoped<IEditProductDisplaying, EditProductDisplaying>();
            services.AddScoped<IProductListForAdmin, ProductListForAdmin>();
            services.AddScoped<IGetCategoriesListService, GetCategoriesListService>();
            services.AddScoped<IAllCategoryListService, AllCategoryListService>();
            services.AddScoped<IAddCategoryService, AddCategoryService>();
            services.AddScoped<IAddProductAdminService, AddProductAdminService>();
            services.AddScoped<IDataBaseContext, DataBaseContext>();
            services.AddEntityFrameworkSqlServer().AddDbContext<DataBaseContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddIdentity<UserAdditional, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddEntityFrameworkStores<DataBaseContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<PersianIdentityErrorDescriber>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHangfireDashboard();

            });
        }
    }
}
