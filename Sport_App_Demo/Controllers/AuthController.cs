using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sport_App_Model.Requests;
using Sport_App_Service.Auth.Login;
using Sport_App_Service.Auth.Register;

namespace Sport_App_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [EnableCors("AllowAll")]
    public class AuthController : Controller
    {
        private readonly IRegisterService _registerService;
        private readonly ILoginService _loginService;

        public AuthController(IRegisterService registerService, ILoginService loginService)
        {
            _registerService = registerService;
            _loginService = loginService;
        }

        [HttpPost]
        public IActionResult RegisterUser([FromBody] RegisterUserRequest request)
        {
            var result = _registerService.RegisterReturn(request.Email, request.Password, request.Role);

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
        public IActionResult LoginUser([FromBody] LoginUserRequest request)
        {
            var result = _loginService.LoginReturn(request.Email, request.Password);

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
