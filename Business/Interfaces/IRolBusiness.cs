using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs;

namespace Business.Interfaces
{
    public interface IRolBusiness
    {
        Task<IEnumerable<RolDto>> GetAllRolesAsync();
        Task<RolDto> GetRolByIdAsync(int id);
        Task<RolDto> CreateRolAsync(RolDto rolDto);
        Task<bool> UpdateRolAsync(RolDto rolDto);
        Task<bool> DeletePersistentRolAsync(int id);
        Task<bool> DeleteLogicalRolAsync(int id);
    }
}
