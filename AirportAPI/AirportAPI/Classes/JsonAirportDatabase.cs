using AirportAPI.Exceptions;
using AirportAPI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;

namespace AirportAPI.Classes
{
    public class JsonAirportDatabase : IAirportDatabase
    {
        protected string DBFileName { get; }
        private readonly object fileLock = new object();
        private IConfigurationRoot Database;

        public JsonAirportDatabase()
        {
            DBFileName = Startup.UserDBName;

            string currentDirectory = Directory.GetCurrentDirectory();
            var configBuilder = new ConfigurationBuilder().SetBasePath(currentDirectory).AddJsonFile(DBFileName);

            Database = configBuilder.Build();
        }

        public User AddUser(User user)
        {
            User newUser = ExecuteDBAccess(fileLock, () => 
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

        public int DeleteUser(int userId)
        {
            User user = ExecuteDBAccess(fileLock, () =>
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

        public User GetUser(int userId)
        {
            User user = ExecuteDBAccess(fileLock, () =>
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

        public User UpdateUser(User user)
        {
            User updatedUser = ExecuteDBAccess(fileLock, () =>
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
            return Database.GetSection("Users:Objects").Get<List<User>>(); 
        }

        protected void SaveUsers(List<User> users)
        {
            Database["Users:Objects"] = JsonConvert.SerializeObject(users);
            File.WriteAllText(DBFileName, JsonConvert.SerializeObject(Database.AsEnumerable(), Formatting.Indented));
        }

        protected User ExecuteDBAccess(object fileLock, Func<User> function)
        {
            User result;

            lock (fileLock)
            {
                result = function();
                return result;
            }
        }
    }
}
