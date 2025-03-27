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
    class ModuleBusiness
    {
        private readonly ModuleData _moduleData;
        private readonly ILogger _logger;

        public ModuleBusiness(ModuleData moduleData, ILogger logger)
        {
            _moduleData = moduleData;
            _logger = logger;
        }

        public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
        {
            try
            {
                var modules = await _moduleData.GetAllAsync();
                var modulesDTO = new List<ModuleDto>();

                foreach (var module in modules)
                {
                    modulesDTO.Add(new ModuleDto
                    {
                        ModuleId = module.Id,
                        ModuleName = module.Name,
                        ModuleDescription = module.Descripcion,
                        ModuleCode = module.Code

                    });
                }
                return modulesDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los Modulos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de Modulos", ex);
            }
        }
        public async Task<ModuleDto> GetModuleByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un modulo con ID inválido: {ModuleId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del modulo debe ser mayor que cero");
            }
            try
            {
                var module = await _moduleData.GetByIdAsync(id);
                if (module == null)
                {
                    _logger.LogInformation("No se encontró ningún modulo con ID: {ModuleId}", id);
                    throw new EntityNotFoundException("Modulo", id);
                }

                return new ModuleDto
                {
                    ModuleId = module.Id,
                    ModuleName = module.Name,
                    ModuleDescription = module.Descripcion,
                    ModuleCode = module.Code
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el modulo con ID: {ModuleId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el modulo con ID {id}", ex);
            }
        }

        public async Task<ModuleDto> CreateModuleAsync(ModuleDto ModuleDto)
        {
            try
            {
                ValidateModule(ModuleDto);

                var module = new Module
                {
                    Name = ModuleDto.ModuleName,
                    Descripcion = ModuleDto.ModuleDescription,
                    Code = ModuleDto.ModuleCode
                };


                var moduleCreado = await _moduleData.CreateAsync(module);

                return new ModuleDto
                {
                    ModuleId = module.Id,
                    ModuleName = module.Name,
                    ModuleDescription = module.Descripcion,
                    ModuleCode = module.Code
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo modulo: {ModuleNombre}", ModuleDto?.ModuleName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }
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
    }
}
