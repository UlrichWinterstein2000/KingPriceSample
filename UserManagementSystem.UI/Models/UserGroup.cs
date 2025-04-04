﻿using UserManagementSystem.UI.Models;

namespace UserManagementSystem.DAL.Entities
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
