using AirportAPI.Models;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace AirportAPI.Classes
{
    public class MySqlAirportDatabase : IAirportDatabase
    {
        protected string ConnectionString { get; }
        MySqlConnection connection;

        public MySqlAirportDatabase()
        {
            ConnectionString = Startup.DBConnectionString;
            connection = new MySqlConnection(ConnectionString);
            connection.Open();
        }

        public async Task<User> AddUser(User user)
        {
            Task<User> task = Task.Run(() =>
            {

                return new User();
            });
            var newUser = await task;

            return newUser;
        }

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
