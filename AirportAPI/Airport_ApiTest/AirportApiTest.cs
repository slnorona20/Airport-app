using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Airport_ApiTest
{
    public class AirportApiTest
    {
        protected IConfigurationBuilder TestConfigurationBuilder;

        protected readonly TestServer TestServer;
        protected readonly HttpClient HttpClient;

        protected string ApiUrl = "";


        public AirportApiTest()
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
    }
}
