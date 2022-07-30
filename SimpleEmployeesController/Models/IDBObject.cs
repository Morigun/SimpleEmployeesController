using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController.Models
{
    public interface IDBObject
    {
        void SetDBContext(DbContext dbContext);
        DbContext? GetDBContext();
        Task UpdateInDBAsync();
        Task<bool> InsertInDBAsync();
    }
}
