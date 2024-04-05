using AirportAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Airport_ApiTest
{
    public class AirportFlightApiTest: AirportApiTest
    {
        public AirportFlightApiTest()
        {
            ApiUrl = "/api/flight";
        }

        [Fact]
        public async void GetFlight_Test()
        {
            var url = $"{ApiUrl}/1";
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            var responseCode = response.StatusCode;
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void GetFlights_Test()
        {
            var departureDate = DateTime.Today.ToString("yyyy-MM-dd");
            var searchOriginCountry = "Cuba";
            var searchDestinationCountry = "Espana";

            var url = $"{ApiUrl}/{departureDate}/{searchOriginCountry}/{searchDestinationCountry}";

            HttpResponseMessage response = await HttpClient.GetAsync(url);
            var responseCode = response.StatusCode;
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void AddFlight_Test()
        {
            var url = ApiUrl;

            Flight flight = new Flight
            {
                Id = 0,
                FlightId = "HK-5674", 
                DepartureDate = DateTime.Now.AddDays(5),
                ArrivalDate = DateTime.Now.AddDays(6),
                OriginCountry = "Cuba",
                DestinationCountry = "Espana",
                AircraftModel = "FM-354",
                MaxSeats = 235,
                AvailableSeats = 98
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(flight), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PostAsync(url, stringContent);
            var responseCode = response.StatusCode;
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void UpdateFlight_Test()
        {
            var url = ApiUrl;

            Flight flight = new Flight
            {
                Id = 5,
                FlightId = "HK-5687",
                DepartureDate = DateTime.Now,
                ArrivalDate = DateTime.Now.AddDays(1),
                OriginCountry = "Havan, Cuba",
                DestinationCountry = "Roma, Italia",
                AircraftModel = "FM-354",
                MaxSeats = 235,
                AvailableSeats = 70
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(flight), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PutAsync(url, stringContent);
            var responseCode = response.StatusCode;
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void DeleteFight_Test()
        {
            var url = $"{ApiUrl}/1";
            HttpResponseMessage response = await HttpClient.DeleteAsync(url);
            var responseCode = response.StatusCode;
             var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }
    }
}
