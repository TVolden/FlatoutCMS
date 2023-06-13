using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FlatoutCMS.Admin.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration configuration;

        public IndexModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet()
        {

        }
    }
}
