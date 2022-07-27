using SimpleEmployeesController.Models;

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleEmployeesController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EmployeesDbContext _dbContext;
        public MainWindow(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
            GetEmployees();
        }

        private void GetEmployees()
        {
            var employees = _dbContext.Employees.ToList();
            EmployeesDG.ItemsSource = employees;
        }
    }
}
