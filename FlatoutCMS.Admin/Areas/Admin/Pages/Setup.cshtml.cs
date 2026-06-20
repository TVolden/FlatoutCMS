using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlatoutCMS.Admin.Areas.Admin.Pages
{
    public class SetupModel : PageModel
    {
        private readonly PasswordFileService passwordFileService;

        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        [BindProperty] public string ConfirmPassword { get; set; }
        public string ErrorMessage { get; set; }

        public SetupModel(PasswordFileService passwordFileService)
        {
            this.passwordFileService = passwordFileService;
        }

        public IActionResult OnGet()
        {
            if (passwordFileService.AnyUserExists())
                return RedirectToPage("Login");
            return Page();
        }

        public IActionResult OnPost()
        {
            if (passwordFileService.AnyUserExists())
                return RedirectToPage("Login");

            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Username is required.";
                return Page();
            }
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
            {
                ErrorMessage = "Password must be at least 8 characters.";
                return Page();
            }
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return Page();
            }

            passwordFileService.SetPassword(Username, Password);
            return RedirectToPage("Login");
        }
    }
}
