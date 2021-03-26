using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheModernIlluminati.Models;

namespace TheModernIlluminati.Pages
{
    public class CountryModel : PageModel
    {
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