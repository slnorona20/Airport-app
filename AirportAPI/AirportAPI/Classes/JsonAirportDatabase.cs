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

        public async Task<User> AddUserAsync(User user)
        {
            User newUser = await ExecuteDBAccess(fileLock, () => 
            {
                List<User> users = GetUsers(); 
                User lastUser = users.OrderBy(x => x.Id).ToList()[^1];

                users.Add(new User
                {
                    Id = lastUser.Id + 1,
                    Name = user.Name,
                    Surname = user.Surname,
                    BirthDate = user.BirthDate,
                    Nationality = user.Nationality,
                    Email = user.Email
                });

                SaveUsers(users);

                return users[^1];
            });

            return newUser;
        }

        public Task<List<Flight>> GetFlightsAsync(DateTime departureDate, string searchOriginCountry, string searchDestinationCountry)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteUserAsync(int userId)
        {
            User user = await ExecuteDBAccess(fileLock, () =>
            {
                List<User> users = GetUsers();
                User user = users.FirstOrDefault(x => x.Id == userId);

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

            return user.Id;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            User user = await ExecuteDBAccess(fileLock, () =>
            {
                List<User> users = GetUsers(); 
                User user = users.FirstOrDefault(x => x.Id == userId);

                return user;
            });

            if (user == null)
            {
                throw new ObjectNotFoundException($"User not found");
            }

            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            User updatedUser = await ExecuteDBAccess(fileLock, () =>
            {
                List<User> users = GetUsers(); 
                User theUser = users.FirstOrDefault(x => x.Id == user.Id);

                if(theUser == null)
                {
                    return null;
                }

                theUser.Id = user.Id;
                theUser.Surname = user.Surname;
                theUser.BirthDate = user.BirthDate;
                theUser.Nationality = user.Nationality;
                theUser.Email = user.Email;

                theUser = users.Where(x => x.Id == user.Id).ToList()[0];
                SaveUsers(users);

                return new User 
                {
                    Id      = theUser.Id,
                    Surname = user.Surname,
                    BirthDate = user.BirthDate,
                    Nationality = user.Nationality,
                    Email = user.Email
                };
            });

            if (updatedUser == null)
            {
                throw new ObjectNotFoundException($"User not found");
            }

            return updatedUser;
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Flight> GetFlightAsync(int flightId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Flight>> GetAllFlightsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Flight> AddFlightAsync(Flight flight)
        {
            throw new NotImplementedException();
        }

        public Task<Flight> UpdateFlightAsync(Flight flight)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteFlightAsync(int flightId)
        {
            throw new NotImplementedException();
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
