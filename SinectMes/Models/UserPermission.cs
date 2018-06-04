using SinectMes.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinectMes.Models
{
    public class UserPermission
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public Permission Permission { get; set; }
    }
}
