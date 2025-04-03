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
    public class ModuleBusiness
    {
        private readonly ModuleData _moduleData;
        private readonly ILogger<ModuleBusiness> _logger;

        public ModuleBusiness(ModuleData moduleData, ILogger<ModuleBusiness> logger)
        {
            _moduleData = moduleData;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los modulos almacenados en la base de datos.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
        {
            try
            {
                var modules = await _moduleData.GetAllModuleAsync();
                return MapToDTOList(modules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los Modulos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de Modulos", ex);
            }
        }

        /// <summary>
        /// Obtiene un modulo por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Utilities.Exceptions.ValidationException"></exception>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<ModuleDto> GetModuleByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un modulo con ID inválido: {ModuleId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del modulo debe ser mayor que cero");
            }
            try
            {
                var module = await _moduleData.GetByIdModuleAsync(id);
                if (module == null)
                {
                    _logger.LogInformation("No se encontró ningún modulo con ID: {ModuleId}", id);
                    throw new EntityNotFoundException("Modulo", id);
                }

                return MapToDTO(module);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el modulo con ID: {ModuleId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el modulo con ID {id}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo modulo en la base de datos.
        /// </summary>
        /// <param name="ModuleDto"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<ModuleDto> CreateModuleAsync(ModuleDto ModuleDto)
        {
            try
            {
                ValidateModule(ModuleDto);

                var module = MapToEntity(ModuleDto);

                var moduleCreado = await _moduleData.CreateModuleAsync(module);

                return MapToDTO(moduleCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo modulo: {ModuleNombre}", ModuleDto?.ModuleName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        /// <summary>
        /// Actualizar un modulo
        /// </summary>
        /// <param name="moduleDto"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<bool> UpdateModuleAsync(ModuleDto moduleDto)
        {
            try
            {
                ValidateModule(moduleDto);

                var existingModule = await _moduleData.GetByIdModuleAsync(moduleDto.ModuleId);
                if (existingModule == null)
                {
                    throw new EntityNotFoundException("Module", moduleDto.ModuleId);
                }

                existingModule.Name = moduleDto.ModuleName;
                existingModule.Description = moduleDto.ModuleDescription;
                existingModule.Code = moduleDto.ModuleCode;
                existingModule.IsDeleted = moduleDto.Hidden;


                return await _moduleData.UpdateModuleAsync(existingModule);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el Module con ID: {ModuleName}", moduleDto.ModuleName);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el Module", ex);
            }
        }

        /// <summary>
        /// Elimina un modulo de forma persistente.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<bool> DeletePersistenModuleAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar un permiso con ID inválido: {PermissionId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso debe ser mayor que cero");
                }
                var module = await _moduleData.GetByIdModuleAsync(id);
                if (module == null)
                {
                    throw new EntityNotFoundException("Module", id);
                }
                return await _moduleData.DeletePersistentModuleAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el Module con ID: {ModuleId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el Module", ex);
            }
        }

        /// <summary>
        /// Elimina un modulo de forma logica.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<bool> DeleteLogicalModuleAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar un permiso con ID inválido: {PermissionId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso debe ser mayor que cero");
                }
                var module = await _moduleData.GetByIdModuleAsync(id);
                if (module == null)
                {
                    _logger.LogInformation("No se encontró ningún Module con ID: {ModuleId}", id);
                    throw new EntityNotFoundException("Module", id);
                }
                return await _moduleData.DeleteLogicalModuleAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el Module con ID: {ModuleId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el Module", ex);
            }
        } 

        /// <summary>
        /// Validacion de module de que no sea nulo 
        /// </summary>
        /// <param name="ModuleDto"></param>
        /// <exception cref="Utilities.Exceptions.ValidationException"></exception>
        private void ValidateModule(ModuleDto ModuleDto)
        {
            if (ModuleDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("Module", "El modulo no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(ModuleDto.ModuleName))
            {
                throw new Utilities.Exceptions.ValidationException("ModuleName", "El nombre del modulo no puede estar vacío");
            }
        }

        // Método para mapear de Rol a RolDTO
        private ModuleDto MapToDTO(Module module)
        {
            return new ModuleDto
            {
                ModuleId = module.Id,
                ModuleName = module.Name,
                ModuleDescription = module.Description,
                ModuleCode = module.Code,
                Hidden = module.IsDeleted
            };
        }

        // Método para mapear de RolDTO a Rol
        private Module MapToEntity(ModuleDto moduleDto)
        {
            return new Module
            {
                Id = moduleDto.ModuleId,
                Name = moduleDto.ModuleName,
                Description = moduleDto.ModuleDescription,
                Code = moduleDto.ModuleCode,
                IsDeleted = moduleDto.Hidden
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<ModuleDto> MapToDTOList(IEnumerable<Module> modules)
        {
            return modules.Select(MapToDTO).ToList();
        }
    }
}
