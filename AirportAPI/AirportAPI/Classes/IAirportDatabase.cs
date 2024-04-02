using AirportAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAirportDatabase
{
    // User
    Task<User> GetUserAsync(int userId);
    Task<List<User>> GetAllUsersAsync();
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<int> DeleteUserAsync(int userId);

    Task<Flight> GetFlightAsync(int flightId);
    Task<List<Flight>> GetAllFlightsAsync();
    Task<Flight> AddFlightAsync(Flight flight);
    Task<Flight> UpdateFlightAsync(Flight flight);
    Task<int> DeleteFlightAsync(int flightId);
}