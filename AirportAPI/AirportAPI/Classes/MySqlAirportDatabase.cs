using AirportAPI.Models;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

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

        public async Task<User> AddUser(User user)
        {
            Task<User> task = Task.Run(() =>
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(StoredProcedureNames.AddUser, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@name", user.UserName);
                        command.Parameters.AddWithValue("@origin", user.Origin);
                        command.Parameters.AddWithValue("@destination", user.Destination);

                        command.ExecuteNonQuery();
                    }
                };

                return user;
            });
            var newUser = await task;

            return newUser;
        }

        public async Task<int> DeleteUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> UpdateUser(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
