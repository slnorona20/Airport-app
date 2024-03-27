using AirportAPI.Models;
using System;
using System.Threading.Tasks;

public interface IAirportDatabase
{
    // User
    Task<User> GetUser(int userId);
    Task<User> AddUser(User user);
    Task<User> UpdateUser(User user);
    Task<int> DeleteUser(int userId);
}