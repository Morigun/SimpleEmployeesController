using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController.Models
{
    /// <summary>
    /// Контекст БД
    /// Внимание:
    /// После добавления новых полей в класс Employee, необходимо закомментировать конструктор: 
    /// public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options) : base(options)
    /// и выполнить команды:
    /// Add-Migration [Имя миграции]
    /// Update-Database
    /// раскомментировать конструктор обратно
    /// </summary>
    public class EmployeesDbContext : DbContext
    {
        #region Properties
        public DbSet<Employee>? Employees { get; set; } = null;
        public DbSet<User>? Users { get; set; } = null;
        public string DbPath { get; }
        #endregion
        #region Constructors
        //Перед миграцией закомментировать этот конструктор
        /// <summary>
        /// Конструктор который передает строку подключения
        /// </summary>
        /// <param name="options">Опции с строкой подключения</param>
        public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var directory = Path.Join(path, CONST.PATH_TO_DB);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            DbPath = Path.Join(directory, "employees.db");
            Database.EnsureCreated();
        }
        /// <summary>
        /// Конструктор для миграции
        /// </summary>
        public EmployeesDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var directory = Path.Join(path, CONST.PATH_TO_DB);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            DbPath = Path.Join(directory, "employees.db");
        }
        #endregion
        #region Override methods
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(GetUsers());
            modelBuilder.Entity<Employee>().HasData(GetEmployees());
            base.OnModelCreating(modelBuilder);
        }
        #endregion
        #region Generate data for db methods
        private List<User> GetUsers()
        {
            return new List<User>
            {
                new User { UserId = 1, Login = "user", Password = "user" },
                new User { UserId = 2, Login = "admin", Password = "admin", IsAdmin = true }
            };
        }
        private List<Employee> GetEmployees()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SimpleEmployeesController.Resources.TestFio.csv";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream!))
                {
                    var lines = reader.ReadToEnd().Split(new[] { Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                    var result = lines.Skip(1).Select((line, i) => {
                        var items = line.Split(';');
                        return new Employee()
                        {
                            EmployeeId = i+1,
                            FirstName = items[1],
                            LastName = items[0],
                            MiddleName = items[2],
                        };
                    }).ToList();
                    return result;
                }
            }
        }
        #endregion
    }
}
