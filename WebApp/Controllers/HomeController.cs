using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApp.Models;
using System.Net.Http.Headers;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {


            string baseUrlJWT = "https://localhost:44349";
            HttpClient clientJWT = new HttpClient();
            clientJWT.BaseAddress = new Uri(baseUrlJWT);
            var contentTypeJWT = new MediaTypeWithQualityHeaderValue("application/json");
            clientJWT.DefaultRequestHeaders.Accept.Add(contentTypeJWT);

            UserInfo user = new UserInfo();
            user.Email = "Admin@abc.com";
            user.Password = "Admin";

            string stringDataJWT = JsonConvert.SerializeObject(user);
            var contentDataJWT = new StringContent(stringDataJWT, System.Text.Encoding.UTF8, "application/json");
            //var contentDataJWT = new StringContent(stringDataJWT, System.Text.Encoding.UTF8, "text/plain");

            HttpResponseMessage responseJWT = clientJWT.PostAsync("/api/token", contentDataJWT).Result;
            string stringJWT = JsonConvert.DeserializeObject(responseJWT.Content.ReadAsStringAsync().Result).ToString();
            HttpContext.Session.Set("token", System.Text.Encoding.ASCII.GetBytes(stringJWT));
            //HttpContext.Session.Remove("token");

            string baseUrl = "https://localhost:44349";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            //var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            //client.DefaultRequestHeaders.Accept.Add(contentType);

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", stringJWT);

            HttpResponseMessage response = client.GetAsync("/api/products").Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<Products> products = JsonConvert.DeserializeObject<List<Products>>(stringData);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            else
            {
                string strTable = "<table border='1' cellpadding='10'>";
                foreach (Products product in products)
                {
                    strTable += "<tr>";
                    strTable += "<td>";
                    strTable += product.ProductId;
                    strTable += "</td>";
                    strTable += "<td>";
                    strTable += product.Name;
                    strTable += "</td>";
                    strTable += "<td>";
                    strTable += product.Color;
                    strTable += "</td>";
                    strTable += "</tr>";

                }
                strTable += "</table>";

                ViewBag.Message = strTable;
            }



            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
