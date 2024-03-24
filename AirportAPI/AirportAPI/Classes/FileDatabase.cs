using AirportAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportAPI.Classes
{
    public class FileDatabase : IAirportDatabase
    {
        protected IConfiguration Configuration { get; }
        protected List<User> userList;

        FileDatabase(IConfiguration configuration)
        {
            Configuration = configuration;

            userList = Configuration.GetSection("Users").Get<List<User>>();
        }

        public User GetUser(int userId) 
        {
            try
            {
                var existingUser = userList.First(x => x.UserId == userId);

                return new User
                {
                    UserId = existingUser.UserId,
                    UserName = existingUser.UserName,
                    Origin = existingUser.Origin,
                    Destination = existingUser.Destination
                };
            } 
            catch (Exception ex)
            {
                throw;
            }
        }

        public User AddUser(User user) 
        {
            var lastUser = userList[userList.Count - 1];

            var newUser = new User
            {
                UserId = lastUser.UserId + 1,
                UserName = user.UserName,
                Origin = user.Origin,
                Destination = user.Destination
            };

            userList.Add(newUser);
            return newUser;
        }

        public User UpdateUser(User user)
        {
            try
            {
                var existingUser = userList.First(x => x.UserId == user.UserId);
                existingUser.UserName = user.UserName;
                existingUser.Origin = user.Origin;
                existingUser.Destination = user.Destination;

                return new User
                {
                    UserId = existingUser.UserId,
                    UserName = existingUser.UserName,
                    Origin = existingUser.Origin,
                    Destination = existingUser.Destination
                };
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public int DeleteUser(int userId)
        {
            try
            {
                User user = userList[userId];
                userList.Remove(user);

                return userId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
