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
using System.Windows.Shapes;

namespace SimpleEmployeesController
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        EmployeesDbContext _dbContext;
        public Login(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
        }
    }
}
