using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Config;
using Microsoft.EntityFrameworkCore;


namespace Data.Factory
{
    public static class DbContextConfigurator
    {
        public static void Configure(DbContextOptionsBuilder options, DataBaseSettings settings)
        {
            switch (settings.DataBaseType.ToLower())
            {
                case "mysql":
                    options.UseMySql(settings.MySqlConnection, ServerVersion.AutoDetect(settings.MySqlConnection));
                    break;
                case "sqlserver":
                    options.UseSqlServer(settings.SqlServerConnection);
                    break;
                case "postgresql":
                    options.UseNpgsql(settings.PostgreSqlConnection);
                    break;
                default:
                    throw new Exception("Motor no soportado");
            }
        }
    }

}
