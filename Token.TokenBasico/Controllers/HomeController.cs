using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Token.TokenBasico.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Hello Word!");
        }
    }
}
