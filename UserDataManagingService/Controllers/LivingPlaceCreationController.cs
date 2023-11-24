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
    public class LivingPlaceCreationController : ControllerBase
    {

        [HttpPost("placeCreationFor_{userID}")]
        public async Task<IActionResult> CreateLivingDataByUserIdRouteProvided([FromBody] EditLivingDataRequest request, [FromRoute] string userID)
        {
            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _placeService.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }

                _logger.LogInformation("request data changing from according service");
                var editedLivingPlace = await _placeService.CreateLivingPlaceDataByUserId(userGuidId, request.City, request.Street, request.BuildingNr, request.ApartmentNr);
                _placeService.AutoCycleFixer_UserLivingPlace(editedLivingPlace);

                _logger.LogInformation("Data updated successfully");
                return Ok(new { Confirmation = "duomenys atnaujinti", LivingPlace = editedLivingPlace });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PAPILDOMAS FUNKCIONALUMAS NE PAGAL UZDUOTI, MOKYMOSI TIKSLAIS
        [HttpPost(template: ("CreateEditLivingPlaceForNickName"))]
        public async Task<IActionResult> EditLivingData([FromBody] EditLivingDataForNickNameRequest request)
        {
            var editedLivingPlace = await _placeService.CreateLivingPlaceDataByNickName(request.NickName, request.City, request.Street, request.BuildingNr, request.ApartmentNr);
            _placeService.AutoCycleFixer_UserLivingPlace(editedLivingPlace);
            return Ok(editedLivingPlace);
        }

        //
        private readonly ILogger<LivingPlaceCreationController> _logger;

        private readonly IUserLoginService _loginService;
        private readonly ILivingPlaceEditService _placeService;

        public LivingPlaceCreationController(IUserLoginService loginService, ILogger<LivingPlaceCreationController> logger, ILivingPlaceEditService placeService)
        {
            _loginService = loginService;
            _logger = logger;
            _placeService = placeService;
        }
       
    }
}
