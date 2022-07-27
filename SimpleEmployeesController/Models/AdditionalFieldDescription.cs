using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController.Models
{
    public class AdditionalFieldDescription : ObservableObject
    {
        public int AdditionalFieldDescriptionId { get; set; }
        public string DisplayName { get; set; }
    }
}
