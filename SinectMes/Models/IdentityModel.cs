using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SinectMes.Models
{
    public class ApplicationUser:IdentityUser<int>
    {
        public ApplicationUser()
        {
            
        }
        public List<UserPermission> Permissions { get; set; }
    }

    public class ApplicationRole:IdentityRole<int>
    {
        public ApplicationRole()
        {
            CreateTime = DateTime.Now;
        }
        public string ChineseName { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
