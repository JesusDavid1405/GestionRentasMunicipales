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
    public class ModuleFormBusiness
    {
        private readonly ModuleFormData _moduleFormData;
        private readonly ILogger _logger;

        public ModuleFormBusiness(ModuleFormData moduleFormData, ILogger logger)
        {
            _moduleFormData = moduleFormData;
            _logger = logger;
        }

        public async Task<IEnumerable<ModuleFormDto>> GetAllModuleFormAsync()
        {
            try
            {
                var moduleForms = await _moduleFormData.GetAllAsync();
                var modulesFormsDto = new List<ModuleFormDto>();

                foreach (var moduleForm in moduleForms)
                {
                    modulesFormsDto.Add(new ModuleFormDto
                    {
                        ModuleFormId = moduleForm.Id,
                        ModuleId = moduleForm.ModuleId,
                        FormId = moduleForm.FormId

                    });
                }
                return modulesFormsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los ModuleForm");
                throw new ExternalServiceException("Base de datos", "Error al recuperrar la lista de ModuleForm", ex);
            }
        }

        public async Task<ModuleFormDto> GetModuleFormByIdAsync(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un moduleForm con ID inválido: {ModuleFormId}", Id);
                throw new Utilities.Exceptions.ValidationException("Id", "El ID del modulo debe ser mayor que cero");
            }
            try
            {
                var moduleForm = await _moduleFormData.GetByIdAsync(Id);
                if (moduleForm == null)
                {
                    _logger.LogInformation("No se encontró ningún moduleForm con ID: {ModuleFormId}", Id);
                    throw new EntityNotFoundException("ModuleForm", Id);
                }
                return new ModuleFormDto
                {
                    ModuleFormId = moduleForm.Id,
                    ModuleId = moduleForm.ModuleId,
                    FormId = moduleForm.FormId
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el moduleform con ID: {ModuleFormId}", Id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el moduleform con ID {Id}", ex);
            }
        }

        public async Task<ModuleFormDto> CreateModuleFormAsync(ModuleFormDto moduleFormDto)
        {
            try
            {
                ValidateModuleForm(moduleFormDto);

                var moduleForm = new ModuleForm
                {
                    ModuleId = moduleFormDto.ModuleId,
                    FormId = moduleFormDto.FormId,
                };

                var moduleFormCreado = await _moduleFormData.CreateAsync(moduleForm);

                return new ModuleFormDto
                {
                    ModuleFormId = moduleForm.Id,
                    ModuleId = moduleForm.ModuleId,
                    FormId = moduleForm.FormId,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo moduleForm: {ModuleFormId}", moduleFormDto?.ModuleFormId.ToString() ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crearr el ModuleForm", ex);
            }
        }

        private void ValidateModuleForm(ModuleFormDto moduleFormDto)
        {
            if (moduleFormDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("ModuleForm", "El modulo no puede ser nulo");
            }
            if (moduleFormDto.FormId == null)
            {
                throw new Utilities.Exceptions.ValidationException("ModuleFormId", "El FormId del ModuleForm no puede estar vacio");
            }
            if (moduleFormDto.ModuleId == null)
            {
                throw new Utilities.Exceptions.ValidationException("ModuleFormId", "El ModuleId del ModuleForm no puede estar vacio");
            }
        }
    }
}
