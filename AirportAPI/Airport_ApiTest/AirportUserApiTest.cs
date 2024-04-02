using AirportAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Airport_ApiTest
{
    public class AirportUserApiTest
    {
        protected IConfigurationBuilder TestConfigurationBuilder;

        protected readonly TestServer TestServer;
        protected readonly HttpClient HttpClient;

        protected string ApiUrl = "/api/user";

        public AirportUserApiTest()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            TestConfigurationBuilder = new ConfigurationBuilder().SetBasePath(currentDirectory).AddJsonFile("appsettings.json");

            TestServer = new TestServer(new WebHostBuilder().UseContentRoot(currentDirectory)
                                                            .UseConfiguration(TestConfigurationBuilder.Build())
                                                            .UseStartup<Startup>());

            HttpClient = TestServer.CreateClient();
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Fact] 
        public async void GetUser_Test()
        {
            var url = $"{ApiUrl}/1";
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            var responseCode = response.StatusCode;
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void AddUser_Test()
        {
            var url = ApiUrl;
           
            User user = new User 
            {
                Id = 0,
                Name = "John",
                Surname = "Doe",
                BirthDate = DateTime.Now.AddYears(-20),
                Nationality = "Escoses",
                Email = "john-doe@email.com"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PostAsync(url, stringContent);
            var responseCode = response.StatusCode;
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void UpdateUser_Test()
        {
            var url = $"{ApiUrl}/5";

            User user = new User
            {
                Id = 1,
                Surname = "Doe",
                BirthDate = DateTime.Now.AddYears(-20),
                Nationality = "Escoses",
                Email = "john-doe@email.com"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PutAsync(url, stringContent);
            var responseCode = response.StatusCode;
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void DeleteUser_Test()
        {
            var url = $"{ApiUrl}/1";
            HttpResponseMessage response = await HttpClient.DeleteAsync(url);
            var responseCode = response.StatusCode;
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }
    }
}