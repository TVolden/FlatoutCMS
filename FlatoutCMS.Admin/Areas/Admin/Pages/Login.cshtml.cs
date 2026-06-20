using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FlatoutCMS.Admin.Areas.Admin.Pages
{
    public class LoginModel : PageModel
    {
        private readonly PasswordFileService passwordFileService;

        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public LoginModel(PasswordFileService passwordFileService)
        {
            this.passwordFileService = passwordFileService;
        }

        public IActionResult OnGet()
        {
            if (!passwordFileService.AnyUserExists())
                return RedirectToPage("Setup");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!passwordFileService.AnyUserExists())
                return RedirectToPage("Setup");

            if (!passwordFileService.ValidateUser(Username, Password))
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "FlatoutAdmin");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("FlatoutAdmin", principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToPage("Index");
        }
    }
}
