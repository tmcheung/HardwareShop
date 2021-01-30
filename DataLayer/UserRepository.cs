using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Models;
using DataLayer.EF;

namespace DataLayer
{
    public class UserRepository : Repository
    {
        public UserRepository(IShopContext context) : base(context)
        {

        }

        public User Get(string username)
        {
            return context.User.Single(u => u.Username.Equals(username));
        }

        public void Create(string username, UserRole role)
        {
            context.User.Add(new User { Username = username, Role = role });
            context.SaveChanges();
        }
    }
}
