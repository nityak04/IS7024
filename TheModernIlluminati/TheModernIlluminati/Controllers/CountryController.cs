using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheModernIlluminati.Controllers
{
    [Route("api")]
    [ApiController]
    public class CountryController : Controller
    {
       // [Route("country/search/{countrycode}")]
        //public ActionResult<Country[]> SearchLaureates(string countrycode)
        //{
        //    using (var webClient = new WebClient())
        //    {
        //        String countryData = webClient.DownloadString("https://data.cityofchicago.org/resource/s6ha-ppgi.json");
        //        var result= 

        //        houses = AffordableHouse.FromJson(houseJSON);
        //        housesFiltered = houses.Where(x => x.ZipCode == zipCode).ToArray();
        //        return housesFiltered;
        //    }
        //}
    }
}
