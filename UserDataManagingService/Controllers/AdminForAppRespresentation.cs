using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserDataManagingService.Controllers.Requests;
using UserDataManagingService.Models;
using UserDataManagingService.Services;

namespace UserDataManagingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminForAppRespresentation : ControllerBase
    {
        [HttpPost(template: ("AdminAllInOne"))]
        public async Task<IActionResult> AdminAllInOne()
        {
            var adminUser = await _adminService.AutentificateAdminUser();
            if(adminUser == null)
            {
                return NotFound("admin nerastas ir nesukurtas");
            }

            return Ok(new { Token = _jwtService.GetJwtToken(adminUser.NickName, (Role)adminUser.Role), adminNick = adminUser.NickName, adminRole = adminUser.Role, AdminId = adminUser.UserId });
        }

        //
        private readonly IAdminService _adminService;
        private readonly IJWTService _jwtService;

        public AdminForAppRespresentation(IAdminService adminService, IJWTService jwtService)
        {
            _adminService = adminService;
            _jwtService = jwtService;
        }
    }
}
