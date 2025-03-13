using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.AdminRole)]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _Usermanager;
        public UserController(UserManager<IdentityUser> Usermanager)
        {
            _Usermanager = Usermanager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claims.Value;
            var res = _Usermanager.Users.Where(u => u.Id != userid).ToList();
            return View(res);
        }
    }
}
