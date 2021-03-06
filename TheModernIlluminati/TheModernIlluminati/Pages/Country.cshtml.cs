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
                string countryData = webClient.DownloadString("http://api.nobelprize.org/v1/country.json");
                Country country = Country.FromJson(countryData);
                List<Count> countries = country.Countries;
                ViewData["Countries"] = countries;
            }
        }
    }
}