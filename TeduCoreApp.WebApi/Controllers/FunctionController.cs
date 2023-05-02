using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Extensions;
using TeduCoreApp.Utilities.Constants;

namespace TeduCoreApp.WebApi.Controllers
{
    [Authorize]
    public class FunctionController : BaseApiController
    {
        private readonly IFunctionService _functionService;

        public FunctionController(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        [HttpGet("GetAdminMenu")]
        public async Task<IActionResult> GetAdminMenu()
        {
            var roles = User.GetSpecificClaim(CommonConstants.UserClaims.Roles);
            List<FunctionViewModel> functions;

            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAllAsync();
            }
            else
            {
                //TODO: Get by permission
                functions = new List<FunctionViewModel>();
            }

            return new OkObjectResult(functions);
        }
    }
}
