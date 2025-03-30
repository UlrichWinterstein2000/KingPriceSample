using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UserManagementSystem.UI.Models;

namespace UserManagementSystem.UI.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PermissionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("UserApi");
            var response = await client.GetAsync("permission");
            if (!response.IsSuccessStatusCode) return View(new List<Permission>());

            var json = await response.Content.ReadAsStringAsync();
            var permissions = JsonConvert.DeserializeObject<List<Permission>>(json);
            return View(permissions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Permission permission)
        {
            if (!ModelState.IsValid) return View(permission);

            var client = _httpClientFactory.CreateClient("UserApi");
            var json = JsonConvert.SerializeObject(permission);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("permission", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            return View(permission);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("UserApi");
            var response = await client.GetAsync($"permission/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var permission = JsonConvert.DeserializeObject<Permission>(json);
            return View(permission);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Permission permission)
        {
            if (!ModelState.IsValid) return View(permission);

            var client = _httpClientFactory.CreateClient("UserApi");
            var json = JsonConvert.SerializeObject(permission);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"permission/{permission.Id}", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            return View(permission);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("UserApi");
            var response = await client.DeleteAsync($"permission/{id}");
            return RedirectToAction("Index");
        }
    }
}
