using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SimpleEmployeesController.Models;
using SimpleEmployeesController.Models.Attributes;
using SimpleEmployeesController.MVVM;
using SimpleEmployeesController.Properties;
using SimpleEmployeesController.ViewModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

namespace SimpleEmployeesController.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            InitWindow();
            InitDataGrid();
            InitControlPanel();
        }
        private void InitWindow()
        {
            Width = Settings.Default.MainWindowWidth;
            Height = Settings.Default.MainWindowHeight;
        }
        private void InitDataGrid()
        {
            EmployeesDG.Columns.Clear();
            bool isAdmin = ((IMainViewModel)DataContext).IsAdmin;
            EmployeesDG.AutoGenerateColumns = false;
            EmployeesDG.CanUserAddRows = isAdmin;
            EmployeesDG.CanUserDeleteRows = false;
            var empPropertyes = ((IMainViewModel)DataContext).GetPropertiesForGenerationColumns();
            foreach (var property in empPropertyes)
            {
                EmployeesDG.Columns.Add(new DataGridTextColumn
                {
                    Binding = new Binding(property.Name),
                    IsReadOnly = !isAdmin,
                    Header = property.GetAttribute<DisplayNameAttribute>()?.DisplayName ?? property.Name,
                    //Считаем если аттрибута нет, то колонка видна
                    Visibility = property.GetAttribute<VisibleAttribute>()?.Visible == false ? Visibility.Hidden : Visibility.Visible
                });
            }
        }
        private void InitControlPanel()
        {
            ControlPanel.Visibility = ((IMainViewModel)DataContext).IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ThemeSelectMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is MenuItem menuItem)
                App.SelectTheme(menuItem!.Tag?.ToString());
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Settings.Default.MainWindowWidth = Width;
            Settings.Default.MainWindowHeight = Height;
            Settings.Default.Save();
        }
    }
}
