using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineForum.Controllers;
using OnlineForum.Models;

namespace OnlineForum.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly Database _dbContext;

        public RegisterModel(Database dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var res = _dbContext.RegisterUser(Input.Email, Input.Password);

            if (res == null)
            {
                ModelState.AddModelError(string.Empty, "Account already exists.");
                return Page();
            }

            await HomeController.Authenticate(HttpContext, res);
            return Redirect("Home");
        }

        public class InputModel
        {
            [Required]
            [StringLength(100)]
            [Display(Name = "Telegram")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}