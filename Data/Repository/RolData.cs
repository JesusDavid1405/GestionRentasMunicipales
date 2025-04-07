using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Data.Contexts;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data.Repository
{
    /// <summary>
    /// Repositorio encargado de la getion de la entidad Rol en la base de datos
    /// </summary>
    public class RolData : IRolData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolData> _logger;

        /// <summary>
        /// Constructor que recibe recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Intancia de <see cref="ApplicationDbContext"/> para la conexión con la base de datos.</param>

        public RolData(ApplicationDbContext context, ILogger<RolData> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los roles almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de roles</returns>
        public async Task<IEnumerable<Rol>> GetAllRolAsyncSql()
        {
            string query = @"
                SELECT r.Id, r.Name, r.Active
                FROM Rol r
                WHERE r.Active = 1;
            ";

            return (IEnumerable<Rol>)await _context.QueryAsync<Rol>(query);
        }

        ///<summary>
        /// Obteniendo todos los roles almacenadis en la base de datos con Linq.
        /// </summary>
        public async Task<IEnumerable<Rol>> GetAllRolAsyncLinq()
        {
            try
            {
                return await _context.Set<Rol>()
                    .Where(r => !r.Active)
                    .Include(r => r.RolUser)
                    .ToListAsync();
                       
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los Roles");
                throw;
            }
        }

        /// <summary>
        /// Obtiene un rol especifico por su identificador en SQL
        /// </summary>
        /// 
        public async Task<Rol?> GetByIdRolAsyncSql(int id)
        {
            try
            {
                string query = @"
                    SELECT r.Id, r.Name, r.Active
                    FROM Rol r
                    WHERE r.Id = @Id AND r.Active = 1;
                ";

                var parameters = new { Id = id };

                return await _context.QueryFirstOrDefaultAsync<Rol>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rol con ID {RolId}", id);
                throw; //Re-lanza la excepcion para sea manejada en capas superiores
            }
        }

        ///<summary>
        /// Obtiene un rol especifico por su identificador en Linq
        /// </summary>
        /// 
        public async Task<Rol?> GetRolByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<Rol>()
                    .FirstOrDefaultAsync(r => r.Id == id && !r.Active);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el usuario con ID {id}");
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo rol en la base de datos con sql
        /// </summary>
        /// 
        public async Task<Rol> CreateRolAsyncSql(Rol rol)
        {
            try
            {
                string query = @"
                    INSERT INTO Rol (Name, Active)
                    OUTPUT INSERTED.Id
                    VALUES (@Name, @Active);
                ";

                var parameters = new 
                {
                    Name = rol.Name,
                    Active = rol.Active
                };

                rol.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                return rol;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el rol: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Actualiza un rol existente en la base de datos SQL
        /// </summary>
        ///
        public async Task<bool> UpdateRolAsyncSql(Rol rol)
        {
            try
            {
                string query = @"
                    UPDATE [Rol]
                    SET 
                    Name = @Name,
                    Active = @Active
                    WHERE Id = @Id;
                ";

                var parameters = new
                {
                    Id = rol.Id,
                    Name = rol.Name,
                    Active = rol.Active
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el rol: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Eliminacion de un rol de forma logica en la base de datos SQL.
        /// </summary>
        ///

        public async Task<bool> DeleteRolLogicalAsyncSql(int id)
        {
            try
            {
                string query = "UPDATE Rol SET Active = 0 WHERE Id = @Id;";

                var parameters = new { Id = id };

                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar rol: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Eliminacion de un rol de forma persistente en la base de datos SQL.
        /// </summary>
        ///
        public async Task<bool> DeleteRolPersistentAsyncSql(int id)
        {
            try
            {
                string query = "DELETE FROM Rol WHERE Id = @Id;";

                var parameters = new { Id = id };

                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Eror al eliminar el Rol: {id}", ex);
                return false;
            }
        }
    }
}
