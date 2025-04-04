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
    public class ModuleFormController : ControllerBase
    {
        private readonly ModuleFormBusiness _moduleFormBusiness;
        private readonly ILogger<ModuleFormController> _logger;

        public ModuleFormController(ModuleFormBusiness moduleFormBusiness, ILogger<ModuleFormController> logger)
        {
            _moduleFormBusiness = moduleFormBusiness;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ModuleFormDto>), 200)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> GetAllModuleForm()
        {
            try
            {
                var moduleForms = await _moduleFormBusiness.GetAllModuleFormAsync();
                return Ok(moduleForms);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error al obtener los ModuleForm");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ModuleFormDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetModuleFormById(int id)
        {
            try
            {
                var moduleForms = await _moduleFormBusiness.GetModuleFormByIdAsync(id);
                return Ok(moduleForms);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validación fallida para el moduleForm con ID: {ModuleFormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "moduleForm no encontrado con ID: {ModuleFormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener moduleForm con ID: {ModuleFormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ModuleForm), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateModuleForm([FromBody] ModuleFormDto moduleForms)
        {
            try
            {
                var newModuleForm = await _moduleFormBusiness.CreateModuleFormAsync(moduleForms);
                return CreatedAtAction(nameof(GetModuleFormById), new { id = newModuleForm.ModuleFormId }, newModuleForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear el moduleForm");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el moduleForm");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ModuleFormDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateModuleForm(int id, [FromBody] ModuleFormDto moduleForms)
        {
            try
            {
                if (moduleForms.ModuleFormId == 0)
                {
                    moduleForms.ModuleFormId = id;
                }

                if (id != moduleForms.ModuleFormId)
                {
                    return BadRequest(new { message = "El ID de la URL no coincide con el ID del moduleForm en el body." });
                }

                var updateModuleForm = await _moduleFormBusiness.UpdateModuleFormAsync(moduleForms);
                return Ok(updateModuleForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al actualizar moduleForm con ID: {ModuleFormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "moduleForm no encontrado con ID: {ModuleFormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar moduleForm con ID: {ModuleFormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ModuleFormDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePermission(int id)
        {
            try
            {
                var deleteModuleForm = await _moduleFormBusiness.DeleteModuleFormAsync(id);
                return Ok(deleteModuleForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al eliminar moduleForm con ID: {ModuleFormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "moduleForm no encontrado con ID: {ModuleFormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar moduleForm con ID: {ModuleFormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
