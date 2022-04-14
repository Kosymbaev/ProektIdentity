using Microsoft.AspNetCore.Identity;
using System;

namespace Laba4.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public bool Status { get; set; }
        public DateTime Register { get; set; }
        public DateTime LastLogin { get; set; }
        public bool Selected { get; set; }
    }
}
