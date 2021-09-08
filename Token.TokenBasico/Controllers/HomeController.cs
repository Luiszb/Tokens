using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Token.TokenBasico.Models;
using Token.TokenBasico.Services;

namespace Token.TokenBasico.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IJwtAuthenticationService _jwtAuthenticationService;
        public HomeController(IJwtAuthenticationService jwtAuthenticationService)
        {
            _jwtAuthenticationService = jwtAuthenticationService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return Ok("Hello Word!");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthInfo authInfo)
        {
            var token = _jwtAuthenticationService.Authenticate(authInfo.username, authInfo.password);

            if (string.IsNullOrEmpty(token))
            {
                return NoContent();
            }

            return Ok(token);
        }


        [Authorize]
        [HttpGet("GetInfo")]
        public IActionResult GetInfo()
        {
            var id = User.Claims.Where(u => u.Type == "jti").FirstOrDefault().Value;

            return Ok(id);
        }
    }
}
