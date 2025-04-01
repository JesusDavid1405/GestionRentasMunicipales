using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    ///<summary>
    ///Controlador para la gestion de permisos en el sistema
    /// </summary>
    ///
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolController : ControllerBase
    {
        private readonly RolBusiness _rolBusiness;
        private readonly ILogger<RolController> _logger;

        ///<summary>
        ///Constructor del controlador de permisos
        /// </summary>
        /// <param name="rolBusiness">Negocio de permisos</param>
        /// <param name="logger">Logger</param>"

        public RolController(RolBusiness rolBusiness, ILogger<RolController> logger)
        {
            _rolBusiness = rolBusiness;
            _logger = logger;
        }

        ///<summary>
        ///Obtine todos los permisos del sistema
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolDto>), 200)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> GetAllRols()
        {
            try
            {
                var roles = await _rolBusiness.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error al obtener los roles");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        ///<summary>
        /// obtiene un permiso por su id
        ///</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> GetRolById(int id)
        {
            try
            {
                var rol = await _rolBusiness.GetRolByIdAsync(id);
                return Ok(rol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        ///<summary>
        /// crea un nuevo permiso en el sistema
        /// </summary>

        [HttpPost]
        [ProducesResponseType(typeof(RolDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRol([FromBody] RolDto rol)
        {
            try
            {
                var newRol = await _rolBusiness.CreateRolAsync(rol);
                return CreatedAtAction(nameof(GetRolById), new { id = newRol.RolId }, newRol);
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

    }
}
