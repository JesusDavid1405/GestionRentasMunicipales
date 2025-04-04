
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;


namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class UserBusiness
    {
        private readonly UserData _userData;
        private readonly ILogger<UserBusiness> _logger;

        public UserBusiness(UserData userData, ILogger<UserBusiness> logger)
        {
            _userData = userData;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
               var users = await _userData.GetAllUserAsync();
               return MapToDTOList(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos lo usuarios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios", ex);
            }
        }

        /// <summary>
        /// Método para obtener un usuario por ID como DTO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Utilities.Exceptions.ValidationException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            if (id < 0)
            {
                _logger.LogWarning("Se intento obtener un usuario con ID invalido: {UsuarioId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del usuario debe ser mayor que cero");
            }
            try
            {
                var user = await _userData.GetByIdUserAsync(id);
                if (user == null)
                {
                    _logger.LogInformation("No se encontró ningún usuario con ID: {UserId}", id);
                    throw new EntityNotFoundException("Usuario", id);
                }

                return MapToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}", ex);
            }
        }

        /// <summary>
        /// Método para crear un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="UserDto"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<UserDto> CreateUsersAsync(UserDto UserDto)
        {
            try
            {
                ValidateRol(UserDto);

                var user = MapToEntity(UserDto);

                var UserCreado = await _userData.CreateUserAsync(user);

                return MapToDTO(UserCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo usuario: {UsuarioNombre}", UserDto?.UserName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario", ex);
            }
        }   

        /// <summary>
        /// Método para actualizar un usuario en la base de datos.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            try
            {
                ValidateRol(userDto);

                var existingUser = await _userData.GetByIdUserAsync(userDto.UserId);
                if (existingUser == null)
                {
                    throw new EntityNotFoundException("Usuario", userDto.UserId);
                }

                existingUser.Name = userDto.UserName;
                existingUser.LastName = userDto.UserLastName;
                existingUser.Email = userDto.UserEmail;
                existingUser.Password = userDto.UserPassword;
                existingUser.Identification = userDto.UserIdentification;
                existingUser.Phone = userDto.Telephone;
                existingUser.Address = userDto.UserAddress;
                existingUser.IsDeleted = userDto.Hidden;

                return await _userData.UpdateUserAsync(existingUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso: {UserName}", userDto?.UserName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el permiso", ex);
            }
        }

        /// <summary>
        /// Método para eliminar un usuario de forma persistente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Utilities.Exceptions.ValidationException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<bool> DeletePersistentUserAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar un User con ID inválido: {UserId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del user debe ser mayor que cero");
                }
                var user = await _userData.GetByIdUserAsync(id);
                if (user == null)
                {
                    _logger.LogInformation("No se encontró ningún user con ID: {UserId}", id);
                    throw new EntityNotFoundException("User", id);
                }
                return await _userData.DeletePersistentUserAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el user con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el user con ID {id}", ex);
            }
        }


        /// <summary>
        /// Método para eliminar un user de forma lógica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<bool> DeleteLogicalPermissionAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar un user con ID inválido: {UserId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del user debe ser mayor que cero");
                }
                var user = await _userData.GetByIdUserAsync(id);
                if (user == null)
                {
                    _logger.LogInformation("No se encontró ningún user con ID: {UserId}", id);
                    throw new EntityNotFoundException("User", id);
                }
                return await _userData.DeleteLogicalUserAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el user con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el user con ID {id}", ex);
            }
        }


        /// <summary>
        /// Validacion de usuario para saber si el Dto esta vacio
        /// </summary>
        /// <param name="UserDto"></param>
        /// <exception cref="Utilities.Exceptions.ValidationException"></exception>
        private void ValidateRol(UserDto UserDto)
        {
            if (UserDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(UserDto.UserName))
            {
                _logger.LogWarning("Se intento crear/actualucar un rol con Name vacio");
                throw new Utilities.Exceptions.ValidationException("Name", "El name del rol es obligatorio");
            }
        }

        // Método para mapear de Rol a RolDTO
        private UserDto MapToDTO(User user)
        {
            return new UserDto
            {
                UserId = user.Id,
                UserName = user.Name,
                UserLastName = user.LastName,
                UserEmail = user.Email,
                UserPassword = user.Password,
                UserIdentification = user.Identification,
                Telephone = user.Phone,
                Hidden = user.IsDeleted,
            };
        }

        // Método para mapear de RolDTO a Rol
        private User MapToEntity(UserDto userDto)
        {
            return new User
            {
                Id = userDto.UserId,
                Name = userDto.UserName,
                LastName = userDto.UserLastName,
                Email = userDto.UserEmail,
                Password = userDto.UserPassword,
                Identification = userDto.UserIdentification,
                Phone = userDto.Telephone,
                Address = userDto.UserAddress,
                IsDeleted = userDto.Hidden,
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<UserDto> MapToDTOList(IEnumerable<User> users)
        {
            return users.Select(MapToDTO).ToList();
        }
    }
}
