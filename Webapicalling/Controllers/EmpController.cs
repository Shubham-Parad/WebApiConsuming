using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Webapicalling.Models;

namespace Webapicalling.Controllers
{
    public class EmpController : Controller
    {
        HttpClient client;
        public EmpController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }
        public IActionResult Index()
        {
            List<Emp> empList = new List<Emp>();
            string url = "https://localhost:44365/api/Emp/GetEmps";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<List<Emp>>(jsondata);
                if (obj != null)
                {
                    empList = obj;
                }
            }
            return View(empList);
        }

        public IActionResult AddEmp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmp(Emp e)
        {
            string url = "https://localhost:44365/api/Emp/AddEmp/";
            var jsondata = JsonConvert.SerializeObject(e);
            StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, stringContent).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult DeleteEmp(int id)
        {
            string url = "https://localhost:44365/api/Emp/DelEmp/";
            HttpResponseMessage response = client.DeleteAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult UpdateEmp(int id)
        {
            string url = "https://localhost:44365/api/Emp/GetEmps/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<Emp>(jsondata);
                return View(obj);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateEmp(Emp e)
        {
            string url = "https://localhost:44365/api/Emp/UpdateEmp/";
            var jsondata = JsonConvert.SerializeObject(e);
            StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(url, stringContent).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
