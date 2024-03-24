using AirportAPI.Models;
using System;

public interface IAirportDatabase
{
    // User
    User GetUser(int userId);
    User AddUser(User user);
    User UpdateUser(User user);
    int DeleteUser(int userId);
}