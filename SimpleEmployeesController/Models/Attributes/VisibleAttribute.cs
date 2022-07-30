using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController.Models.Attributes
{
    public class VisibleAttribute : Attribute
    {
        public bool Visible { get; set; }
        public VisibleAttribute(bool visible) => Visible = visible;

    }
}
