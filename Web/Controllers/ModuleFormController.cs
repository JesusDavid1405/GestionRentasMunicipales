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
    }
}
