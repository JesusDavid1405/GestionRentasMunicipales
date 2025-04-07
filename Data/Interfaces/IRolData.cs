using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;

namespace Data.Interfaces
{
    public interface IRolData
    {
        Task<IEnumerable<Rol>> GetAllRolAsyncSql();
        Task<Rol?> GetByIdRolAsyncSql(int id);
        Task<Rol> CreateRolAsyncSql(Rol rol);
        Task<bool> UpdateRolAsyncSql(Rol rol);
        Task<bool> DeleteRolLogicalAsyncSql(int id);
        Task<bool> DeleteRolPersistentAsyncSql(int id);
    }
}
