using System;

namespace AirportAPI.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightId { get; set; }
        public DateTime DepartDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string OriginCountry { get; set; }
        public string DestinationCountry { get; set; }
        public string AircraftModel { get; set; }
        public int MaxSeats { get; set; }
        public int AvailableSeats { get; set; }
    }
}