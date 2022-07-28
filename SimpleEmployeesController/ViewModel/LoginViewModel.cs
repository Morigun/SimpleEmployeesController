using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SimpleEmployeesController.Models;
using SimpleEmployeesController.MVVM;
using SimpleEmployeesController.View;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleEmployeesController.ViewModel
{
    public class LoginViewModel : ObservableObject
    {
        IServiceProvider? _serviceProvider;
        EmployeesDbContext? _dbContext;
        public LoginViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbContext = _serviceProvider.GetRequiredService<EmployeesDbContext>();
            _user = new User();
        }
        private User? _user;
        public User? User
        {
            get => _user;
            set => Set(ref _user, value);
        }
        private RelayCommand? _login;
        public RelayCommand? Login
        {
            get
            {
                return _login ??= new RelayCommand(async obj => {
                    if (obj is LoginWindow lw)
                    {
                        var authUser = await _dbContext.Users.Where(a => a.Login == lw.viewModel.User.Login && 
                                                                         a.Password == lw.viewModel.User.Password)
                                                             .FirstOrDefaultAsync();
                        if (authUser != null)
                        {
                            var mainWindow = _serviceProvider?.GetService<MainWindow>();
                            if (mainWindow != null)
                                mainWindow.Show();
                            lw.Close();                            
                            Dispose();
                        }
                        else
                        {
                            MessageBox.Show("Не верно указан логин или пароль!", 
                                            "Ошибка авторизации", 
                                            MessageBoxButton.OK, 
                                            MessageBoxImage.Error);
                        }
                    }
                }, (obj) =>
                {
                    if (obj is LoginWindow lw)
                    {
                        return !string.IsNullOrEmpty(lw?.viewModel?.User?.Login) && 
                               !string.IsNullOrEmpty(lw?.viewModel?.User?.Password);
                    }
                    return false;
                });
            }
        }

        protected override void FreeObjects()
        {
            _serviceProvider = null;
            _dbContext = null;
            _user = null;
            _login = null;
        }
    }
}
