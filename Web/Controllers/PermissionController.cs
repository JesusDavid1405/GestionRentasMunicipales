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
    public class PermissionController : ControllerBase
    {
        private readonly PermissionBusiness _permissionBusiness;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController(PermissionBusiness permissionBusiness, ILogger<PermissionController> logger)
        {                                                                                       
            _permissionBusiness = permissionBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obteniendo todos los Permission 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PermissionDto>), 200)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> GetAllPermission()
        {
            try
            {
                var permission = await _permissionBusiness.GetAllPermissionAsync();
                return Ok(permission);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error al obtener los Permission");
                return StatusCode(500, new { message = ex.Message });
            }
        }
        
        /// <summary>
        /// Obetniendo Permission con un ID expecifico 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PermissionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            try
            {
                var permission = await _permissionBusiness.GetPermissionByIdAsync(id);
                return Ok(permission);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validación fallida para el permiso con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener Permission con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Creando un Permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>

        [HttpPost]
        [ProducesResponseType(typeof(PermissionDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDto permission)
        {
            try
            {
                var newPermission = await _permissionBusiness.CreatePermissionAsync(permission);
                return CreatedAtAction(nameof(GetPermissionById), new { id = newPermission.PermissionId }, newPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear el permiso");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el permiso");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualizando un Permission 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permissionDto"></param>
        /// <returns></returns>

        [HttpPut]
        [ProducesResponseType(typeof(PermissionDto),200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePermissionAsync(int id, [FromBody] PermissionDto permissionDto)
        {
            try
            {
                if (permissionDto.PermissionId == 0)
                {
                    permissionDto.PermissionId = id;
                }

                if (id != permissionDto.PermissionId)
                {
                    return BadRequest(new { message = "El ID de la URL no coincide con el ID del Permission en el body." });
                }

                var updatePermission = await _permissionBusiness.UpdatePermissionAsync(permissionDto);
                return Ok(updatePermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al actualizar Permission con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {Permission}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar Permission con ID: {Permission}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(PermissionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRolAsync(int id)
        {
            try
            {
                var deletePermission = await _permissionBusiness.DeletePersistentPernissionAsync(id);
                return Ok(deletePermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al eliminar Permission con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar Permission con ID: {Permission}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPatch]
        [ProducesResponseType(typeof(PermissionDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLogicalRolAsync(int id)
        {
            try
            {
                var deleteLogicalPermission = await _permissionBusiness.DeleteLogicalPermissionAsync(id);
                return Ok(deleteLogicalPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al eliminar logico Permission con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar logico Permission con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}

    
   
