using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheModernIlluminati.Models;

namespace TheModernIlluminati.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            using (var webClient = new System.Net.WebClient())
            {
                IDictionary<long, TheModernIlluminati.Models.Nobel> allNobels = new Dictionary<long, TheModernIlluminati.Models.Nobel>();
                string nobelJSON = webClient.DownloadString("http://api.nobelprize.org/v1/laureate.json");
                TheModernIlluminati.Models.Nobel nobel = TheModernIlluminati.Models.Nobel.FromJson(nobelJSON);
                List<Laureate> laureate1 = nobel.Laureates;
                List<Laureate> laureate2 = new List<Laureate>();
                foreach (Laureate laureate3 in laureate1)
                {
                    laureate2.Add(laureate3);
                }
                ViewData["Laureates"] = laureate2;


                string jsonString = webClient.DownloadString("http://api.nobelprize.org/v1/country.json");
                Country country = Country.FromJson(jsonString);
                List<Count> random1 = country.Countries;
                List<Count> random2 = new List<Count>();
                foreach (Count coun in random1)
                {
                    random2.Add(coun);
                }
                ViewData["Countries"] = random2;
            }
        }
    }
}
