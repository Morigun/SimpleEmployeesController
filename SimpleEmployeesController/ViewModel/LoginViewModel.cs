using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SimpleEmployeesController.Models;
using SimpleEmployeesController.MVVM;
using SimpleEmployeesController.Properties;
using SimpleEmployeesController.View;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimpleEmployeesController.ViewModel
{
    public class LoginViewModel : ObservableObject
    {
        #region Private variables
        IServiceProvider? _serviceProvider;
        EmployeesDbContext? _dbContext;
        #endregion
        public LoginViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbContext = _serviceProvider.GetRequiredService<EmployeesDbContext>();
            _user = AuthUser.GetInstance().User;
            _isSave = Settings.Default.IsSave;
        }
        #region Properties
        private bool _isSave;
        public bool IsSave
        {
            get => _isSave;
            set => Set(ref _isSave, value);
        }
        private User? _user;
        public User? User
        {
            get => _user;
            set => Set(ref _user, value);
        }
        #endregion
        #region Commands
        private RelayCommand? _login;
        public RelayCommand Login => _login ??= new RelayCommand(async obj =>
        {
            if (obj != null && 
                obj is Window lw)
            {
                var authUser = await _dbContext!.Users!.Where(a => a.Login == User!.Login &&
                                                                   a.Password == User.Password)
                                                       .FirstOrDefaultAsync();
                if (authUser != null)
                {
                    EditSettings();
                    _user!.Password = null;
                    _user.IsAdmin = authUser.IsAdmin;
                    var mainWindow = _serviceProvider?.GetService<MainWindow>();
                    if (mainWindow != null)
                    {
                        mainWindow.Show();
                        Application.Current.MainWindow = mainWindow;
                    }
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
            return !string.IsNullOrEmpty(User?.Login) &&
                   !string.IsNullOrEmpty(User?.Password);
        });
        private RelayCommand? _inputPassword;
        public RelayCommand InputPassword
        {
            get => _inputPassword ??= new RelayCommand((obj) =>
            {
                if (obj != null && obj is PasswordBox pb)
                {
                    User!.Password = pb.Password;
                }
            });
        }
        #endregion
        #region Methods
        private void EditSettings()
        {
            if (IsSave)
            {
                Settings.Default.IsSave = IsSave;
                Settings.Default.Login = User!.Login;
                Settings.Default.Password = User.Password;//Под рукой нет рабочего шифровальщика и в задаче не стояло, поэтому оставил так
                Settings.Default.Save();

            }
            else
            {
                Settings.Default.IsSave = false;
                Settings.Default.Login = string.Empty;
                Settings.Default.Password = string.Empty;
                Settings.Default.Save();
            }
        }
        #region Dispose
        protected override void FreeObjects()
        {
            _serviceProvider = null;
            _dbContext = null;
            _user = null;
            _login = null;
        }
        #endregion
        #endregion
    }
}
