using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IO;
using UserDataManagingService.Controllers.Requests;
using UserDataManagingService.Models;
using UserDataManagingService.Services;

namespace UserDataManagingService.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class EditLivingPlaceController : ControllerBase
    {

        [HttpPost(template: ("EditLivingData"))]
        public async Task<IActionResult> EditLivingData([FromBody] EditLivingDataRequest request)
        {
            var editedLivingPlace = await _placeService.EditLivingPlaceData(request.NickName, request.City, request.Street, request.BuildingNr, request.ApartmentNr);
            /*       if (newAcc.Item1 == false)
                   {
                       //return BadRequest("NickName is not availible");
                       return StatusCode(400, "requested NickName is not availible");
                   }
                   else
                   {
                       return Ok(newAcc.Item2);
                   }*/
            return Ok(editedLivingPlace);
        }

        [HttpPost(template: "UserLogin")]
        public IActionResult UserLogin([FromBody] LoginRequest request)
        {
            var response = _loginService.UserLogin(request.NickName, request.Password);
            if (!response.IsUserExist)
            {
                return Unauthorized();
            }
            return Ok(_jwtService.GetJwtToken(request.NickName, (Role)response.Role));
        }
        // GET: api/<AuthenticationController>

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
