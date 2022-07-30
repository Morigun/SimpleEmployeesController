using SimpleEmployeesController.MVVM;
using SimpleEmployeesController.Properties;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController.Models
{
    /// <summary>
    /// Одиночка содержит информацию о залогиненом пользователе
    /// </summary>
    internal class AuthUser : ObservableObject
    {
        private static readonly AuthUser instance = new AuthUser();
        internal User? User { get; private set; }
        private AuthUser()
        {
            User = new User
            {
                Login = Settings.Default.Login,
                Password = Settings.Default.Password
            };
        }
        public static AuthUser GetInstance() => instance;
        protected override void FreeObjects()
        {
            User = null;
        }
    }
}
