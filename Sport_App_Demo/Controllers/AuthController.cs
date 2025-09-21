using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sport_App_Service.Auth.Login;
using Sport_App_Service.Auth.Register;
using Sports_App_Model.Dto;
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
        public async Task<IActionResult> RegisterUser([FromBody] UserDto request)
        {
            var result = await _registerService.RegisterUserAsync(request);

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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new
                {
                    status = false,
                    message = "Session expired, please log in again"
                });
            }

            var result = await _otpService.VerifyOtpAsync(email, request.Otp);

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
