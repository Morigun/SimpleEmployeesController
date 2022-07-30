using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController.ViewModel
{
    public interface IMainViewModel
    {
        int LastId { get; }
        bool IsAdmin { get; }
        List<PropertyInfo> GetPropertiesForGenerationColumns();
    }
}
