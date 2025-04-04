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
        private readonly ILogger<ModuleFormBusiness> _logger;

        public ModuleFormBusiness(ModuleFormData moduleFormData, ILogger<ModuleFormBusiness> logger)
        {
            _moduleFormData = moduleFormData;
            _logger = logger;
        }

        /// <summary>
        /// Metodo para obtener todos los ModuleForm desde Dto
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<IEnumerable<ModuleFormDto>> GetAllModuleFormAsync()
        {
            try
            {
                var moduleForms = await _moduleFormData.GetAllModuleFormAsync();
                return MapToDTOList(moduleForms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los ModuleForm");
                throw new ExternalServiceException("Base de datos", "Error al recuperrar la lista de ModuleForm", ex);
            }
        }

        /// <summary>
        /// Metodo para obtener todos los ModuleForm con un ID expecifico desde Dto
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="Utilities.Exceptions.ValidationException"></exception>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<ModuleFormDto> GetModuleFormByIdAsync(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un moduleForm con ID inválido: {ModuleFormId}", Id);
                throw new Utilities.Exceptions.ValidationException("Id", "El ID del modulo debe ser mayor que cero");
            }
            try
            {
                var moduleForm = await _moduleFormData.GetByIdModuleFormAsync(Id);
                if (moduleForm == null)
                {
                    _logger.LogInformation("No se encontró ningún moduleForm con ID: {ModuleFormId}", Id);
                    throw new EntityNotFoundException("ModuleForm", Id);
                }

                return MapToDTO(moduleForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el moduleform con ID: {ModuleFormId}", Id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el moduleform con ID {Id}", ex);
            }
        }

        /// <summary>
        /// Metodo para crear un ModuleForm desde un Dto
        /// </summary>
        /// <param name="moduleFormDto"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<ModuleFormDto> CreateModuleFormAsync(ModuleFormDto moduleFormDto)
        {
            try
            {
                ValidateModuleForm(moduleFormDto);

                var moduleForm = MapToEntity(moduleFormDto);

                var moduleFormCreado = await _moduleFormData.CreateModuleFormAsync(moduleForm);

                return MapToDTO(moduleFormCreado);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo moduleForm: {ModuleFormId}", moduleFormDto?.ModuleFormId.ToString() ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crearr el ModuleForm", ex);
            }
        }

        /// <summary>
        /// Actualizando un ModuleForm desde el Dto
        /// </summary>
        /// <param name="moduleFormDto"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<bool> UpdateModuleFormAsync(ModuleFormDto moduleFormDto)
        {
            try
            {
                ValidateModuleForm(moduleFormDto);

                var existigModuleForm = await _moduleFormData.GetByIdModuleFormAsync(moduleFormDto.ModuleFormId);
                if (existigModuleForm == null)
                {
                    throw new EntityNotFoundException("ModuleForm", moduleFormDto.ModuleFormId);
                }

                existigModuleForm.FormId = moduleFormDto.FormId;
                existigModuleForm.ModuleId = moduleFormDto.ModuleId;

                return await _moduleFormData.UpdateModuleFormAsync(existigModuleForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el ModuleForm: {FormName}", moduleFormDto?.FormName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el permiso", ex);
            }
        }

        /// <summary>
        /// Eliminando un ModuleForm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<bool> DeleteModuleFormAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar un ModuleForm con ID inválido: {ModuleFormId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del ModuleForm debe ser mayor que cero");
                }
                var moduleForm = await _moduleFormData.GetByIdModuleFormAsync(id);
                if (moduleForm == null)
                {
                    _logger.LogInformation("No se encontró ningún ModuleForm con ID: {ModuleFormId}", id);
                    throw new EntityNotFoundException("ModuleForm", id);
                }
                return await _moduleFormData.DeleteModuleFormAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el ModuleForm con ID: {ModuleFormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el ModuleForm con ID {id}", ex);
            }
        }


        private void ValidateModuleForm(ModuleFormDto moduleFormDto)
        {
            if (moduleFormDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("ModuleForm", "El modulo no puede ser nulo");
            }
            if (moduleFormDto.FormId <= 0)
            {
                throw new Utilities.Exceptions.ValidationException("ModuleFormId", "El FormId del ModuleForm no puede estar vacio");
            }
            if (moduleFormDto.ModuleId <= 0)
            {
                throw new Utilities.Exceptions.ValidationException("ModuleFormId", "El ModuleId del ModuleForm no puede estar vacio");
            }
        }

        // Método para mapear de Rol a RolDTO
        private ModuleFormDto MapToDTO(ModuleForm moduleForm)
        {
            return new ModuleFormDto
            {
                ModuleFormId = moduleForm.Id,
                FormId = moduleForm.FormId,
                ModuleId = moduleForm.ModuleId,
                ModuleName = moduleForm.Module?.Name,
                FormName = moduleForm.Module?.Name
            };
        }

        // Método para mapear de RolDTO a Rol
        private ModuleForm MapToEntity(ModuleFormDto moduleFormDto)
        {
            return new ModuleForm
            {
                Id = moduleFormDto.ModuleFormId,
                FormId = moduleFormDto.FormId,
                ModuleId = moduleFormDto.ModuleId

            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<ModuleFormDto> MapToDTOList(IEnumerable<ModuleForm> moduleForms)
        {
            return moduleForms.Select(MapToDTO).ToList();
        }
    }
}
