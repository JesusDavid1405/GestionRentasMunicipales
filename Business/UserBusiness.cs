
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
        private readonly ILogger _logger;

        public UserBusiness(UserData userData, ILogger logger)
        {
            _userData = userData;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userData.GetAllAsync();
                var userDto = new List<UserDto>();

                foreach (var user in users)
                {
                    userDto.Add(new UserDto
                    {
                        UserId = user.Id,
                        UserName = user.Name
                    });
                }
                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos lo usuarios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios", ex);
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            if (id < 0)
            {

                _logger.LogWarning("Se intento obtener un usuario con ID invalido: {UsuarioId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del usuario debe ser mayor que cero");
            }
            try
            {
                var user = await _userData.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogInformation("No se encontró ningún usuario con ID: {UserId}", id);
                    throw new EntityNotFoundException("Usuario", id);
                }

                return new UserDto
                {
                    UserId = user.Id,
                    UserName = user.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}", ex);
            }
        }
        public async Task<UserDto> CreateUsersAsync(UserDto UserDto)
        {
            try
            {
                ValidateRol(UserDto);
                var user = new User
                {
                    Name = UserDto.UserName
                };
                var UserCreado = await _userData.CreateAsync(user);
                return new UserDto
                {
                    UserId = UserCreado.Id,
                    UserName = UserCreado.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo usuario: {UsuarioNombre}", UserDto?.UserName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario", ex);
            }
        }
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
    }
}
