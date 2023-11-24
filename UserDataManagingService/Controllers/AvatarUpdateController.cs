using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserDataManagingService.Controllers.Requests;
using UserDataManagingService.Models;
using UserDataManagingService.Services;

namespace UserDataManagingService.Controllers
{
    [Route("[controller]")]
    [ApiController]

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = (nameof(Role.DefaultUser) + "," + nameof(Role.Admin)))]
    public class AvatarUpdateController : ControllerBase
    {
        [HttpPost("avatarUpdateFor_{userID}")]
        public async Task<IActionResult> UpdateAvatarByUserIdRouteProvided([FromForm] ImageUploadRequest request, [FromRoute] string userID)
        {
            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _avatarService.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }
                _logger.LogInformation("request data changing from according service");

                var editedAvatar = await _avatarService.CreateOrUpdateAvatar(userGuidId, request.Image);
                _avatarService.AutoCycleFixer_UserAvatar(editedAvatar);

                _logger.LogInformation("Data updated successfully");
                return Ok(new { Confirmation = "duomenys atnaujinti", Avatar = editedAvatar });

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //
        private readonly ILogger<AvatarUpdateController> _logger;

        private readonly IAvatarCRUDService _avatarService;
        private readonly IJWTService _jwtService;
        public AvatarUpdateController(IJWTService jwtService, ILogger<AvatarUpdateController> logger, IAvatarCRUDService avatarService)
        {
            _logger = logger;
            _avatarService = avatarService;
            _jwtService = jwtService;

        }
    }
}
