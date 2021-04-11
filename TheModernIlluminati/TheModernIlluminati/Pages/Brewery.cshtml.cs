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
                string breweryData = webClient.DownloadString("https://api.openbrewerydb.org/breweries?by_city=cincinnati&brewery_type=regional");
                Welcome[] brewery = Welcome.FromJson(breweryData);
                ViewData["MyBreweryAPI"] = brewery;
            }
        }
    }
}
