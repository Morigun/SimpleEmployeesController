using Microsoft.EntityFrameworkCore;

using SimpleEmployeesController.Models.Attributes;
using SimpleEmployeesController.MVVM;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController.Models
{
    /// <summary>
    /// Класс сотрудников
    /// После добавления новых полей, необходимо закомментировать конструктор: 
    /// public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options) : base(options)
    /// и выполнить команды:
    /// Add-Migration [Имя миграции]
    /// Update-Database
    /// раскомментировать конструктор обратно
    /// </summary>
    public class Employee : ObservableObject, IDBObject
    {
        DbContext? dbContext { get; set; }
        bool inDB = false;
        #region Constructors
        //Конструктор для внедрения
        public Employee(EmployeesDbContext dbContext)
        {
            this.dbContext = dbContext;
            inDB = true;
        }
        //Конструктор для начальной инициализации
        public Employee()
        {
            inDB = false;
        }
        #endregion
        #region Properties
        [Visible(false)]
        public int EmployeeId { get; set; }
        private string? _firstName;
        [DisplayName("Имя")]
        public string? FirstName
        {
            get => _firstName;
            set
            {
                TrySetFirstName(value).Wait();
            }
        }
        private string? _lastName;
        [DisplayName("Фамилия")]
        public string? LastName
        {
            get => _lastName;
            set
            {
                TrySetLastName(value).Wait();
            }
        }
        private string? _middleName;
        [DisplayName("Отчество")]
        public string? MiddleName
        {
            get => _middleName;
            set
            {
                TrySetMiddleName(value).Wait();
            }
        }
        #endregion
        #region Methods
        #region Dispose
        /// <summary>
        /// Срабатывает при освобожении памяти объектом(при вызове метода Dispose)
        /// </summary>
        protected override void FreeObjects()
        {
            _firstName = null;
            _lastName = null;
        }
        #endregion
        /// <summary>
        /// Метод, для попытки установить новое имя, 
        /// проверяет, если с новым именем ФИО текущего сотрудника теряет уникальность, 
        /// то не дает изменить имя
        /// </summary>
        /// <param name="value">Новое значение для имени</param>
        /// <returns></returns>
        private async Task TrySetFirstName(string? value)
        {
            var newValue = value.ToFirstUpLetter();
            if (await CheckNewEmployee(newValue, this._lastName, this._middleName))
                Set(ref _firstName, newValue, nameof(FirstName));
        }
        /// <summary>
        /// Метод, для попытки установить новую фамилию, 
        /// проверяет, если с новым именем ФИО текущего сотрудника теряет уникальность, 
        /// то не дает изменить фамилию
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task TrySetLastName(string? value)
        {
            var newValue = value.ToFirstUpLetter();
            if (await CheckNewEmployee(this._firstName, newValue, this._middleName))
                Set(ref _lastName, newValue, nameof(LastName));
        }
        /// <summary>
        /// Метод, для попытки установить новое отчество, 
        /// проверяет, если с новым именем ФИО текущего сотрудника теряет уникальность, 
        /// то не дает изменить отчество
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task TrySetMiddleName(string? value)
        {
            var newValue = value.ToFirstUpLetter();
            if (await CheckNewEmployee(this._firstName, this._lastName, newValue))
                Set(ref _middleName, newValue, nameof(MiddleName));
        }
        /// <summary>
        /// Метод для обновления Сотрудников в базе,
        /// вызывается при изменении полей,
        /// которые сохраняют значение через метод Set
        /// </summary>
        public async override void OnNotification()
        {
            await UpdateInDBAsync();
        }
        #region DB
        public bool CheckInDB() => inDB;
        public void SetDBContext(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public DbContext? GetDBContext()
        {
            return dbContext;
        }
        public async Task UpdateInDBAsync()
        {
            if (dbContext != null)
            {
                dbContext.Update(this);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task<bool> InsertInDBAsync()
        {
            if (dbContext != null)
            {
                if (await CheckNewEmployee(this))
                {
                    await dbContext.AddAsync(this);
                    await dbContext.SaveChangesAsync();
                    inDB = true;
                    return true;
                }
            }
            return false;
        }
        private async Task<bool> CheckNewEmployee(Employee employee)
        {
            if (employee == null)
                return false;
            var findedEmployee = await dbContext!.Set<Employee>()
                                                 .FirstOrDefaultAsync(dbEmployee => dbEmployee.FirstName == employee.FirstName &&
                                                                                    dbEmployee.LastName == employee.LastName &&
                                                                                    dbEmployee.MiddleName == employee.MiddleName);

            if (findedEmployee == null)
                return true;
            return false;
        }
        private async Task<bool> CheckNewEmployee(string? firstName, string? lastName, string? middleName)
        {
            if (dbContext == null) return true;
            var findedEmployee = await dbContext!.Set<Employee>()
                                                 .FirstOrDefaultAsync(dbEmployee => dbEmployee.FirstName == firstName &&
                                                                                    dbEmployee.LastName == lastName &&
                                                                                    dbEmployee.MiddleName == middleName);
            if (findedEmployee == null)
                return true;
            return false;
        }
        #endregion
        #endregion
    }
}
