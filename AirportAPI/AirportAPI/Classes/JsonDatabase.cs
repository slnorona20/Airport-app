using AirportAPI.Exceptions;
using AirportAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;

namespace AirportAPI.Classes
{
    public class JsonDatabase : IAirportDatabase
    {
        protected string FileName { get; }

        private readonly object fileLock = new object();

        public JsonDatabase(string fileName)
        {
            FileName = fileName;
        }

        public User AddUser(User user)
        {
            User user1 = new User();


            User newUser = ExecuteWriteDBAccess(fileLock, () => 
            {
                List<User> sortedUsers = GetUsers(FileName);

                User lastUser = sortedUsers.Sort()[^1];

                var newUser = new User
                {
                    UserId = lastUser.UserId + 1,
                    UserName = user.UserName,
                    Origin = user.Origin,
                    Destination = user.Destination
                };

                sortedUsers.Add(newUser);
                SaveUsers(sortedUsers);

                return newUser;
            });

            return newUser;
        }

        private void SaveUsers(List<User> users)
        {
            throw new NotImplementedException();
        }

        private List<User> GetUsers(string fileName)
        {
            throw new NotImplementedException();
        }

        private User ExecuteWriteDBAccess(object fileLock, Func<User> function)
        {
            User result;

            lock (fileLock)
            {
                // Perform write operations on the file
                result = function();
                return result;
            }
        }

        /*
        private User ExecuteMutexDBAccess(string mutexId, List<User> userList, Func<List<User>, User> function)
        {
            User result;

            // Create a mutex with a unique name
            using (var mutex = new Mutex(initiallyOwned: false, mutexId))
            {
                bool hasHandle = false;
                try
                {
                    try
                    {
                        // Attempt to acquire the mutex
                        hasHandle = mutex.WaitOne(5000, false);
                        if (hasHandle == false)
                        {
                            throw new TimeoutException("Timeout waiting for exclusive access to the file.");
                        }
                    }
                    catch (AbandonedMutexException)
                    {
                        // The mutex was abandoned in another process, possibly due to a crash
                        hasHandle = true;
                    }

                    result = function(userList);
                }
                finally
                {
                    if (hasHandle)
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }

            return result;
        }
        */

        public int DeleteUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public User GetUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public User UpdateUser(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
