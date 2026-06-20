using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace FlatoutCMS.Admin.Areas.Admin.Pages
{
    public class EditorModel : PageModel
    {
        public string[] accept = {@".\Data", @".\Pages", @".\Views", @".\Models"};

        public void OnGet()
        {
        }
    }
}
