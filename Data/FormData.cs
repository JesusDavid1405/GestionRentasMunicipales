using Data.Contexts;
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
    public class FormData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormData> _logger;

        /// <summary>
        /// Constructor que recibe recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Intancia de <see cref="ApplicationDbContext"/> para la conexión con la base de datos.</param>

        public FormData(ApplicationDbContext context, ILogger<FormData> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los formularios almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de formularios</returns>
        public async Task<IEnumerable<Form>> GetAllFormAsync()
        {
            string query = @"
                SELECT f.Id, f.Name, f.Description, f.IsDeleted
                FROM Form f
                WHERE f.IsDeleted = 0;
            ";

            return (IEnumerable<Form>)await _context.QueryAsync<Form>(query);
            
        }

        /// <summary>
        /// Obtiene un formulario especifico por su identificador
        /// </summary>
        public async Task<Form?> GetByIdFormAsync(int id)
        {
            try
            {
                string query = @"
                    SELECT f.Id, f.Name, f.Description, f.IsDeleted
                    FROM Form f
                    WHERE f.IsDeleted = 0 AND f.Id = @Id;
                ";

                var parameters = new { Id = id };

                return await _context.QueryFirstOrDefaultAsync<Form>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formulario con ID {FormId}", id);
                throw; //Re-lanza la excepcion para sea manejada en capas superiores
            }
        }

        /// <summary>
        /// Crea un nuevo formulario en la base de datos
        /// </summary>
        /// <param name="form"></param>
        /// <returns>el formulario creado.</returns>
        /// 
        public async Task<Form> CreateFormAsync(Form form)
        {
            try
            {
                string query = @"
                    INSERT INTO Form (Name,Description,IsDeleted)
                    OUTPUT INSERTED.Id
                    VALUES (@Name, @Description, 0);
                ";

                var parameters = new
                {
                    Name = form.Name,
                    Description = form.Description,
                };

                form.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                return form;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el formulario: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Actualiza un formulario existente en la base de datos
        /// </summary>
        /// <param name="form">Objeto con la informacion actualizada</param>
        /// <returns>True si la operacion fue exitosa, False en caso contrario</returns>
        public async Task<bool> UpdateFormAsync(Form form)
        {
            try
            {
                string query = @"
                    UPDATE [Form]
                    SET 
                    Name = @Name, 
                    Description = @Description
                    WHERE Id = @Id;
                ";

                var parameters = new
                {
                    Id = form.Id,
                    Name = form.Name,
                    Description = form.Description
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el formulario: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Eliminando un Form de forma Persistente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeletePersistentFormAsync(int id)
        {
            try
            {
                string query = @"
                    DELETE FROM Form
                    WHERE Id = @Id;
                ";

                var parameters = new { Id = id };

                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar formulario: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Eliminando un Form de forma logica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteLogicalFormAsync(int id)
        {
            try
            {
                string query = @"
                    UPDATE Form
                    SET IsDeleted = 1
                    WHERE Id = @Id;
                ";
                var parameters = new { Id = id };
                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el Form: {id}", ex);
                return false;
            }
        }
    }
}
