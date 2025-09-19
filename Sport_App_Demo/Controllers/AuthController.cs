using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sport_App_Service.Auth.Login;
using Sport_App_Service.Auth.Register;
using Sports_App_Model.Requests.Auth;
using Sports_App_Service.Auth.Otp;

namespace Sport_App_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [EnableCors("AllowAll")]
    public class AuthController : Controller
    {
        private readonly IRegisterService _registerService;
        private readonly ILoginService _loginService;
        private readonly IOtpService _otpService;

        public AuthController(IRegisterService registerService, ILoginService loginService,
            IOtpService otpService)
        {
            _registerService = registerService;
            _loginService = loginService;
            _otpService = otpService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            var result = await _registerService.RegisterUserAsync(request.Name, request.Surname, request.Email, request.Password, request.Role, request.Race, request.IdNumber, request.RoleType);

            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request)
        {
            var result = await _loginService.LoginUserAsync(request.Email, request.Password);

            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            int userId = 0;

            if (int.TryParse(userIdClaim, out var id))
            {
                userId = id;
            }

            var result = await _otpService.VerifyOtpAsync(userId, request.Otp);

            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
