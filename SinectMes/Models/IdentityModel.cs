using System;
using Microsoft.AspNetCore.Identity;

namespace SinectMes.Models
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser()
        {
        }
    }

    public class ApplicationRole:IdentityRole<Guid>
    {
        
    }
}
