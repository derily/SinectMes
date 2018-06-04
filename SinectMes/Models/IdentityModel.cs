using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SinectMes.Models
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser()
        {
        }
        public List<UserPermission> Permissions { get; set; }
    }

    public class ApplicationRole:IdentityRole<Guid>
    {
        
    }
}
