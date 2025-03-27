using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Debt
    {
        public int Id { get; set; }
        public DateTime DebtDate { get; set; }
        public float GrossValue { get; set; }
        public float IvaValue { get; set; }
        public float DebtTotal { get; set; }
    }
}
