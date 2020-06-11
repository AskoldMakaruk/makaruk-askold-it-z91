using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineForum.Controllers;
using OnlineForum.Models;

namespace OnlineForum.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly Database _context;


        public LoginModel(Database context)
        {
            _context = context;
        }

        [BindProperty] public InputModel Input { get; set; }

        [TempData] public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(ErrorMessage)) ModelState.AddModelError(string.Empty, ErrorMessage);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            if (ModelState.IsValid)
            {
                var account = _context.AuthUser(Input.Name, Input.Password);

                if (account != null)
                {
                    await HomeController.Authenticate(HttpContext, account);
                    return Redirect(Url.Content("~/"));
                }
            }

            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return Page();
        }

        public class InputModel
        {
            [Required]
            [DisplayName("Имя")]
            [Display(Name = "Имя")]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }
        }
    }
}