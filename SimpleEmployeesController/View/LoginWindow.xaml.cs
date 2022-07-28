using Microsoft.Extensions.DependencyInjection;

using SimpleEmployeesController.Models;
using SimpleEmployeesController.ViewModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleEmployeesController.View
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        EmployeesDbContext _dbContext;
        IServiceProvider _serviceProvider;
        internal LoginViewModel viewModel;
        public LoginWindow(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbContext = _serviceProvider?.GetService<EmployeesDbContext>();
            InitializeComponent();
            viewModel = _serviceProvider.GetService<LoginViewModel>();
            DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                viewModel.User.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
