using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
    /// <summary>
    /// Repositorio encargado de la getion de la entidad Rol en la base de datos
    /// </summary>
    public class RolData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor que recibe recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Intancia de <see cref="ApplicationDbContext"/> para la conexión con la base de datos.</param>

        public RolData(ApplicationDbContext context, ILogger logger)
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
                SELECT r.Id, r.Name
                FROM Rol r
                WHERE r.IsDeleted = 0;
            ";

            return await _context.QueryAsync<Rol>(query);
        }

        /// <summary>
        /// Obtiene un rol especifico por su identificador
        /// </summary>
        public async Task<IEnumerable<Rol>> GetByIdRolAsync(int id)
        {
            try
            {
                string query = @"
                    SELECT r.Id, r.Name
                    FROM Rol r
                    WHERE r.Id = @Id AND r.IsDeleted = 0;
                ";

                var parameters = new { Id = id };
                return await _context.QueryAsync<Rol>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rol con ID {RolId}", id);
                throw; //Re-lanza la excepcion para sea manejada en capas superiores
            }
        }
        /// <summary>
        /// Crea un nuevo rol en la base de datos
        /// </summary>
        /// <param name="rol"></param>
        /// <returns>el rol creado.</returns>
        /// 
        public async Task<IEnumerable<Rol>> CreateAsync(Rol rol)
        {
            try
            {
                string query = @"
                    INSERT INTO Rol (Id, Name)
                    OUTPUT INSERTED.Id
                    VALUES (@Id, @Name);
                ";
                var parameters = new {
                    rol.Id,
                    rol.Name
                };
                await _context.QueryAsync<Rol>(query);
                await _context.SaveChangesAsync();
                return rol;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el rol: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Actualiza un rol existente en la base de datos
        /// </summary>
        /// <param name="rol">Objeto con la informacion actualizada</param>
        /// <returns>True si la operacion fue exitosa, False en caso contrario</returns>
        public async Task<bool> UpdateAsync(Rol rol)
        {
            try
            {
                _context.Set<Rol>().Update(rol);
                await _context.SaveChagesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el rol: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var rol = await _context.Set<Rol>().FindAsync(id);
                if (rol == null)
                    return false;
                _context.Set<Rol>().Remove(rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar rol: {ex.Message}");
                return false;
            }
        }
    }
}
