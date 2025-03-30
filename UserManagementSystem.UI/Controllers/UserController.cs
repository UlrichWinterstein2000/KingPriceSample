using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UserManagementSystem.UI.Models;

namespace UserManagementSystem.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("UserApi");
            var response = await client.GetAsync("user");
            if (!response.IsSuccessStatusCode) return View(new List<User>());

            var json = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(json);
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid) return View(user);

            var client = _httpClientFactory.CreateClient("UserApi");
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("user", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("UserApi");
            var response = await client.GetAsync($"user/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(json);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid) return View(user);

            var client = _httpClientFactory.CreateClient("UserApi");
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"user/{user.Id}", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("UserApi");
            var response = await client.DeleteAsync($"user/{id}");
            return RedirectToAction("Index");
        }
    }
}
