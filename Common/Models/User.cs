using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public enum UserRole
    {
        Admin,
        Customer
    }

    public class User
    {
        public string Username { get; set; }
        public UserRole Role { get; set; }
    }
}
