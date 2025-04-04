using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ModuleFormDto
    {
        public int ModuleFormId { get; set; }
        public int ModuleId { get; set; }
        public string ? ModuleName { get; set; }
        public int FormId { get; set; }
        public string ? FormName { get; set; }
    }
}
