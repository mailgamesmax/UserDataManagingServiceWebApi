using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UserDataManagingService.Controllers.Requests;
using UserDataManagingService.Models;
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
        public async Task<IActionResult> EditLivingDataByUserIdRouteProvided([FromRoute] string userID)
        {
            Guid guidUserId;
            if (Guid.TryParse(userID, out guidUserId))
            {
                var isUserDataOk = await _loginService.CompleteUserCreating(guidUserId);
                if (isUserDataOk.Item1 == true)
                {
                return Ok(isUserDataOk.Item2);
                }
                else
                {
                    return BadRequest(isUserDataOk.Item2);
                }
            }
            else
            {
                return BadRequest("wrong user id threatment");
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
            return Ok(_jwtService.GetJwtToken(request.NickName, (Role)response.Role));
        }

        // GET: api/<AuthenticationController>

        //
        private readonly ILogger<AuthenticationController> _logger;

        private readonly IUserLoginService _loginService;
        private readonly IJWTService _jwtService;
        public AuthenticationController(IUserLoginService loginService, IJWTService jwtService, ILogger<AuthenticationController> logger)
        {
            _loginService = loginService;
            _jwtService = jwtService;
            _logger = logger;
        }
       
    }
}
