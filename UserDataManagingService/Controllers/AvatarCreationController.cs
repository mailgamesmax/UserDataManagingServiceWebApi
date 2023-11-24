using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using UserDataManagingService.Controllers.Requests;
using UserDataManagingService.Models;
using UserDataManagingService.Services;
using Azure;

namespace UserDataManagingService.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AvatarCreationController : ControllerBase
    {

        [HttpPost("avatarCreationFor_{userID}")]
        public async Task<IActionResult> CreateAvatarByUserIdRouteProvided([FromForm] ImageUploadRequest request, [FromRoute] string userID)
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
        private readonly ILogger<AvatarCreationController> _logger;
        private readonly IAvatarCRUDService _avatarService;

        public AvatarCreationController(ILogger<AvatarCreationController> logger, IAvatarCRUDService avatarService)
        {
            _logger = logger;
            _avatarService = avatarService;
        }
       
    }
}
