using Data.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
    public class UserData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserData> _logger;

        //contructor que recibe el contexto de base de datos
        public UserData(ApplicationDbContext context, ILogger<UserData> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los usuarios almacenados en la base de datos.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            string query = @"
                SELECT 
				u.Id, 
				u.Name, 
				u.LastName, 
				u.Email, 
				u.Password, 
				u.Identification, 
				u.Phone, 
				u.Address, 
				u.IsDeleted
                FROM [User] u
                WHERE u.IsDeleted = 0;
            ";

            return (IEnumerable<User>)await _context.QueryAsync<User>(query);
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User?> GetByIdUserAsync(int id)
        {
            try
            {
                string query = @"
                    SELECT 
                    u.Id, 
				    u.Name, 
				    u.LastName, 
				    u.Email, 
				    u.Password, 
				    u.Identification, 
				    u.Phone, 
				    u.Address, 
				    u.IsDeleted
                    FROM [User] u
                    WHERE u.IsDeleted = 0 AND u.Id = @Id;
                ";

                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<User>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario con ID {UserId}", id);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                string query = @"
                    INSERT INTO [User] (Name, LastName, Email, Password, Identification, Phone, Address, IsDeleted)
                    OUTPUT INSERTED.Id
                    VALUES (@Name, @LastName, @Email, @Password, @Identification, @Phone, @Address, 0);
                ";
                
                var parameters = new
                {
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    Identification = user.Identification,
                    Phone = user.Phone,
                    Address = user.Address
                };

                user.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro al crear el usuario: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Actualiza un usuario existente en la base de datos.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                string query = @"
                    UPDATE [User]
                    SET 
                    Name = @Name, 
                    LastName = @LastName, 
                    Email = @Email, 
                    Password = @Password, 
                    Identification = @Identification, 
                    Phone = @Phone, 
                    Address = @Address
                    WHERE Id = @Id;
                ";
                
                var parameters = new
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    Identification = user.Identification,
                    Phone = user.Phone,
                    Address = user.Address
                };

                var rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Elimina un usuario de forma persistente.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeletePersistentUserAsync(int id)
        {
            try
            {
               string query = @"
                    DELETE FROM [User]
                    WHERE Id = @Id;
               "
;
                var parameters = new { Id = id };

                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteLogicalUserAsync(int id)
        {
            try
            {
                string query = @"
                    UPDATE [User]
                    SET IsDeleted = 1
                    WHERE Id = @Id;
                ";
                var parameters = new { Id = id };
                var rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
                return false;
            }
        }
    }
}
