using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public int UserIdentification { get; set; }
        public int Telephone { get; set; }
        public string UserAddress { get; set; }
        public bool Hidden { get; set; }
    }
}
