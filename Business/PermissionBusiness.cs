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
    class PermissionBusiness
    {
        private readonly PermissionData _permissionData;
        private readonly ILogger _logger;

        public PermissionBusiness(PermissionData permissionData, ILogger logger)
        {
            _permissionData = permissionData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<PermissionDto>> GetAllPermissionAsync()
        {
            try
            {
                var permissions = await _permissionData.GetAllAsync();
                var permissionsDTO = new List<PermissionDto>();

                foreach (var permission in permissions)
                {
                    permissionsDTO.Add(new PermissionDto
                    {
                        PermissionId = permission.Id,
                        PermissionName = permission.Name,
                        PermissionDescription = permission.Description

                    });
                }

                return permissionsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<PermissionDto> GetPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un permiso con ID inválido: {PermissionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso debe ser mayor que cero");
            }

            try
            {
                var permission = await _permissionData.GetByIdAsync(id);
                if (permission == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso con ID: {PermissionId}", id);
                    throw new EntityNotFoundException("Permiso", id);
                }

                return new PermissionDto
                {
                    PermissionId = permission.Id,
                    PermissionName = permission.Name,
                    PermissionDescription = permission.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID: {PermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso con ID {id}", ex);
            }
        }

        // Método para crear un formulario desde un DTO
        public async Task<PermissionDto> CreatePermissionAsync(PermissionDto PermissionDto)
        {
            try
            {
                ValidatePermission(PermissionDto);

                var permission = new Permission
                {
                    Name = PermissionDto.PermissionName,
                    Description = PermissionDto.PermissionDescription // Se agregó la descripción
                };

                var permissionCreado = await _permissionData.CreateAsync(permission);

                return new PermissionDto
                {
                    PermissionId = permission.Id,
                    PermissionName = permission.Name,
                    PermissionDescription = permission.Description // Se agregó la descripción
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo permiso: {PermissionName}", PermissionDto?.PermissionName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso", ex);
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
    }
}
