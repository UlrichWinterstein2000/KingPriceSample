using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.UI.Models
{
    public class Permission
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
