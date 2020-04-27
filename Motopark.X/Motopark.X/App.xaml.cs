
using Motopark.Core.Entities;
using Motopark.Core.IRepositories;
using Motopark.Core.IServices;
using Motopark.Core.Services;
using Motopark.Core.ViewModels;
using Motopark.Infrastructure.Repositories;
using Motopark.X.Pages;
using System;
using Unity;
using Unity.Injection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Motopark.X
{
    public partial class App : Application
    {
        public App()
        {
            var _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Motopark;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            
            InitializeComponent();

            IUnityContainer container = new UnityContainer();
            container.RegisterType<IAdminRepository<Admin>, AdminRepository>(new InjectionConstructor(_connectionString));
            container.RegisterType<IBasketRepository<Basket>, BasketRepository>(new InjectionConstructor(_connectionString));
            container.RegisterType<ICategoryRepository<Category>, CategoryRepository>(new InjectionConstructor(_connectionString));
            container.RegisterType<IDeliveryRepository<Delivery>, DeliveryRepository>(new InjectionConstructor(_connectionString));
            container.RegisterType<IFeatureRepository<Feature>, FeatureRepository>(new InjectionConstructor(_connectionString));
            container.RegisterType<IImageProductRepository<ImageProduct>, ImageProductRepository>(new InjectionConstructor(_connectionString));
            container.RegisterType<IOrderRepository<Order>, OrderRepository>(new InjectionConstructor(_connectionString));
            container.RegisterType<IProductRepository<Product>, ProductRepository>(new InjectionConstructor(_connectionString));

            container.RegisterType<IAdminService<Admin>, AdminService>();
            container.RegisterType<IBasketService<Basket>, BasketService>();
            container.RegisterType<ICategoryService<Category>, CategoryService>();
            container.RegisterType<IDeliveryService<Delivery>, DeliveryService>();
            container.RegisterType<IFeatureService<Feature>, FeatureService>();
            container.RegisterType<IImageProductService<ImageProduct>, ImageProductService>();
            container.RegisterType<IOrderService<Order>, OrderService>();
            container.RegisterType<IProductService<Product>, ProductService>();


            var mainPage = new MainPage(container);
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
