using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class FormDto
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public string FormDescription { get; set; }
        public bool Hidden { get; set; }

    }
}
