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
            if (newAcc.Item1 == false)
            {
                //return BadRequest("NickName is not availible");
                return StatusCode(400, "requested NickName is not availible");
            }
            else
            {
                return Ok(newAcc.Item2);
            }
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
        private readonly ILogger<AuthenticationController> _logger;

        private readonly IUserLoginAndCreateService _loginService;
        private readonly IJWTService _jwtService;
        public AuthenticationController(IUserLoginAndCreateService loginService, IJWTService jwtService, ILogger<AuthenticationController> logger)
        {
            _loginService = loginService;
            _jwtService = jwtService;
            _logger = logger;
        }
       
    }
}
