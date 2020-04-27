using Castle.Windsor;
using SERP.Core.IRepositories;
using SERP.Core.IServices;
using SERP.Core.Services;
using SERP.Core.ViewModels;
using SERP.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Injection;

namespace SERP.Desktop
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Serp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            base.OnStartup(e);
    
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ISerpRepository, SerpRepository>(new InjectionConstructor(connectionString));
            container.RegisterType<ISerpService, SerpService>();
            var mainWindow = container.Resolve<MainWindow>();

            mainWindow.Show();
        }
    }
}
