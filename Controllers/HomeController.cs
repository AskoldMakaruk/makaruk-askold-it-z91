using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineForum.Models;

namespace OnlineForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly Database _db;

        public HomeController(Database db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View("Index", _db.GetBoards());
        }

        [Authorize(Roles = "admin")]
        public IActionResult CreateBoard(string sub, string com)
        {
            var user = _db.FindUser(int.Parse(User.Claims.First().Value));
            _db.AddBoard(user, sub, com);
            return Index();
        }

        [Authorize]
        public IActionResult PostReply(int boardId, string text)
        {
            var accountid = int.Parse(HttpContext.User.Identity.Name);
            _db.AddPost(accountid, boardId, text);
            return GetBoard(boardId);
        }

        [HttpGet("board/{id}")]
        public IActionResult GetBoard([FromRoute] int id)
        {
            var board = _db.GetBoard(id);
            return View("Board", board);
        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return returnUrl != null ? Redirect(returnUrl) : Index();
        }

        public static async Task Authenticate(HttpContext context, User account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, account.Id.ToString())
            };
            if (account.IsAdmin) claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            if (context != null)
            {
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id),
                    new AuthenticationProperties
                    {
                        IsPersistent = true
                    });
            }
        }
    }
}