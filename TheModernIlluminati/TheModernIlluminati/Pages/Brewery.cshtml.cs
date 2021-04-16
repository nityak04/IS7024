using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuickType;
namespace TheModernIlluminati.Pages
{
    public class BreweryModel : PageModel
    {
        public void OnGet()
        {
            using (var webClient = new System.Net.WebClient())
            {
                string breweryData = webClient.DownloadString("https://justbrewit2021.azurewebsites.net/OtherAPI");
                Welcome[] brewery = Welcome.FromJson(breweryData);
                ViewData["MyBreweryAPI"] = brewery;
            }
        }
    }
}
