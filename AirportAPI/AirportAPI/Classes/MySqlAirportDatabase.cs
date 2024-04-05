using AirportAPI.Models;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Text;
using AirportAPI.Exceptions;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reflection.PortableExecutable;

namespace AirportAPI.Classes
{
    public class MySqlAirportDatabase : IAirportDatabase
    {
        protected string ConnectionString { get; }

        public MySqlAirportDatabase()
        {
            ConnectionString = Startup.DBConnectionString;
        }

        #region User
        public async Task<User> GetUserAsync(int userId)
        {
            User user;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand("spGetUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            user = new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Surname = reader.GetString(2),
                                BirthDate = reader.GetDateTime(3),
                                Nationality = reader.GetString(4),
                                Email = reader.GetString(5)
                            };
                        }
                        else
                        {
                            throw new ObjectNotFoundException("Failed to retrieve a user just added to the DB");
                        }
                    }
                }
            }
            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            List<User> users = new List<User>();
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spGetAllUsers", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                users.Add(new User
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Surname = reader.GetString(2),
                                    BirthDate = reader.GetDateTime(3),
                                    Nationality = reader.GetString(4),
                                    Email = reader.GetString(5)
                                });
                            }
                        }
                        else
                        {
                            throw new ObjectNotFoundException("Failed to retrieve a list of user from the DB");
                        }
                    }
                }
            }
            return users;
        }

        public async Task<User> AddUserAsync(User user)
        {
            User newUser;

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAddUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.AddWithValue("@username", "John");
                    command.Parameters.AddWithValue("@usersurname", "Doe");
                    command.Parameters.AddWithValue("@birthdate", new DateTime(1990, 1, 1));
                    command.Parameters.AddWithValue("@nationality", "US");
                    command.Parameters.AddWithValue("@email", "john.doe@example.com");

                    // Execute the stored procedure
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            newUser = new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Surname = reader.GetString(2),
                                BirthDate = reader.GetDateTime(3),
                                Nationality = reader.GetString(4),
                                Email = reader.GetString(5)
                            };
                        }
                        else
                        {
                            throw new ObjectNotFoundException("Failed to retrieve a list of user from the DB");
                        }
                    }
                }
            }


            return newUser;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            User updatedUser;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand("spUpdateUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@userId", user.Id);
                    command.Parameters.AddWithValue("@username", user.Name);
                    command.Parameters.AddWithValue("@usersurname", user.Surname);
                    command.Parameters.AddWithValue("@birthdate", user.BirthDate);
                    command.Parameters.AddWithValue("@nationality", user.Nationality);
                    command.Parameters.AddWithValue("@email", user.Email);

                    // Execute the stored procedure
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            updatedUser = new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Surname = reader.GetString(2),
                                BirthDate = reader.GetDateTime(3),
                                Nationality = reader.GetString(4),
                                Email = reader.GetString(5)
                            };
                        }
                        else
                        {
                            throw new ObjectNotFoundException("Failed to retrieve a list of user from the DB");
                        }
                    }
                }
            }
                    
            return updatedUser;
        }

        public async Task<int> DeleteUserAsync(int userId)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand("spDeleteUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@userId", userId);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return userId;
        }
        #endregion User

        #region Flight
        public async Task<Flight> GetFlightAsync(int dbid)
        {
            Flight flight;

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("spGetFlight", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@dbid", dbid);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            flight = new Flight
                            {
                                Id = reader.GetInt32(0),
                                FlightId = reader.GetString(1),
                                DepartureDate = reader.GetDateTime(2),
                                ArrivalDate = reader.GetDateTime(3),
                                OriginCountry = reader.GetString(4),
                                DestinationCountry = reader.GetString(5),
                                AircraftModel = reader.GetString(6),
                                MaxSeats = reader.GetInt32(7),
                                AvailableSeats = reader.GetInt32(1)
                            };
                        }
                        else
                        {
                            throw new ObjectNotFoundException("Failed to retrieve a list of user from the DB");
                        }
                    }
                }
            }

            return flight;
        }

        public async Task<List<Flight>>  GetFlightsAsync(DateTime departureDate, string searchOriginCountry, string searchDestinationCountry)
        {
            List<Flight> flights = new List<Flight>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand("spGetFlights", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@departuredate", departureDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@origincountry", searchOriginCountry);
                    command.Parameters.AddWithValue("@destinationCountry", searchDestinationCountry);

                    using (var reader = await command.ExecuteReaderAsync()) 
                    {
                        if (reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                flights.Add(new Flight
                                {
                                    Id = reader.GetInt32(0),
                                    FlightId = reader.GetString(1),
                                    DepartureDate = reader.GetDateTime(2),
                                    ArrivalDate = reader.GetDateTime(3),
                                    OriginCountry = reader.GetString(4),
                                    DestinationCountry = reader.GetString(5),
                                    AircraftModel = reader.GetString(6),
                                    MaxSeats = reader.GetInt32(7),
                                    AvailableSeats = reader.GetInt32(0)
                                });
                            }
                        }
                        else
                        {
                            throw new ObjectNotFoundException("Failed to retrieve a flight just added to the DB");
                        }
                    }
                }
            }
                
            return flights;
        }

        public async Task<Flight> AddFlightAsync(Flight flight)
        {
            Flight newFlight;

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand("spAddFlight", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@flightid", flight.FlightId);
                    command.Parameters.AddWithValue("@departuredate", flight.DepartureDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@arrivaldate", flight.ArrivalDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@origincountry", flight.OriginCountry);
                    command.Parameters.AddWithValue("@destinationcountry", flight.DestinationCountry);
                    command.Parameters.AddWithValue("@aircraftmodel", flight.AircraftModel);
                    command.Parameters.AddWithValue("@maxseats", flight.MaxSeats);
                    command.Parameters.AddWithValue("@availableseats", flight.AvailableSeats);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            newFlight = new Flight
                            {
                                Id = reader.GetInt32(0),
                                FlightId = reader.GetString(1),
                                DepartureDate = reader.GetDateTime(2),
                                ArrivalDate = reader.GetDateTime(3),
                                OriginCountry = reader.GetString(4),
                                DestinationCountry = reader.GetString(5),
                                AircraftModel = reader.GetString(6),
                                MaxSeats = reader.GetInt32(7),
                                AvailableSeats = reader.GetInt32(8)
                            };
                        }
                        else
                        {
                            throw new ObjectNotFoundException("Failed to retrieve a list of user from the DB");
                        }
                    }

                }
            }

            return newFlight;
        }

        public async Task<Flight> UpdateFlightAsync(Flight flight)
        {
            Flight updatedFlight;

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("spUpdateFlight", connection)) 
                { 
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("dbid", flight.Id);
                    command.Parameters.AddWithValue("flightid", flight.FlightId);
                    command.Parameters.AddWithValue("departureDate", flight.DepartureDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("arrivalDate", flight.ArrivalDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("originCountry", flight.OriginCountry);
                    command.Parameters.AddWithValue("destinationCountry", flight.DestinationCountry);
                    command.Parameters.AddWithValue("aircraftModel", flight.AircraftModel);
                    command.Parameters.AddWithValue("maxSeats", flight.MaxSeats);
                    command.Parameters.AddWithValue("availableSeats", flight.AvailableSeats);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            updatedFlight = new Flight
                            {
                                Id = reader.GetInt32(0),
                                FlightId = reader.GetString(1),
                                DepartureDate = reader.GetDateTime(2),
                                ArrivalDate = reader.GetDateTime(3),
                                OriginCountry = reader.GetString(4),
                                DestinationCountry = reader.GetString(5),
                                AircraftModel = reader.GetString(6),
                                MaxSeats = reader.GetInt32(7),
                                AvailableSeats = reader.GetInt32(8)
                            };
                        }
                        else
                        {
                            throw new ObjectNotFoundException("Failed to retrieve a list of user from the DB");
                        }
                    }
                }
            }

            return updatedFlight;
        }

        public async Task<int> DeleteFlightAsync(int flightId)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("spDeleteFlight", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("dbid", flightId);

                    await command.ExecuteNonQueryAsync();
                }
            }
            return flightId;
        }
        #endregion Flight
    }
}
