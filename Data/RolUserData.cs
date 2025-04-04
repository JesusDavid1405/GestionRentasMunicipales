using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class RolUserData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolUserData> _logger;

        public RolUserData(ApplicationDbContext context, ILogger<RolUserData> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todo los RolUser existentes 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RolUser>> GetAllRolUserAsync()
        {
            string query = @"
                SELECT
                ru.Id,
                ru.RolId,
                r.Name AS RolName,
                ru.UserId,
                u.Name AS UserName,
                u.LastName AS LastName
                FROM RolUser ru
                INNER JOIN [User] u ON ru.UserId = u.Id
                INNER JOIN Rol r ON ru.RolId = r.Id;
            ";

            return await _context.QueryAsync<RolUser>(query);
        }

        /// <summary>
        /// Obtener un RolUser que contenga un Id expecifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RolUser?> GetByIdRolUserAsync(int id)
        {
            try
            {
                string query = @"
                    SELECT
                    ru.Id,
                    ru.RolId,
                    r.Name AS RolName,
                    ru.UserId,
                    u.Name AS UserName,
                    u.LastName AS LastName
                    FROM RolUser ru
                    INNER JOIN [User] u ON ru.UserId = u.Id
                    INNER JOIN Rol r ON ru.RolId = r.Id;
                ";

                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<RolUser>(query,parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario con su rol con ID {UserRolId}", id);
                throw;
            }
        }

        /// <summary>
        /// Crear un RolUser
        /// </summary>
        /// <param name="rolUser"></param>
        /// <returns></returns>
        public async Task<RolUser> CreateRolUserAsync(RolUser rolUser)
        {
            try
            {
                string query = @"
                    INSERT INTO RolUser (RolId, UserId)
                    OUTPUT  INSERTED.Id
                    VALUES (@RolId, @UserId);
                ";

                var parameters = new
                {
                    RolId = rolUser.RolId,
                    UserId = rolUser.UserId
                };

                rolUser.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                return rolUser;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro al crear el usuario con su rol: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateRolUserAsync(RolUser rolUser)
        {
            try
            {
                string query = @"
                    UPDATE rolUser
                    SET
                    RolId = @RolId,
                    UserId = @UserId
                    WHERE Id = @Id;
                ";

                var parameters = new
                {
                    Id = rolUser.Id,
                    RolId = rolUser.RolId,
                    UserId = rolUser.UserId
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario con su rol: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteRolUserAsync(int id)
        {
            try
            {
                string query = @"
                    DELETE FROM RolUser
                    WHERE Id = @Id;
                ";

                var parameters = new { Id = id };

                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el usuario con su rol: {ex.Message}");
                return false;
            }
        }
    }
}
