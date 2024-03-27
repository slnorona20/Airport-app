using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AirportAPI.Exceptions;
using AirportAPI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace AirportAPI.Classes
{
    public class JsonAirportDatabase : IAirportDatabase
    {
        protected string DBFileName { get; }
        private readonly object fileLock = new object();
        private IConfigurationRoot DatabaseConfig;

        public JsonAirportDatabase()
        {
            DBFileName = Startup.UserDBName;

            string currentDirectory = Directory.GetCurrentDirectory();
            var configBuilder = new ConfigurationBuilder().SetBasePath(currentDirectory).AddJsonFile(DBFileName);

            DatabaseConfig = configBuilder.Build();
        }

        public async Task<User> AddUser(User user)
        {
            User newUser = await ExecuteDBAccess(fileLock, () => 
            {
                List<User> users = GetUsers(); 
                User lastUser = users.OrderBy(x => x.UserId).ToList()[^1];

                users.Add(new User
                {
                    UserId = lastUser.UserId + 1,
                    UserName = user.UserName,
                    Origin = user.Origin,
                    Destination = user.Destination
                });

                SaveUsers(users);

                return users[^1];
            });

            return newUser;
        }

        public async Task<int> DeleteUser(int userId)
        {
            User user = await ExecuteDBAccess(fileLock, () =>
            {
                List<User> users = GetUsers();
                User user = users.FirstOrDefault(x => x.UserId == userId);

                if (user == null)
                {
                    return null;
                }

                users.Remove(user);
                SaveUsers(users);

                return user;
            });

            if (user == null)
            {
                throw new ObjectNotFoundException($"User not found");
            }

            return user.UserId;
        }

        public async Task<User> GetUser(int userId)
        {
            User user = await ExecuteDBAccess(fileLock, () =>
            {
                List<User> users = GetUsers(); 
                User user = users.FirstOrDefault(x => x.UserId == userId);

                return user;
            });

            if (user == null)
            {
                throw new ObjectNotFoundException($"User not found");
            }

            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            User updatedUser = await ExecuteDBAccess(fileLock, () =>
            {
                List<User> users = GetUsers(); 
                User theUser = users.FirstOrDefault(x => x.UserId == user.UserId);

                if(theUser == null)
                {
                    return null;
                }

                theUser.UserId = user.UserId;
                theUser.UserName = user.UserName;
                theUser.Origin = user.Origin;
                theUser.Destination = user.Destination;

                theUser = users.Where(x => x.UserId == user.UserId).ToList()[0];
                SaveUsers(users);

                return new User 
                {
                    UserId      = theUser.UserId,
                    UserName    = theUser.UserName,
                    Origin      = theUser.Origin,
                    Destination = theUser.Destination
                };
            });

            if (updatedUser == null)
            {
                throw new ObjectNotFoundException($"User not found");
            }

            return updatedUser;
        }

        protected List<User> GetUsers()
        {
            return DatabaseConfig.GetSection("Users:Objects").Get<List<User>>(); 
        }

        protected void SaveUsers(List<User> users)
        {
            var dbUsers = new List<User>();
            dbUsers.AddRange(users);

            var usersObjects = new { Users = new { Objects = new List<User>(dbUsers) } };
            File.WriteAllText(DBFileName, JsonConvert.SerializeObject(usersObjects, Formatting.Indented));
        }

        protected async Task<User> ExecuteDBAccess(object fileLock, Func<User> function)
        {
            User result;

            Task<User> task = Task.Run(() =>
            {
                lock (fileLock)
                {
                    result = function();
                    return result;
                }
            });
            result = await task;

            return result;
        }
    }
}
