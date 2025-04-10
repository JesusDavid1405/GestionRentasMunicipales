﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Identification { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }

        public List<RolUser> RolUser { get; set; } = new List<RolUser>();

    }
}
