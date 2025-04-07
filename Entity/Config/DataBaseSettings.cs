using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Config
{
    public class DataBaseSettings
    {
        public string DataBaseType { get; set; }
        public string MySqlConnection { get; set; }
        public string SqlServerConnection { get; set; }
        public string PostgreSqlConnection { get; set; }
    }
}
