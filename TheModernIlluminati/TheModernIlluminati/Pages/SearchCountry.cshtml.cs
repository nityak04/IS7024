using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheModernIlluminati.Models;

namespace TheModernIlluminati.Pages
{
    public class SearchCountryModel : PageModel
    {
        
        [BindProperty]
        public string CountrySearch { get; set; }
        public string CountryCode { get; set; }
        public void OnGet()
        {

        }
         public IActionResult OnPost()
        {
            if (CountrySearch == null) { 
                
                return Page(); 
            
            }
           
            using (var webClient = new WebClient())
            {
                String countryJSON = webClient.DownloadString("http://api.nobelprize.org/v1/country.json");
                    Country country = Country.FromJson(countryJSON);
                    List<Count> countries = country.Countries;
                int i = 0;
                foreach (var coun in countries)
                    {
                       if (CountrySearch == coun.Name && i == 0)
                         {
                            CountryCode = coun.Code ;
                            i++ ;        
                         }
                    }

                IDictionary<long, TheModernIlluminati.Models.Nobel> allNobels = new Dictionary<long, TheModernIlluminati.Models.Nobel>();
                string nobelJSON = webClient.DownloadString("http://api.nobelprize.org/v1/laureate.json");
                TheModernIlluminati.Models.Nobel nobel = TheModernIlluminati.Models.Nobel.FromJson(nobelJSON);
                List<TheModernIlluminati.Models.Laureate> nobelLaureates = nobel.Laureates;
                List<TheModernIlluminati.Models.Laureate> laureates = new List<TheModernIlluminati.Models.Laureate>();
               
                foreach (var laureate in nobelLaureates)
                {
                    if (laureate.BornCountryCode == CountryCode && laureate.Gender.ToString()!= "Org")
                    {
                       
                        laureates.Add(laureate);
                    }

                }

                if (laureates.Count() != 0)
                {
                    ViewData["filteredLaureate"] = laureates;
                }
               

            }
            
            return Page();
        }
    }
}
