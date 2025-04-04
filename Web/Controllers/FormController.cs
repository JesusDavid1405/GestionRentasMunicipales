using Business;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FormController : ControllerBase
    {
        private readonly FormBusiness _formBusiness;
        private readonly ILogger<FormController> _logger;

        public FormController(FormBusiness formBusiness, ILogger<FormController> logger)
        {
            _formBusiness = formBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obteniendo todos los Form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormDto>), 200)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> GetAllForm()
        {
            try
            {
                var form = await _formBusiness._formBusiness();
                return Ok(form);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error al obtener los Form");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obteniendo From con un ID Expecifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> GetByIdForm(int id)
        {
            try
            {
                var form = await _formBusiness.GetFormByIdAsync(id);
                return Ok(form);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validación fallida para el Form con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Form no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener Form con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Creando un Form
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(FormDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateForm([FromBody] FormDto form)
        {
            try
            {
                var newForm = await _formBusiness.CreateFormAsync(form);
                return CreatedAtAction(nameof(GetByIdForm), new { id = newForm.FormId }, newForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear el Form");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el Form");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualizando un Form
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(FormDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateFormAsync(int id, [FromBody] FormDto formDto)
        {
            try
            {
                if (formDto.FormId == 0)
                {
                    formDto.FormId = id;
                }

                if (id != formDto.FormId)
                {
                    return BadRequest(new { message = "El ID de la URL no coincide con el ID del Form en el body." });
                }

                var updateForm = await _formBusiness.UpdateFormAsync(formDto);
                return Ok(updateForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al actualizar Form con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Form no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar Form con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        /// <summary>
        /// Elminar un Form de forma persistente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(FormDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRolAsync(int id)
        {
            try
            {
                var deleteForm = await _formBusiness.DeletePersistentFormAsync(id);
                return Ok(deleteForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al eliminar Form con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Form no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar Form con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        /// <summary>
        /// Eliminar un Form de forma logical
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(FormDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLogicalRolAsync(int id)
        {
            try
            {
                var deleteLogicalForm = await _formBusiness.DeleteLogicalFormAsync(id);
                return Ok(deleteLogicalForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al eliminar logico Form con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Form no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar logico Form con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
