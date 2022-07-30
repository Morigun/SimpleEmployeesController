using Microsoft.EntityFrameworkCore;

using SimpleEmployeesController.Models;
using SimpleEmployeesController.MVVM;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SimpleEmployeesController.ViewModel
{
    internal class MainWindowViewModel : ObservableObject, IMainViewModel
    {
        #region Private variables
        private string _search;
        EmployeesDbContext? _dbContext;
        int _lastId = 0;
        ObservableCollection<Employee>? _employees;
        #endregion
        #region Public Property
        public int LastId { get => ++_lastId; }
        public ICollectionView FiltredEmployee { get; }
        public bool IsAdmin { get => AuthUser.GetInstance().User!.IsAdmin; }
        public string Search
        {
            get => _search;
            set => Set(ref _search, value);
        }
        #endregion
        public MainWindowViewModel(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
            _employees = new ObservableCollection<Employee>(_dbContext!.Employees!.ToList());
            FiltredEmployee = CollectionViewSource.GetDefaultView(_employees);
            _lastId = _dbContext!.Employees!.Max(a => a.EmployeeId);
            _search = string.Empty;
        }
        #region Commands
        private RelayCommand? _add;
        public RelayCommand Add
        {
            get => _add ??= new RelayCommand(async (obj) =>
            {
                if (obj == null) return;
                if (obj is Employee employee)
                {
                    employee.SetDBContext(_dbContext!);
                    if (!await employee.InsertInDBAsync())
                    {
                        MessageBox.Show("Сотрудник с таким ФИО уже есть в БД",
                                        "Ошибка при добавлении сотрудника",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                        _employees!.Remove(employee);
                    }
                }
            },
            (obj) =>
            {
                if (obj == null) return false;
                if (obj is Employee employee &&
                    !employee.CheckInDB())
                    return true;
                return false;
            });
        }
        private RelayCommand? _delete;
        public RelayCommand Delete
        {
            get => _delete ??= new RelayCommand(async (obj) =>
            {
                if (obj == null) return;
                if (obj is Employee employee)
                {
                    _employees!.Remove(employee);
                    if (!employee.CheckInDB()) return;
                    _dbContext!.Remove(employee);
                    await _dbContext.SaveChangesAsync();
                }
            });
        }
        private RelayCommand? _filter;
        public RelayCommand Filter => _filter ??= new RelayCommand((obj) =>
        {
            if (string.IsNullOrEmpty(_search))
            {
                FiltredEmployee.Filter = null;
                FiltredEmployee.Refresh();
            }
            else
            {
                FiltredEmployee.Filter = (obj) => {
                    if (obj != null && obj is Employee employee)
                    {
                        return (employee!.LastName ?? string.Empty).ToLower().Contains(_search.ToLower());
                    }
                    return false;
                };
                FiltredEmployee.Refresh();
            }
        });
        private RelayCommand? _exit;
        public RelayCommand Exit => _exit ??= new RelayCommand((obj) =>
        {
            App.Current.MainWindow.Close();
        });
        private RelayCommand? _startAddRow;
        public RelayCommand StartAddRow => _startAddRow ??= new RelayCommand((obj) =>
        {
            if (obj != null && obj is AddingNewItemEventArgs e)
                e.NewItem = new Employee { EmployeeId = LastId };
        });
        #endregion
        #region Methods
        public List<PropertyInfo> GetPropertiesForGenerationColumns()
        {
            return typeof(Employee).GetProperties().ToList();
        }
        #region Dispose
        protected override void FreeObjects()
        {
            _employees?.Clear();
            _employees = null;
        }
        #endregion
        #endregion
    }
}
