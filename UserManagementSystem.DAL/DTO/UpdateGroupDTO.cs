using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementSystem.DAL.DTO
{
    public class UpdateGroupDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<int>? SelectedUsers { get; set; } = new();
        public List<int>? SelectedPermissions { get; set; } = new();
    }
}