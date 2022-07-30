using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SimpleEmployeesController.Models;
using SimpleEmployeesController.Properties;
using SimpleEmployeesController.View;
using SimpleEmployeesController.ViewModel;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleEmployeesController
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
            SelectTheme(GetThemeFromSettings());
        }
        private string GetThemeFromSettings()
        {
            return Settings.Default.Theme;
        }
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<EmployeesDbContext>(options =>
            {
                options.UseSqlite("Data Source = Employee.db");
            });
            services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginWindow>();
            services.AddSingleton<IMainViewModel, MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var loginWindow = serviceProvider.GetService<LoginWindow>();
            loginWindow?.Show();
        }
        public static void SelectTheme(string? theme)
        {
            if (string.IsNullOrEmpty(theme)) return;
            Settings.Default.Theme = theme;
            Settings.Default.Save();
            string style = $"Themes/{theme}.xaml";
            var uri = new Uri(style, UriKind.Relative);
            ResourceDictionary? resourceDict = LoadComponent(uri) as ResourceDictionary;
            if (resourceDict != null)
            {
                Current.Resources.Clear();
                Current.Resources.MergedDictionaries.Add(resourceDict);
            }
        }
    }
}
