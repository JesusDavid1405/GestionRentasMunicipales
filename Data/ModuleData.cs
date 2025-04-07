using Data.Contexts;
using Entity.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ModuleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModuleData> _logger;

        //contructor para inyectar el contexto de la base de datos y el logger
        public ModuleData(ApplicationDbContext context, ILogger<ModuleData> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los modulos almacenados en la base de datos.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Module>> GetAllModuleAsync()
        {
            string query = @"
                SELECT m.Id, m.Name, m.Description, m.Code
                FROM Module m
                WHERE m.IsDeleted = 0;
            ";  

            return (IEnumerable<Module>) await _context.QueryAsync<Module>(query);
        }

        /// <summary>
        /// Obtiene un modulo por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Module?> GetByIdModuleAsync(int id)
        {
            try
            {
                string query = @"
                    SELECT m.Id, m.Name, m.Description, m.Code
                    FROM Module m
                    WHERE m.IsDeleted = 0 AND m.Id = @Id;
                ";

                var parameters = new
                {
                    Id = id
                };

                return await _context.QueryFirstOrDefaultAsync<Module>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Modulo con ID {ModuleId}", id);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo modulo en la base de datos.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public async Task<Module> CreateModuleAsync(Module module)
        {
            try
            {
                string query = @"
                    INSERT INTO Module (Name, Description, Code, IsDeleted)
                    OUTPUT INSERTED.Id
                    VALUES (@Name, @Description, @Code, 0);
                ";

                var parameters = new
                {
                    Name = module.Name,
                    Description = module.Description,
                    Code = module.Code,
                };

                module.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                return module;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el Modulo: {ex.Message}");
                throw;
            }
        }

        //Metodo con LinQ para obtener los modulos
        public async Task<bool> UpdateModuleAsync(Module module)
        {
            try
            {
                string query = @"
                    UPDATE Module
                    SET 
                    Name = @Name, 
                    Description = @Description, 
                    Code = @Code
                    WHERE Id = @Id;
                ";

                var parameters = new
                {
                    Name = module.Name,
                    Description = module.Description,
                    Code = module.Code,
                    Id = module.Id
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el modulo con sus permisos: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeletePersistentModuleAsync(int id)
        {
            try
            {
                string query = @"
                    DELETE FROM Module
                    WHERE Id = @Id;
                ";

                var parameters = new
                {
                    Id = id
                };

                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el modulo{ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteLogicalModuleAsync(int id)
        {
            try
            {
                string query = @"
                    UPDATE Module
                    SET IsDeleted = 1
                    WHERE Id = @Id;
                ";
                var parameters = new
                {
                    Id = id
                };

                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el modulo: {id}", ex);
                return false;
            }
        }
    }
}
