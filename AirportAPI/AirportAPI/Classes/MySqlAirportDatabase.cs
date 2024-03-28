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
            throw new System.NotImplementedException();
        }

        public async Task<User> AddUser(User user)
        {
            User newUser;

            Task<User> task = Task.Run(() =>
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    var sqlBuilder = new StringBuilder();
                    sqlBuilder.Append("INSERT INTO User (name, surname, birthday, nationality, email) ");
                    sqlBuilder.Append("VALUES(@name, @surname, @birthday, @nationality)");

                    var sql = sqlBuilder.ToString();

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.TableDirect;

                        command.Parameters.AddWithValue("@name",        user.Name);
                        command.Parameters.AddWithValue("@surname",     user.Surname); 
                        command.Parameters.AddWithValue("@birthday",    user.Birthday);
                        command.Parameters.AddWithValue("@nationality", user.Nacionality);
                        command.Parameters.AddWithValue("@email",       user.Email);

                        try
                        {
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    newUser = new User();

                                    newUser.Id = reader.GetInt32(0);
                                    newUser.Name = reader.GetString(1);
                                    newUser.Surname = reader.GetString(2);
                                    newUser.Birthday = reader.GetDateTime(3);
                                    newUser.Nacionality = reader.GetString(4);
                                    newUser.Email = reader.GetString(5);
                                }
                                else
                                {
                                    throw new Exception("Failed to retrieve a user just added to the DB");
                                }
                            }
                        }
                        catch(Exception exc)
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
