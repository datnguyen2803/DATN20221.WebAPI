using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public UserModel()
        {
            Name = string.Empty;
            Password = string.Empty;
        }

        public UserTable ToUserTable()
        {
            return new UserTable
            {
                Id = Guid.NewGuid(),
                Name = Name,
                Password = Password
            };
        }

    }
}
