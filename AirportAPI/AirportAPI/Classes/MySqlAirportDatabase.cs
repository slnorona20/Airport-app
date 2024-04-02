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

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "spGetUser(@userId)";

                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
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

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "spGetAllUsers()";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            users.Add( new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Surname = reader.GetString(2),
                                BirthDate = reader.GetDateTime(3),
                                Nationality = reader.GetString(4),
                                Email = reader.GetString(5)
                            });
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
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
;
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "CALL spAddUser(@username, @usersurname, @birthdate, @nationality, @email)";

                    command.Parameters.AddWithValue("@username", user.Name);
                    command.Parameters.AddWithValue("@usersurname", user.Surname);
                    command.Parameters.AddWithValue("@birthdate", user.BirthDate);
                    command.Parameters.AddWithValue("@nationality", user.Nationality);
                    command.Parameters.AddWithValue("@email", user.Email);

                    //await command.ExecuteNonQueryAsync();
                    //newUser = await GetUserByEmailAsync(user.Email);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
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
            };

            return newUser;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            User updatedUser;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "CALL spUpdateUser(@userId, @username, @usersurname, @birthdate, @nationality, @email)";

                    command.Parameters.AddWithValue("@userId", user.Id);
                    command.Parameters.AddWithValue("@username", user.Name);
                    command.Parameters.AddWithValue("@usersurname", user.Surname);
                    command.Parameters.AddWithValue("@birthdate", user.BirthDate);
                    command.Parameters.AddWithValue("@nationality", user.Nationality);
                    command.Parameters.AddWithValue("@email", user.Email);

                    await command.ExecuteNonQueryAsync();
                    updatedUser = await GetUserAsync(user.Id);
                }
            }
                    
            return updatedUser;
        }

        public async Task<int> DeleteUserAsync(int userId)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.TableDirect;
                    command.CommandText = "spDeleteUsr(@userId)";

                    command.Parameters.AddWithValue("@userId", userId);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return userId;
        }

        /// <summary>
        /// Future use
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ObjectNotFoundException"></exception>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "CALL spGetUserByEmail(@email)";

                    command.Parameters.AddWithValue("@email", email);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
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
        #endregion User

        #region Flight
        public async Task<Flight> GetFlightAsync(int flightId)
        {
            Flight flight;

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "spGetFlight( @flightId )";

                    command.Parameters.AddWithValue("@flightId", flightId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            flight = new Flight
                            {
                                Id = reader.GetInt32(0),
                                FlightId = reader.GetString(1),
                                DepartDate = reader.GetDateTime(2),
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

        public async Task<List<Flight>> GetAllFlightsAsync()
        {
            List<Flight> flights = new List<Flight>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "spGetAllFlights()";

                    using (var reader = await command.ExecuteReaderAsync()) 
                    {
                        if (reader.HasRows)
                        {
                            flights.Add(new Flight
                            {
                                Id = reader.GetInt32(0),
                                FlightId = reader.GetString(1),
                                DepartDate = reader.GetDateTime(2),
                                ArrivalDate = reader.GetDateTime(3),
                                OriginCountry = reader.GetString(4),
                                DestinationCountry = reader.GetString(5),
                                AircraftModel = reader.GetString(6),
                                MaxSeats = reader.GetInt32(7),
                                AvailableSeats = reader.GetInt32(0)
                            });
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

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "spAddFlight(@flightid, @departdate, @arrivadate, @origincountry, @destinationcountry, @aircraftmodel, @maxseats, @availableseats)";

                    command.Parameters.AddWithValue("@departdate", flight.DepartDate);
                    command.Parameters.AddWithValue("@arrivaldate", flight.ArrivalDate);
                    command.Parameters.AddWithValue("@origincountry", flight.OriginCountry);
                    command.Parameters.AddWithValue("@destinationcountry", flight.DestinationCountry);
                    command.Parameters.AddWithValue("@aircraftmodel", flight.AircraftModel);
                    command.Parameters.AddWithValue("@maxseats", flight.MaxSeats);
                    command.Parameters.AddWithValue("@availableseats", flight.AvailableSeats);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            newFlight = new Flight
                            {
                                Id = reader.GetInt32(0),
                                FlightId = reader.GetString(1),
                                DepartDate = reader.GetDateTime(2),
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

            return newFlight;
        }

        public async Task<Flight> UpdateFlightAsync(Flight flight)
        {
            Flight updatedFlight;

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand()) 
                { 
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "spUpdateFlight(@flightid, @departdate, @arrivadate, @origincountry, @destinationcountry, @aircraftmodel, @maxseats, @availableseats)";

                    command.Parameters.AddWithValue("", flight.FlightId);
                    command.Parameters.AddWithValue("", flight.DepartDate);
                    command.Parameters.AddWithValue("", flight.ArrivalDate);
                    command.Parameters.AddWithValue("", flight.OriginCountry);
                    command.Parameters.AddWithValue("", flight.DestinationCountry);
                    command.Parameters.AddWithValue("", flight.AircraftModel);
                    command.Parameters.AddWithValue("", flight.MaxSeats);
                    command.Parameters.AddWithValue("", flight.AvailableSeats);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            updatedFlight = new Flight
                            {
                                Id = reader.GetInt32(0),
                                FlightId = reader.GetString(1),
                                DepartDate = reader.GetDateTime(2),
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
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "spDeleteFlight(@flightId)";

                    await command.ExecuteNonQueryAsync();
                }
            }
            return flightId;
        }
        #endregion Flight

    }
}
