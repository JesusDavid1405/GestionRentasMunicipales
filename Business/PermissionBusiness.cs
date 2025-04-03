using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business
{
    public class PermissionBusiness
    {
        private readonly PermissionData _permissionData;
        private readonly ILogger<PermissionBusiness> _logger;

        public PermissionBusiness(PermissionData permissionData, ILogger<PermissionBusiness> logger)
        {
            _permissionData = permissionData;
            _logger = logger;
        }

        /// <summary>
        /// Método para obtener todos los permisos como DTOs
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<IEnumerable<PermissionDto>> GetAllPermissionAsync()
        {
            try
            {
                var permissions = await _permissionData.GetAllPermissionAsync();
                return MapToDTOList(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos", ex);
            }
        }

        /// <summary>
        /// Método para obtener un permiso por ID como DTO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Utilities.Exceptions.ValidationException"></exception>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<PermissionDto> GetPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un permiso con ID inválido: {PermissionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso debe ser mayor que cero");
            }

            try
            {
                var permission = await _permissionData.GetByIdPermissionAsync(id);
                if (permission == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso con ID: {PermissionId}", id);
                    throw new EntityNotFoundException("Permiso", id);
                }

                return MapToDTO(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID: {PermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso con ID {id}", ex);
            }
        }

        /// <summary>
        /// Método para crear un nuevo Permission
        /// </summary>
        /// <param name="PermissionDto"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<PermissionDto> CreatePermissionAsync(PermissionDto PermissionDto)
        {
            try
            {
                ValidatePermission(PermissionDto);

                var permission = MapToEntity(PermissionDto);

                var permissionCreado = await _permissionData.CreatePermissionAsync(permission);

                return MapToDTO(permissionCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo permiso: {PermissionName}", PermissionDto?.PermissionName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso", ex);
            }
        }

        /// <summary>
        /// Metodo para actualizar un Permission
        /// </summary>
        /// <param name="PermissionDto"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>

        public async Task<bool> UpdatePermissionAsync(PermissionDto PermissionDto)
        {
            try
            {
                ValidatePermission(PermissionDto);   
                
                var existigPermission = await _permissionData.GetByIdPermissionAsync(PermissionDto.PermissionId);
                if (existigPermission == null)
                {
                    throw new EntityNotFoundException("Permiso", PermissionDto.PermissionId);
                } 

                existigPermission.Name = PermissionDto.PermissionName;
                existigPermission.Description = PermissionDto.PermissionDescription;
                existigPermission.IsDeleted = PermissionDto.Hidden;

                return await _permissionData.UpdatePermissionAsync(existigPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso: {PermissionName}", PermissionDto?.PermissionName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el permiso", ex);
            }
        }

        /// <summary>
        /// Método para eliminar un permiso de forma persistente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>

        public async Task<bool> DeletePersistentPernissionAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar un permiso con ID inválido: {PermissionId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso debe ser mayor que cero");
                }
                var permission = await _permissionData.GetByIdPermissionAsync(id);
                if (permission == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso con ID: {PermissionId}", id);
                    throw new EntityNotFoundException("Permiso", id);
                }
                return await _permissionData.DeletePersistenteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el permiso con ID: {PermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el permiso con ID {id}", ex);
            }
        }


        /// <summary>
        /// Método para eliminar un permiso de forma lógica
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
                    _logger.LogWarning("Se intentó eliminar un permiso con ID inválido: {PermissionId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso debe ser mayor que cero");
                }
                var permission = await _permissionData.GetByIdPermissionAsync(id);
                if (permission == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso con ID: {PermissionId}", id);
                    throw new EntityNotFoundException("Permiso", id);
                }
                return await _permissionData.DeleteLogicalAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el permiso con ID: {PermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el permiso con ID {id}", ex);
            }
        }


        // Método para validar el DTO
        private void ValidatePermission(PermissionDto PermissionDto)
        {
            if (PermissionDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto permiso no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(PermissionDto.PermissionName))
            {
                _logger.LogWarning("Se intentó crear/actualizar un permiso con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del permiso es obligatorio");
            }
        }

        // Método para mapear de Rol a RolDTO
        private PermissionDto MapToDTO(Permission permission)
        {
            return new PermissionDto
            {
                PermissionId = permission.Id,
                PermissionName = permission.Name,
                PermissionDescription = permission.Description,
                Hidden = permission.IsDeleted,
            };
        }

        // Método para mapear de RolDTO a Rol
        private Permission MapToEntity(PermissionDto permissionDTO)
        {
            return new Permission
            {
                Id = permissionDTO.PermissionId,
                Name = permissionDTO.PermissionName,
                Description = permissionDTO.PermissionDescription,
                IsDeleted = permissionDTO.Hidden,
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<PermissionDto> MapToDTOList(IEnumerable<Permission> permissions)
        {
            return permissions.Select(MapToDTO).ToList();
        }
    }
}
