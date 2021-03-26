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
                string nobelJSON = webClient.DownloadString("http://api.nobelprize.org/v1/laureate.json");
                TheModernIlluminati.Models.Nobel nobel = TheModernIlluminati.Models.Nobel.FromJson(nobelJSON);
                List<TheModernIlluminati.Models.Laureate> laureate1 = nobel.Laureates;
                List<TheModernIlluminati.Models.Laureate> laureate2 = new List<TheModernIlluminati.Models.Laureate>();
                foreach (TheModernIlluminati.Models.Laureate laureate3 in laureate1)
                {
                    laureate2.Add(laureate3);
                }
                ViewData["Laureates"] = laureate2;
            }
        }
    }
}
