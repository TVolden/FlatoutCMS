using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlatoutCMS.Admin.Areas.Admin.Pages
{
    [Authorize(AuthenticationSchemes = "FlatoutAdmin")]
    public class IndexModel : PageModel
    {
        public void OnGet() { }
    }
}
