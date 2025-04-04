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
    public class RolUserBusiness
    {
        private readonly RolUserData _rolUserData;
        private readonly ILogger<RolUserBusiness> _logger;

        public RolUserBusiness(RolUserData rolUserData, ILogger<RolUserBusiness> logger)
        {
            _rolUserData = rolUserData;
            _logger = logger;
        }

        /// <summary>
        /// Metodo para obtener todos los ModuleForm desde Dto
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<IEnumerable<RolUserDto>> GetAllUsersAsync()
        {
            try
            {
                var rolUser = await _rolUserData.GetAllRolUserAsync();
                return MapToDTOList(rolUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Utilities.Exceptions.ValidationException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<RolUserDto> GetRolUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un RolUser con ID inválido: {RolUserId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del RolUser debe ser mayor que cero");
            }
            try
            {
                var rolUser = await _rolUserData.GetByIdRolUserAsync(id);
                if (rolUser == null)
                {
                    _logger.LogInformation("No se encontró ningún RolUser con ID: {RolUserId}", id);
                    throw new EntityNotFoundException("RolUser", id);
                }

                return MapToDTO(rolUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}", ex);
            }
        }

        public async Task<RolUserDto> CreateRolUsersAsync(RolUserDto RolUserDto)
        {
            try
            {
                ValidateRolUser(RolUserDto);

                var rolsuser = MapToEntity(RolUserDto);

                var RolUserCreado = await _rolUserData.CreateRolUserAsync(rolsuser);

                return MapToDTO(RolUserCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo usuario: {UsuarioId}", RolUserDto?.UserId ?? 0);
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario", ex);
            }
        }

        public async Task<bool> UpdateModuleFormAsync(RolUserDto RolUserDto)
        {
            try
            {
                ValidateRolUser(RolUserDto);

                var rolUser = MapToEntity(RolUserDto);

                var existigRolUser = await _rolUserData.GetByIdRolUserAsync(rolUser.Id);
                if (existigRolUser == null)
                {
                    throw new EntityNotFoundException("RolUser", rolUser.Id);
                }

                existigRolUser.RolId = rolUser.RolId;
                existigRolUser.UserId = rolUser.UserId;

                return await _rolUserData.UpdateRolUserAsync(existigRolUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el RolUser: {RolName}", RolUserDto?.RolName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el RolUser", ex);
            }
        }

        public async Task<bool> DeleteModuleFormAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar un RolUser con ID inválido: {RolNameId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del RolUser debe ser mayor que cero");
                }
                var moduleRolUser = await _rolUserData.GetByIdRolUserAsync(id);
                if (moduleRolUser == null)
                {
                    _logger.LogInformation("No se encontró ningún RolUser con ID: {RolNameId}", id);
                    throw new EntityNotFoundException("RolUser", id);
                }
                return await _rolUserData.DeleteRolUserAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el RolUser con ID: {RolNameId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el RolUser con ID {id}", ex);
            }
        }

        private void ValidateRolUser(RolUserDto RolUserDto)
        {
            if (RolUserDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }
            if (RolUserDto.UserId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con UserId inválido");
                throw new Utilities.Exceptions.ValidationException("UserId", "El UserId es obligatorio y debe ser mayor que cero");
            }
        }

        // Método para mapear de Rol a RolDTO
        private RolUserDto MapToDTO(RolUser rolUser)
        {
            return new RolUserDto
            {
                RolUserId = rolUser.Id,
                RolId = rolUser.RolId,
                RolName = rolUser.Rol?.Name,
                UserId = rolUser.UserId,
                UserName = rolUser.User?.Name,
                UserLastName = rolUser.User?.LastName
            };
        }

        // Método para mapear de RolDTO a Rol
        private RolUser MapToEntity(RolUserDto rolUserDTO)
        {
            return new RolUser
            {
                Id = rolUserDTO.RolUserId,
                UserId = rolUserDTO.UserId,
                RolId = rolUserDTO.RolId,
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<RolUserDto> MapToDTOList(IEnumerable<RolUser> rolUsers)
        {
            return rolUsers.Select(MapToDTO).ToList();
        }
    }
}
