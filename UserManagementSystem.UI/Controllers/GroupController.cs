using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UserManagementSystem.UI.DTO;
using UserManagementSystem.UI.Models;

namespace UserManagementSystem.UI.Controllers
{
    public class GroupController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GroupController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("UserApi");
            var response = await client.GetAsync("group");
            if (!response.IsSuccessStatusCode) return View(new List<Group>());

            var json = await response.Content.ReadAsStringAsync();
            var groups = JsonConvert.DeserializeObject<List<Group>>(json);
            return View(groups);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Group group)
        {
            if (!ModelState.IsValid) return View(group);

            var client = _httpClientFactory.CreateClient("UserApi");
            var json = JsonConvert.SerializeObject(group);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("group", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            return View(group);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var group = new Group();
            var client = _httpClientFactory.CreateClient("UserApi");
            var response = await client.GetAsync($"group/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var updateGroup = JsonConvert.DeserializeObject<UpdateGroupDTO>(json);
            group.Name = updateGroup?.Name;
            group.Id = updateGroup!.Id;
            group.SelectedPermissions = updateGroup?.SelectedPermissions;
            group.SelectedUsers = updateGroup?.SelectedUsers;

            var response2 = await client.GetAsync("permission");
            if (!response2.IsSuccessStatusCode) return View(new List<Permission>());
            var json2 = await response2.Content.ReadAsStringAsync();
            group.AvailablePermissions = JsonConvert.DeserializeObject<List<Permission>>(json2) ?? new List<Permission>();

            var response3 = await client.GetAsync("user");
            if (!response3.IsSuccessStatusCode) return View(new List<User>());
            var json3 = await response3.Content.ReadAsStringAsync();
            group.AvailableUsers = JsonConvert.DeserializeObject<List<User>>(json3) ?? new List<User>();

            return View(group);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Group group)
        {
            if (!ModelState.IsValid) return View(group);
            var updateDto = new UpdateGroupDTO
            {
                Id = group.Id,
                Name = group?.Name,
                SelectedUsers = group?.SelectedUsers,
                SelectedPermissions = group?.SelectedPermissions
            };
            var client = _httpClientFactory.CreateClient("UserApi");
            var json = JsonConvert.SerializeObject(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"group/{group!.Id}", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            return View(group);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("UserApi");
            var response = await client.DeleteAsync($"group/{id}");
            return RedirectToAction("Index");
        }
    }
}
