using Data.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
    /// <summary>
    /// Repositorio encargado de la getion de la entidad Rol en la base de datos
    /// </summary>
    public class PermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionData> _logger;

        //Contructor que recibe el contexto de base de datos
        public PermissionData(ApplicationDbContext context, ILogger<PermissionData> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los Permission almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de roles</returns>
        public async Task<IEnumerable<Permission>> GetAllPermissionAsync()
        {
            string query = @"
                SELECT per.Id, per.Name, per.Description, per.IsDeleted
                FROM Permission per
                WHERE per.IsDeleted = 0;
            ";

            return (IEnumerable<Permission>)await _context.QueryAsync<Permission>(query);
        }

        /// <summary>
        /// Obtiene todos los Permission con base a un Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Permission?> GetByIdPermissionAsync(int id)
        {
            try
            {
                string query = @"
                    SELECT per.Id, per.Name, per.Description, per.IsDeleted
                    FROM Permission per
                    WHERE per.IsDeleted = 0 AND per.Id = @Id;
                ";

                var parameters = new { Id = id };

                return await _context.QueryFirstOrDefaultAsync<Permission>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID {PermissioniD}", id);
                throw;
            }
        }

        /// <summary>
        /// Crea un permission en la base de datos
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public async Task<Permission> CreatePermissionAsync(Permission permission)
        {
            try
            {
                string query = @"
                    INSERT INTO Permission (Name,Description,IsDeleted)
                    OUTPUT INSERTED.Id
                    VALUES (@Name, @Description, 0);
                ";

                var parameters = new
                {
                    Name = permission.Name,
                    Description = permission.Description,
                };

                permission.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                return permission;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el permiso: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdatePermissionAsync(Permission permission)
        {
            try
            {
                string query = @"
                    UPDATE [Permission]
                    SET 
                    Name = @Name, 
                    Description = @Description
                    WHERE Id = @Id;
                ";
                
                var parameters = new
                {
                    Id = permission.Id,
                    Name = permission.Name,
                    Description = permission.Description,
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erroe al actualizar el permiso: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Elimina permamentemente un permission de la base de datos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeletePersistenteAsync(int id)
        {
            try
            {
                string query = @"
                    DELETE FROM Permission
                    WHERE Id = @Id;
                ";
                var parameters = new { Id = id };

                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Eror al eliminar el Permission: {id}", ex);
                return false;
            }
        }

        /// <summary>
        /// Elimina un permission de forma logica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                string query = @"
                    UPDATE Permission
                    SET IsDeleted = 1
                    WHERE Id = @Id;
                ";
                var parameters = new { Id = id };
                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el Permission: {id}", ex);
                return false;
            }
        }
    }
}
