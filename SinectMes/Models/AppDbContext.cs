using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SinectMes.Models
{
	public class UserDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
