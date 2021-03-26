using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using QuickType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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
