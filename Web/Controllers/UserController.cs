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
    public class UserController : ControllerBase
    {
        private readonly UserBusiness _userBusiness;
        private readonly ILogger<PermissionController> _logger;

        /// <summary>
        /// Contructor que recibe el contexto de base de datos
        /// </summary>
        /// <param name="userBusiness"></param>
        /// <param name="logger"></param>
        public UserController(UserBusiness userBusiness, ILogger<PermissionController> logger)
        {
            _userBusiness = userBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obteniendo todos los Usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var users = await _userBusiness.GetAllUsersAsync();
                return Ok(users);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error al obtener los User");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PermissionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userBusiness.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validacion fallida para el user con ID: {UserId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "User no encontrado con ID: {UserId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener User con ID: {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
