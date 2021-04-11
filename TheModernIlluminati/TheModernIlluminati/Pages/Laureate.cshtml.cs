using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheModernIlluminati.Models;

namespace TheModernIlluminati.Pages
{
    public class LaureateModel : PageModel
    {
        public void OnGet()
        {
            using (var webClient = new System.Net.WebClient())

            {
                IDictionary<long, TheModernIlluminati.Models.Nobel> allNobels = new Dictionary<long, TheModernIlluminati.Models.Nobel>();
                string nobelLaureates = webClient.DownloadString("http://api.nobelprize.org/v1/laureate.json");
                TheModernIlluminati.Models.Nobel nobelLaureate = TheModernIlluminati.Models.Nobel.FromJson(nobelLaureates);
                List<TheModernIlluminati.Models.Laureate> laureateDetails = nobelLaureate.Laureates;
                ViewData["Laureates"] = laureateDetails;
            }
        }
    }
}
