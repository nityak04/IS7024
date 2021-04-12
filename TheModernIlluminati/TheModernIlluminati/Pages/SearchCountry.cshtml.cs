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

        public bool searchFinished { get; set; }
        [BindProperty]  
        public string CountrySearch { get; set;}
        public string CountryCode { get; set; }
        public string CategorySelected { get; set; }
        public string LevelOfSearch{ get; set; }
        public Category Category { get; set; }



        public void OnGet()
        {

        }
         public void OnPost()
        {

           
            using (var webClient = new WebClient())
            {
                String countryJSON = webClient.DownloadString("http://api.nobelprize.org/v1/country.json");
                    Country country = Country.FromJson(countryJSON);
                    List<Count> countryList= country.Countries;
                int i = 0;
                if (!String.IsNullOrEmpty(CountrySearch))
                {
                    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                    var TitleCountry = myTI.ToTitleCase(CountrySearch);
                    foreach (var coun in countryList)
                    {
                        if (TitleCountry == coun.Name && i == 0)
                        {
                            CountryCode = coun.Code;
                            i++;
                        }
                    }
                }

                IDictionary<long, TheModernIlluminati.Models.Nobel> allNobels = new Dictionary<long, TheModernIlluminati.Models.Nobel>();
                string nobelJSON = webClient.DownloadString("http://api.nobelprize.org/v1/laureate.json");
                TheModernIlluminati.Models.Nobel nobel = TheModernIlluminati.Models.Nobel.FromJson(nobelJSON);
                List<TheModernIlluminati.Models.Laureate> laureateAll = nobel.Laureates;
                List<TheModernIlluminati.Models.Laureate> laureateCategory_Year = new List<TheModernIlluminati.Models.Laureate>();
                List<TheModernIlluminati.Models.Laureate> laureateYear = new List<TheModernIlluminati.Models.Laureate>();
                List<TheModernIlluminati.Models.Laureate> laureateCountry_Category_Year = new List<TheModernIlluminati.Models.Laureate>();
                List<TheModernIlluminati.Models.Laureate> laureateCountry_Year = new List<TheModernIlluminati.Models.Laureate>();
                string Year = Request.Form["Year"];
                long Year1 = int.Parse(Year);
                string categoryFromScreen = Request.Form["Category"];
                TextInfo myCAT = new CultureInfo("en-US", false).TextInfo;
                var TitleCategory = myCAT.ToTitleCase(categoryFromScreen);

                foreach (var laureateInLoop in laureateAll)
                {
                    if (laureateInLoop.BornCountryCode == CountryCode)
                    {
                        foreach (var prize in laureateInLoop.Prizes)
                        {
                            if (prize.Category.ToString() ==  TitleCategory && prize.Year == Year1)
                            {
                                laureateCountry_Category_Year.Add(laureateInLoop);
                                LevelOfSearch = "1";
                            }
                            else if (prize.Year == Year1 && TitleCategory == "None")
                            {
                                laureateCountry_Year.Add(laureateInLoop);
                                LevelOfSearch = "2";
                            }
                        }
                    }
                    else if(String.IsNullOrEmpty(CountryCode))
                    {
                        foreach (var prizes in laureateInLoop.Prizes)
                        {
                            if (prizes.Category.ToString() == TitleCategory && prizes.Year == Year1)
                            {
                                laureateCategory_Year.Add(laureateInLoop);
                                LevelOfSearch = "3";
                            }
                            else if (prizes.Year == Year1 && TitleCategory == "None")
                            {
                                laureateYear.Add(laureateInLoop);
                                LevelOfSearch = "4";

                            }
                        }
                    }

                }

                switch (LevelOfSearch)
                {
                    case "1":
                        ViewData["filteredLaureate"] = laureateCountry_Category_Year ;
                        break;
                    case "2":
                        ViewData["filteredLaureate"] = laureateCountry_Year;
                        break;
                    case "3":
                        ViewData["filteredLaureate"] = laureateCategory_Year;
                        break;
                    case "4":
                        ViewData["filteredLaureate"] = laureateYear;
                        break;
                }
                
            }
            if (!String.IsNullOrEmpty(LevelOfSearch))
            {
                searchFinished = true;
            }
            
        }
    }
}
