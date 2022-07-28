using SimpleEmployeesController.Models;
using SimpleEmployeesController.MVVM;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController.ViewModel
{
    internal class MainWindowViewModel : ObservableObject
    {
        EmployeesDbContext? _dbContext;
        public MainWindowViewModel(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
            _employees = new ObservableCollection<Employee>(_dbContext.Employees.ToList());
            _employees.CollectionChanged += Employees_CollectionChanged;
        }

        private async void Employees_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                await _dbContext.AddAsync<Employee>((Employee)e.NewItems[0]);
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                _dbContext.Remove<Employee>((Employee)e.OldItems[0]);
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset ||
                     e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace ||
                     e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
                _dbContext.UpdateRange(e.OldItems);
        }

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => Set(ref _employees, value);
        }
        protected override void FreeObjects()
        {
            _employees.Clear();
            _employees = null;
        }
    }
}
