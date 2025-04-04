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
    public class FormBusiness
    {
        private readonly FormData _formData;
        private readonly ILogger<FormBusiness> _logger;

        public FormBusiness(FormData formData, ILogger<FormBusiness> logger)
        {
            _formData = formData;
            _logger = logger;
        }

        /// <summary>
        /// Metodo para obetner todos los Form desde el Dto
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<IEnumerable<FormDto>> _formBusiness()
        {
            try
            {
                var forms = await _formData.GetAllFormAsync();
                return MapToDTOList(forms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los formularios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de formularios", ex);
            }
        }

        /// <summary>
        /// Metodo para obtener un form expecifico desde el Dto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Utilities.Exceptions.ValidationException"></exception>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<FormDto> GetFormByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un formulario con ID inválido: {FormId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del formulario debe ser mayor que cero");
            }

            try
            {
                var form = await _formData.GetByIdFormAsync(id);
                if (form == null)
                {
                    _logger.LogInformation("No se encontró ningún formulario con ID: {FormId}", id);
                    throw new EntityNotFoundException("Formulario", id);
                }

                return MapToDTO(form);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formulario con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el formulario con ID {id}", ex);
            }
        }



        /// <summary>
        /// Creando un Form desde el Dto
        /// </summary>
        /// <param name="FormDto"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<FormDto> CreateFormAsync(FormDto FormDto)
        {
            try
            {
                ValidateRol(FormDto);

                var form = MapToEntity(FormDto);

                var formCreado = await _formData.CreateFormAsync(form);

                return MapToDTO(formCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo formulario: {FormName}", FormDto?.FormName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el formulario", ex);
            }
        }

        /// <summary>
        /// Actualizando un Permission
        /// </summary>
        /// <param name="FormDto"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<bool> UpdateFormAsync(FormDto FormDto)
        {
            try
            {
                ValidateRol(FormDto);

                var existigForm = await _formData.GetByIdFormAsync(FormDto.FormId);
                if (existigForm == null)
                {
                    throw new EntityNotFoundException("Form", FormDto.FormId);
                }

                existigForm.Name = FormDto.FormName;
                existigForm.Description = FormDto.FormDescription;
                existigForm.IsDeleted = FormDto.Hidden;

                return await _formData.UpdateFormAsync(existigForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el Form: {PermissionName}", FormDto?.FormName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el Form", ex);
            }
        }

        /// <summary>
        /// Metodo para Eliminar de forma persistente un Form desde el Dto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<bool> DeletePersistentFormAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar un Form con ID inválido: {FormId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del Form debe ser mayor que cero");
                }
                var form = await _formData.GetByIdFormAsync(id);
                if (form == null)
                {
                    _logger.LogInformation("No se encontró ningún Form con ID: {FormId}", id);
                    throw new EntityNotFoundException("Permiso", id);
                }
                return await _formData.DeletePersistentFormAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el Form con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el Form con ID {id}", ex);
            }
        }

        /// <summary>
        /// Método para eliminar un Form de forma lógica desde el Dto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ExternalServiceException"></exception>
        public async Task<bool> DeleteLogicalFormAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar un Form con ID inválido: {FormId}", id);
                    throw new Utilities.Exceptions.ValidationException("id", "El ID del Form debe ser mayor que cero");
                }
                var form = await _formData.GetByIdFormAsync(id);
                if (form == null)
                {
                    _logger.LogInformation("No se encontró ningún Form con ID: {FormId}", id);
                    throw new EntityNotFoundException("Permiso", id);
                }
                return await _formData.DeleteLogicalFormAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el Form con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el Form con ID {id}", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRol(FormDto FormDto)
        {
            if (FormDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto formulario no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(FormDto.FormName))
            {
                _logger.LogWarning("Se intentó crear/actualizar un formulario con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del formulario es obligatorio");
            }
        }

        private FormDto MapToDTO(Form form)
        {
            return new FormDto
            {
                FormId = form.Id,
                FormName = form.Name,
                FormDescription = form.Description,
                Hidden = form.IsDeleted,
            };
        }

        // Método para mapear de RolDTO a Rol
        private Form MapToEntity(FormDto formDto)
        {
            return new Form
            {
                Id = formDto.FormId,
                Name = formDto.FormName,
                Description = formDto.FormDescription,
                IsDeleted = formDto.Hidden,
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<FormDto> MapToDTOList(IEnumerable<Form> forms)
        {
            return forms.Select(MapToDTO).ToList();
        }
    }
}
