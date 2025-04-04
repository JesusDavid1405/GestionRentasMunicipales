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
    public class ModuleFormData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModuleFormData> _logger;

        public ModuleFormData(ApplicationDbContext context, ILogger<ModuleFormData> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todo los Module Form existentes
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModuleForm>> GetAllModuleFormAsync()
        {
            string query = @"
                SELECT
                mf.Id,
                mf.FormId,
                f.Name as FormName,
                mf.ModuleId,
                m.Name as ModuleName
                FROM ModuleForm mf
                INNER JOIN Form f ON mf.FormId = f.Id
                INNER JOIN Module m ON mf.ModuleId = m.Id
            ";

            return await _context.QueryAsync<ModuleForm>(query);
        }

        /// <summary>
        /// Obtener los ModuleForm que existan con un Id en expecificos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ModuleForm?> GetByIdModuleFormAsync(int id)
        {
            try
            {
                string query = @"
                    SELECT
                    mf.Id,
                    mf.FormId,
                    f.Name as FormName,
                    mf.ModuleId,
                    m.Name as ModuleName
                    FROM ModuleForm mf
                    INNER JOIN Form f ON mf.FormId = f.Id
                    INNER JOIN Module m ON mf.ModuleId = m.Id
                 	WHERE mf.Id = 1;
                ";

                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<ModuleForm>(query,parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Modulo con sus fomularios con ID {ModuleFormId}", id);
                throw;
            }
        }

        /// <summary>
        /// Crear un ModuleForm
        /// </summary>
        /// <param name="moduleForm"></param>
        /// <returns></returns>
        public async Task<ModuleForm> CreateModuleFormAsync(ModuleForm moduleForm)
        {
            try
            {
                string query = @"
                    INSERT INTO ModuleForm (FormId, ModuleId)
                    OUTPUT INSERTED.Id
                    VALUES (@FormId, @ModuleId);
                ";

                var parameters = new
                {
                    FormId = moduleForm.FormId,
                    ModuleId = moduleForm.ModuleId,
                };

                moduleForm.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                return moduleForm;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el Modulo con sus permisos: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Actualizar un ModuleForm
        /// </summary>
        /// <param name="moduleForm"></param>
        /// <returns></returns>
        public async Task<bool> UpdateModuleFormAsync(ModuleForm moduleForm)
        {
            try
            {
                string query = @"
                    UPDATE ModuleForm
                    SET
                    FormId = @FormId,
                    ModuleId = @ModuleId
                    WHERE Id = @Id;
                ";

                var parameters = new
                {
                    Id = moduleForm.Id,
                    FormId = moduleForm.FormId,
                    ModuleId = moduleForm.ModuleId,
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el modulo con sus permisos: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Elimando un ModuleForm de forma Persistente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteModuleFormAsync(int id)
        {
            try
            {
                string query = @"
                    DELETE FROM ModuleForm
                    WHERE Id = @Id;
                ";

                var parameters = new { Id = id };

                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el modulo con sus permisos {ex.Message}");
                return false;
            }
        }
    }
}
