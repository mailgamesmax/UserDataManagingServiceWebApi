using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UserDataManagingService.Controllers.Requests;
using UserDataManagingService.Models;
using UserDataManagingService.Models.Repositories;
using UserDataManagingService.Services;

namespace UserDataManagingService.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // autorization test
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = (nameof(Role.DefaultUser) + "," + nameof(Role.Admin)))]
        [HttpGet(template: "TouchMeIfYouAreАuthenticated !")]
        public ActionResult<string> BeNiceForAutetified()
        {
            string heyBro = "hey bro!";
            return heyBro;
        }

        [HttpPost(template: ("SignUpNewUser"))]
        public async Task<IActionResult> SignUpNewAcc([FromBody] SignUpNewUserRequest request)
        {            
            var newAcc = await _loginService.SignupNewUser(request.Username, request.UserLastName, request.NickName, request.Password, request.PersonalCode, request.PhoneNr, request.Email);
            if (newAcc.Item1 == true)
            {
                //return BadRequest("NickName is not availible");
                return StatusCode(400, "requested NickName is not availible");
            }
            else
            {
                return Ok(newAcc.Item2);
            }
        }

        [HttpPost("FinalizingUserCreationFor_{userID}")]
        public async Task<IActionResult> ConfirmUserCreationaByUserIdRouteProvided([FromRoute] string userID)
        {
            try
            {
                _logger.LogInformation("read user id from route and convert to guid, check L.Place and Avatar for null");
                var userGuidId = _loginService.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }

                if (await _livingPlaceRepository.GetLivingPlaceDataByUserID(userGuidId) == null)
                {
                    _logger.LogWarning("Living place of User is Null. This method can't be requested in such way");
                    return StatusCode(400, "nera user living place");
                }

                if (await _avatarRepository.GetAvatarByUserID(userGuidId) == null)
                {
                    _logger.LogWarning("Avatar of User is Null. This method can't be requested in such way");
                    return StatusCode(400, "nera user avatar");
                }

                _logger.LogInformation("checking user data and activation status");
                var isUserDataOk = await _loginService.CompleteUserCreating(userGuidId);
                var userChekingOkAndProfileIsActivated = isUserDataOk.Item1;
                if (userChekingOkAndProfileIsActivated == true)
                {
                    return Ok(isUserDataOk.Item2); //action comments
                }
                else
                {
                    return BadRequest(isUserDataOk.Item2);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(template: "UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] LoginRequest request)
        {
            var response = await _loginService.UserLogin(request.NickName, request.Password);
            if (!response.IsUserExist)
            {
                return NotFound("user nerastas arba blogas slaptazodis");
                //return Unauthorized();
            }
            var createdUserId = _userRepository.GetUserIdByNickname(request.NickName); //for app representation only
            return Ok(new { Token = _jwtService.GetJwtToken(request.NickName, (Role)response.Role), UserId = createdUserId.Result });
        }

        //
        private readonly ILogger<AuthenticationController> _logger;

        private readonly IUserRepository _userRepository;
        private readonly IUserLoginService _loginService;
        private readonly IJWTService _jwtService;
        private readonly ILivingPlaceRepository _livingPlaceRepository;
        private readonly IAvatarRepository _avatarRepository;
        public AuthenticationController(IUserRepository userRepository, IUserLoginService loginService, IJWTService jwtService, ILogger<AuthenticationController> logger, ILivingPlaceRepository livingPlaceRepository, IAvatarRepository avatarRepository)
        {
            _userRepository = userRepository;
            _loginService = loginService;
            _jwtService = jwtService;
            _logger = logger;
            _livingPlaceRepository = livingPlaceRepository;
            _avatarRepository = avatarRepository;

        }
       
    }
}
