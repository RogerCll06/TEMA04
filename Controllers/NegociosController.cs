using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Tema3Front.Models;

namespace Tema3Front.Controllers
{
    public class NegociosController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Cliente> temporal = new List<Cliente>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:19983/api/NegociosAPI/");
                HttpResponseMessage response = await client.GetAsync("getClientes");
                string apiResponse = await response.Content.ReadAsStringAsync();
                temporal = JsonConvert.DeserializeObject<List<Cliente>>(apiResponse).ToList();

            }
            return View(await Task.Run(() => temporal));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(await Task.Run(() => new Cliente()));
        }
        [HttpPost]
        public async Task<IActionResult> Create(Cliente reg)
        {
            string mensaje = "";
            using (var client = new HttpClient())
            {
                //tener en cuenta que sea el mismo puerto donde esta corriendo swagger
                client.BaseAddress = new Uri("http://localhost:19983/api/NegociosAPI/");
                StringContent content = new StringContent(JsonConvert.SerializeObject(reg), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("insertCliente", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = apiResponse;

            }
            ViewBag.mensaje = mensaje;  
            return View(await Task.Run(() => reg));
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            Cliente reg = new Cliente();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:19983/api/NegociosAPI/");
                HttpResponseMessage response = await client.GetAsync("getCliente/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                reg = JsonConvert.DeserializeObject<Cliente>(apiResponse);
            }

            return View(await Task.Run(() => reg));
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(Cliente reg)
        {
            string mensaje = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:19983/api/NegociosAPI/");
                StringContent content = new StringContent(JsonConvert.SerializeObject(reg),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync("updateCliente", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = apiResponse;
            }

            ViewBag.mensaje = mensaje;
            return View(await Task.Run(() => reg));
        }
    }
}
