using AirportAPI.Models;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Text;

namespace AirportAPI.Classes
{
    public class MySqlAirportDatabase : IAirportDatabase
    {
        protected string ConnectionString { get; }
        MySqlConnection connection;

        public MySqlAirportDatabase()
        {
            ConnectionString = Startup.DBConnectionString;
        }

        public async Task<User> GetUser(int userId)
        {
            Task<User> task = Task.Run(() =>
            {
                User user;
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand()) 
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "CALL spGetUser(@userId)";

                        command.Parameters.AddWithValue("@userId", userId);

                        try
                        {
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    user = new User();

                                    user.Id = reader.GetInt32(0);
                                    user.Name = reader.GetString(1);
                                    user.Surname = reader.GetString(2);
                                    user.Birthday = reader.GetDateTime(3);
                                    user.Nacionality = reader.GetString(4);
                                    user.Email = reader.GetString(5);
                                }
                                else
                                {
                                    throw new Exception("Failed to retrieve a user just added to the DB");
                                }
                            }
                        }
                        catch 
                        {
                            throw;
                        }
                    }
                }
                return user;
            });

            return await task;
        }

        public async Task<User> AddUser(User user)
        {
            Task<User> task = Task.Run(async () =>
            {
                User newUser;
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    var sql = "CALL spAddUser(@username, @usersurname, @birthdate, @nationality, @email)";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@username",    user.Name);
                        command.Parameters.AddWithValue("@usersurname", user.Surname); 
                        command.Parameters.AddWithValue("@birthdate",   user.Birthday);
                        command.Parameters.AddWithValue("@nationality", user.Nacionality);
                        command.Parameters.AddWithValue("@email",       user.Email);

                        try
                        {
                            command.ExecuteNonQuery();
                            newUser = await GetUserByEmail(user.Email);
                        }
                        catch
                        {
                            // Log error
                            throw;
                        }
                    }
                };

                return newUser;
            });

            return await task;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            Task<User> task = Task.Run(() =>
            {
                User user = null;

                using (var connection = new MySqlConnection(ConnectionString))
                {

                }
                    
                return user;
            });

            return await task;
        }

        public async Task<User> UpdateUser(User user)
        {
            User updatedUser;

            Task<User> task = Task.Run(() =>
            {
                var connection = new MySqlConnection(ConnectionString);
                connection.Open();

                return updatedUser;
            });

            return await task;
        }

        public async Task<int> DeleteUser(int userId)
        {
            int newUserId = 0;

            Task<int> task = Task.Run(() =>
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    var sql = "DELETE FROM User WHERE UserId = @userId";

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.TableDirect;
                        command.Parameters.AddWithValue("userId", userId);

                        command.ExecuteNonQuery();
                    }
                }

                return newUserId;
            });

            return await task;
        }
    }
}
