using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheModernIlluminati.Models;
using System.Globalization;

namespace TheModernIlluminati.Pages
{
    public class SearchCountryModel : PageModel
    {
        public string _CountrySearch;

        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
        public bool searchFinished { get; set; }
        [BindProperty]

        
        public string CountrySearch { get; set;}
        public string CountryCode { get; set; }

       
    public static string ToTitleCase(string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
    }

    public void OnGet()
        {

        }
         public void OnPost()
        {

           
            using (var webClient = new WebClient())
            {
                String countryJSON = webClient.DownloadString("http://api.nobelprize.org/v1/country.json");
                    Country country = Country.FromJson(countryJSON);
                    List<Count> random1 = country.Countries;
                int i = 0;
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                var TitleCountry = myTI.ToTitleCase(CountrySearch);
                foreach (var coun in random1)
                    {
                       if ( TitleCountry == coun.Name && i == 0)
                         {
                            CountryCode = coun.Code ;
                            i++ ;        
                         }
                    }

                IDictionary<long, TheModernIlluminati.Models.Nobel> allNobels = new Dictionary<long, TheModernIlluminati.Models.Nobel>();
                string nobelJSON = webClient.DownloadString("http://api.nobelprize.org/v1/laureate.json");
                TheModernIlluminati.Models.Nobel nobel = TheModernIlluminati.Models.Nobel.FromJson(nobelJSON);
                List<TheModernIlluminati.Models.Laureate> laureate1 = nobel.Laureates;
                List<TheModernIlluminati.Models.Laureate> laureate2 = new List<TheModernIlluminati.Models.Laureate>();
               
                foreach (var laureate4 in laureate1)
                {
                    if (laureate4.BornCountryCode == CountryCode)
                    {
                        laureate2.Add(laureate4);
                    }

                }
                ViewData["filteredLaureate"] = laureate2;

            }
            searchFinished = true;
            
        }
    }
}
