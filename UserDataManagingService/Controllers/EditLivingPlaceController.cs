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
    public class EditLivingPlaceController : ControllerBase
    {

        [HttpPost(template: ("EditLivingPlaceForNickName"))]
        public async Task<IActionResult> EditLivingData([FromBody] EditLivingDataForNickNameRequest request)
        {

            var editedLivingPlace = await _placeService.EditLivingPlaceDataByNickName(request.NickName, request.City, request.Street, request.BuildingNr, request.ApartmentNr);
            _placeService.AutoCycleFixer_UserLivingPlace(editedLivingPlace);
            return Ok(editedLivingPlace);
        }

        [HttpPost("editingFor_{userID}")]
        public async Task<IActionResult> EditLivingDataByUserIdRouteProvided([FromBody] EditLivingDataRequest request, [FromRoute] string userID)
        {
            Guid guidUserId;
            if (Guid.TryParse(userID, out guidUserId))
            {
                var editedLivingPlace = await _placeService.EditLivingPlaceDataByUserId(guidUserId, request.City, request.Street, request.BuildingNr, request.ApartmentNr);
                _placeService.AutoCycleFixer_UserLivingPlace(editedLivingPlace);
                return Ok(editedLivingPlace);                
            }
            else
            {
                return BadRequest("nesekmingas vartotojo ID apdorojimas - kreipkites i administratoriu"); 
            }           
        }


        //
        private readonly ILogger<EditLivingPlaceController> _logger;

        private readonly IUserLoginAndCreateService _loginService;
        private readonly IJWTService _jwtService;
        private readonly ILivingPlaceEditService _placeService;

        public EditLivingPlaceController(IUserLoginAndCreateService loginService, IJWTService jwtService, ILogger<EditLivingPlaceController> logger, ILivingPlaceEditService placeService)
        {
            _loginService = loginService;
            _jwtService = jwtService;
            _logger = logger;
            _placeService = placeService;
        }
       
    }
}
