using OnlineStore.Domain.Abstract;
using OnlineStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Concrete
{
    public class UserRepository: IUserRepository
    {
        private EFDbContext context = new EFDbContext();
        
        public User GetUser(string username, string password){
            User user = context.Users.Where(u => u.Email.Equals(username) && u.Password.Equals(password)).FirstOrDefault();
            if(user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public User GetUserByEmail(string email)
        {
            User user = context.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
