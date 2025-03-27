using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Certificate
    {
        public int Id { get; set; }
        public string TypeCerticicate { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }


    }
}
