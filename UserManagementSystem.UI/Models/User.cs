﻿using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.UI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
