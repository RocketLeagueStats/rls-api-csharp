using System;
using System.Linq;
using System.Threading.Tasks;
using RLSApi.Data;
using RLSApi.Net.Requests;

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

            // Retrieve a single player.
            var player = await client.GetPlayerAsync(RlsPlatform.Steam, "76561198033338223");
            var playerSeasonSix = player.RankedSeasons.FirstOrDefault(x => x.Key == RlsSeason.Six);
            if (playerSeasonSix.Value != null)
            {
                Console.WriteLine($"# Player: {player.DisplayName}");

                foreach (var playerRank in playerSeasonSix.Value)
                {
                    Console.WriteLine($"{playerRank.Key}: {playerRank.Value.RankPoints} rating");
                }
            }

            // Retrieve multiple players.
            var players = await client.GetPlayersAsync(new[]
            {
                new PlayerBatchRequest(RlsPlatform.Steam, "76561198033338223"),
                new PlayerBatchRequest(RlsPlatform.Ps4, "Wizwonk"),
                new PlayerBatchRequest(RlsPlatform.Xbox, "Loubleezy")
            });
            
            Console.WriteLine("Finished multiple players.");

            // Search for players.
            var players2 = await client.SearchPlayerAsync("AeonLucid");
            
            Console.WriteLine("Finished search for players.");

            // Retrieve the top 100 players of a ranked playlist.
            var topPlayers = await client.GetLeaderboardRankedAsync(RlsPlaylistRanked.Standard);
            
            Console.WriteLine("Finished top 100 players of a ranked playlist.");

            // Retrieve the top 100 players of a stat type.
            var topPlayers2 = await client.GetLeaderboardStatAsync(RlsStatType.Wins);
            
            Console.WriteLine("Finished top 100 players of a stat type.");

            // Retrieve platform data.
            var platforms = await client.GetPlatformsAsync();
            
            Console.WriteLine("Finished platform data.");

            // Retrieve seasons data.
            var seasons = await client.GetSeasonsAsync();
            
            Console.WriteLine("Finished seasons data.");

            // Retrieve playlist (& population) data.
            var playlists = await client.GetPlaylistsAsync();
            
            Console.WriteLine("Finished playlists data.");

            // Retrieve tiers data.
            var tiers = await client.GetTiersAsync();
            
            Console.WriteLine("Finished tiers data.");

            // Retrieve tier data of a specific season.
            var tiers2 = await client.GetTiersAsync(RlsSeason.One);
            
            Console.WriteLine("Finished tier data of a specific season.");
            Console.WriteLine("Finished.");
        }
    }

}