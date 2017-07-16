using System;
using System.Threading.Tasks;

namespace RLSApi.Example
{
    internal class Program
    {
        private static void Main(string[] args) => Run().GetAwaiter().GetResult();

        private static async Task Run()
        {
            // Grabs RLS api key from environment variables.
            var apiKey = Environment.GetEnvironmentVariable("RLS_API_KEY");
            
            // Initialize RLSClient.
            var client = new RLSClient(apiKey);

            var platforms = await client.GetPlatformsAsync();

            Console.WriteLine("Finished.");
        }
    }

}