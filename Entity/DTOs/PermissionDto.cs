using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
        public bool Hidden { get; set; }
    }
}
