using Business.Interfaces;
using Data;
using Data.Repository;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business.Services
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class RolBusiness : IRolBusiness
    {
        private readonly RolData _rolData;
        private readonly ILogger<RolBusiness> _logger;

        public RolBusiness(RolData rolData, ILogger<RolBusiness> logger)
        {
            _rolData = rolData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<RolDto>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _rolData.GetAllRolAsyncSql();
                return MapToDTOList(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<RolDto> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var rol = await _rolData.GetByIdRolAsyncSql(id);
                if (rol == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                return MapToDTO(rol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<RolDto> CreateRolAsync(RolDto rolDto)
        {
            try
            {
                ValidateRol(rolDto);

                var rol = MapToEntity(rolDto);

                var rolCreado = await _rolData.CreateRolAsyncSql(rol);

                return MapToDTO(rolCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", rolDto?.RolName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        /// <summary>
        /// Actualizar un rol
        /// </summary>
        /// <param name="rolDto"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ExternalServiceException"></exception>

        public async Task<bool> UpdateRolAsync(RolDto rolDto)
        {
            try
            {
                ValidateRol(rolDto);

                var existingRol = await _rolData.GetByIdRolAsyncSql(rolDto.RolId);
                if (existingRol == null)
                {
                    throw new EntityNotFoundException("Rol", rolDto.RolId);
                }

                existingRol.Name = rolDto.RolName;
                existingRol.Active = rolDto.State;

                return await _rolData.UpdateRolAsyncSql(existingRol);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con ID: {UserId}", rolDto.RolName);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el Rol", ex);
            }
        }

        /// <summary>
        /// Elimina un rol de la base de datos.
        /// </summary>
        /// <param name="RolDto"></param>
        /// <exception cref="Utilities.Exceptions.ValidationException"></exception>

        public async Task<bool> DeletePersistentRolAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar un rol con ID inválido: {RolId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
                }

                var rol = await _rolData.GetByIdRolAsyncSql(id);
                if (rol == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                var result = await _rolData.DeleteRolPersistentAsyncSql(id);

                if (result)
                {
                    _logger.LogInformation("Rol eliminado exitosamente con ID: {RolId}", id);
                }
                else
                {
                    _logger.LogWarning("No se pudo eliminar el rol con ID: {RolId}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el rol", ex);
            }
        }

        /// <summary>
        /// Elminar logico de un rol de la base de datos
        /// </summary>
        /// <param name="RolDto"></param>
        /// <exception cref="Utilities.Exceptions.ValidationException"></exception>
        
        public async Task<bool> DeleteLogicalRolAsync(int id)
        {
            try
            {
                if(id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar logicamente un rol con ID invalido: {RolId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
                }

                var rol = await _rolData.GetByIdRolAsyncSql(id);

                if(rol == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                var result = await _rolData.DeleteRolLogicalAsyncSql(id);

                if (result)
                {
                    _logger.LogInformation("Rol eliminado logicamente exitosamenrte con ID: {RolId}", id);
                }
                else
                {
                    _logger.LogWarning("No se pudo eliminar de forma logica el rol con ID: {RolId}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el rol", ex);
            }
        }


        // Método para validar el DTO
        private void ValidateRol(RolDto RolDto)
        {
            if (RolDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(RolDto.RolName))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }

        // Método para mapear de Rol a RolDTO
        private RolDto MapToDTO(Rol rol)
        {
            return new RolDto
            {
                RolId = rol.Id,
                RolName = rol.Name,
                State = rol.Active,
            };
        }

        // Método para mapear de RolDTO a Rol
        private Rol MapToEntity(RolDto rolDTO)
        {
            return new Rol
            {
                Id = rolDTO.RolId,
                Name = rolDTO.RolName,
                Active= rolDTO.State,
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<RolDto> MapToDTOList(IEnumerable<Rol> roles)
        {
            return roles.Select(MapToDTO).ToList();
        }
    }
}