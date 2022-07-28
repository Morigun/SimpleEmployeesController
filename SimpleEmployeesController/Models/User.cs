using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController.Models
{
    public class User : ObservableObject
    {
        public int UserId { get; set; }
        private string _login;
        public string Login { get => _login; set => Set(ref _login, value); }
        private string _password;
        public string Password { get => _password; set => Set(ref _password, value); }
        private bool _isAdmin;
        public bool IsAdmin { get => _isAdmin; set => Set(ref _isAdmin, value); }
    }
}
