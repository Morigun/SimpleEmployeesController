using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController.Models
{
    public class EmployeeAdditionalFields : ObservableObject
    {
        public int EmployeeAdditionalFieldsId { get; set; }
        public int EmployeeID { get; set; }
        private AdditionalFieldDescription _additionalFieldDescription;
        public AdditionalFieldDescription AdditionalFieldDescription
        {
            get => _additionalFieldDescription;
            set => Set(ref _additionalFieldDescription, value);
        }
        private string _value;
        public string Value { get => _value; set => Set(ref _value, value); }
    }
}
