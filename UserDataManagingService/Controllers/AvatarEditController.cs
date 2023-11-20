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

namespace UserDataManagingService.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AvatarEditController : ControllerBase
    {

/*        [HttpPost(template: ("EditLivingPlaceForNickName"))]
        public async Task<IActionResult> EditLivingData([FromBody] EditLivingDataForNickNameRequest request)
        {

            var editedLivingPlace = await _placeService.EditLivingPlaceDataByNickName(request.NickName, request.City, request.Street, request.BuildingNr, request.ApartmentNr);
            _placeService.AutoCycleFixer_UserLivingPlace(editedLivingPlace);
            return Ok(editedLivingPlace);
        }*/

        [HttpPost("AvatarEditingFor_{userID}")]
        public async Task<IActionResult> EditAvatarByUserIdRouteProvided([FromForm] ImageUploadRequest request, [FromRoute] string userID)
        {
            Guid guidUserId;
            if (Guid.TryParse(userID, out guidUserId))
            {
                var editedAvatar = await _avatarService.CreateOrUpdateAvatar(guidUserId, request.Image);
                _avatarService.AutoCycleFixer_UserAvatar(editedAvatar);
                return Ok(editedAvatar);                
            }
            else
            {
                return BadRequest("nesekmingas vartotojo ID apdorojimas - kreipkites i administratoriu"); 
            }           
        }


        //
        private readonly ILogger<AvatarEditController> _logger;

        //private readonly IUserLoginervice _loginService;
        private readonly IJWTService _jwtService;
        private readonly ILivingPlaceEditService _placeService;
        private readonly IAvatarCRUDService _avatarService;

        public AvatarEditController(IJWTService jwtService, ILogger<AvatarEditController> logger, ILivingPlaceEditService placeService, IAvatarCRUDService avatarService)
        {
            _jwtService = jwtService;
            _logger = logger;
            _placeService = placeService;
            _avatarService = avatarService;
        }
       
    }
}
