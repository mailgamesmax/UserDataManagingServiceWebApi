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
        public async Task<IActionResult> SignUpNewClient([FromBody] SignUpNewUserRequest request)
        {
            var newAcc = await _loginService.SignupNewUser(request.Username, request.NickName, request.Password);
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
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthenticationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthenticationController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthenticationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthenticationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

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
