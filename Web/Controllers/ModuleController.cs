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
    public class ModuleController : ControllerBase
    {
        private readonly ModuleBusiness _moduleBusiness;
        private readonly ILogger<ModuleController> _logger;
        public ModuleController(ModuleBusiness moduleBusiness, ILogger<ModuleController> logger)
        {
            _moduleBusiness = moduleBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obteniendo todos los Modulos 
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ModuleDto>), 200)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> GetAllModules()
        {
            try
            {
                var modules = await _moduleBusiness.GetAllModulesAsync();
                return Ok(modules);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error al obtener los Modulos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obetniendo Modulo con un ID expecifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ModuleDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> GetModuleById(int id)
        {
            try
            {
                var module = await _moduleBusiness.GetModuleByIdAsync(id);
                return Ok(module);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validación fallida para el module con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Module no encontrado con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener Module con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        /// <summary>
        /// Crear Module
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ModuleDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> CreateModule([FromBody] ModuleDto module)
        {
            try
            {
                var newModule = await _moduleBusiness.CreateModuleAsync(module);
                return CreatedAtAction(nameof(GetModuleById), new { id = newModule.ModuleId }, newModule);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validación fallida al crear el module");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el module");
                return StatusCode(500, new { message = ex.Message });
            }

        }


        /// <summary>
        /// Actualizar Module
        /// </summary>
        /// <param name="id"></param>
        /// <param name="moduleDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ModuleDto),200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> UpadteModuleAsync(int id,[FromBody] ModuleDto moduleDto)
        {
            try
            {
                if (moduleDto.ModuleId == 0)
                {
                    moduleDto.ModuleId = id;
                }

                if(id != moduleDto.ModuleId)
                {
                    return BadRequest(new { message = "El ID de la URL no coincede con el ID del Permission en el body" });
                }

                var UpdateModule = await _moduleBusiness.UpdateModuleAsync(moduleDto);
                return Ok(UpdateModule);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al actualizar Module con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Module no encontrado con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar Module con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Eliminar un Module de forma Persistente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete]
        [ProducesResponseType(typeof(ModuleDto),200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteModuleAsync(int id)
        {
            try
            {
                var deleteModule = await _moduleBusiness.DeletePersistenModuleAsync(id);
                return Ok(deleteModule);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al eliminar Module con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Module no encontrado con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar Module con ID: {Module}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPatch]
        [ProducesResponseType(typeof(RolDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteLogicoModuleAsync(int id)
        {
            try
            {
                var deleteLogicalModule = await _moduleBusiness.DeleteLogicalModuleAsync(id);
                return Ok(deleteLogicalModule);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al eliminar logico Module con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Module no encontrado con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar logico Module con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
