using AirportAPI.Exceptions;
using AirportAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AirportAPI.Classes
{
    public class FileDatabase : IAirportDatabase
    {
        protected IConfiguration Configuration { get; }
        protected List<User> userList;

        public FileDatabase(IConfiguration configuration)
        {
            Configuration = configuration;

            userList = Configuration.GetSection("Users:Objects").Get<List<User>>();
        }

        public User GetUser(int userId) 
        {
            var existingUser = userList.FirstOrDefault(x => x.UserId == userId);
            if (existingUser == null)
            {
                throw new ObjectNotFoundException($"User not found");
            }

            return new User
            {
                UserId = existingUser.UserId,
                UserName = existingUser.UserName,
                Origin = existingUser.Origin,
                Destination = existingUser.Destination
            };
        }

        public User AddUser(User user) 
        {
            var lastUser = userList[^1];

            var newUser = new User
            {
                UserId = lastUser.UserId + 1,
                UserName = user.UserName,
                Origin = user.Origin,
                Destination = user.Destination
            };

            userList.Add(newUser);
            Update("Users:Objects", userList);

            return newUser;
        }

        public User UpdateUser(User user)
        {
            var existingUser = userList.FirstOrDefault(x => x.UserId == user.UserId);
            if (existingUser == null)
            {
                throw new ObjectNotFoundException($"User not found");
            }

            existingUser.UserName = user.UserName;
            existingUser.Origin = user.Origin;
            existingUser.Destination = user.Destination;

            Update("Users:Objects", userList);

            return new User
            {
                UserId = existingUser.UserId,
                UserName = existingUser.UserName,
                Origin = existingUser.Origin,
                Destination = existingUser.Destination
            };
        }

        public int DeleteUser(int userId)
        {
            var user = userList.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                throw new ObjectNotFoundException($"User not found");
            }
            userList.Remove(user);

            Update("Users:Objects", userList);

            return userId;
        }

        protected void Update(string entry, List<User> userList)
        {
            //Configuration["Users:Objects"] = userList.ToString();

            var userListStr = userList.ToString();
            Configuration["Users:Objects"] = userListStr;

            File.WriteAllText("appsettings.json", JsonConvert.SerializeObject(Configuration.AsEnumerable(), Formatting.Indented));
        }
    }
}
