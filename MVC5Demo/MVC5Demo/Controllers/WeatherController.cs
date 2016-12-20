using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC5Demo.Controllers
{
    public class WeatherController : Controller
    {
        // GET: Weather
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ShowAsync()
        {
            var str = await GetWeather();

            ViewBag.Weather = str;

            return View();
        }

        public async Task<string> GetWeather()
        {
            var uri = "http://api.jirengu.com/weather.php";
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri);
                return (await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> GetGithubReadme()
        {
            var uri = "http://api.jirengu.com/readme2html.php?url=https://github.com/aspnet/Mvc";
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri);
                return (await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<ActionResult> ShowLongTaskAsync()
        {
            var task1 = GetWeather();
            var task2 = GetGithubReadme();

            await Task.WhenAll(task1, task2);

            ViewBag.Weather = task1.Result;
            ViewBag.Readme = task2.Result;

            return View();
        }
    }
}