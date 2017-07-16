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
            var playerSeasonFive = player.RankedSeasons.FirstOrDefault(x => x.Key == RlsSeason.Five);
            if (playerSeasonFive.Value != null)
            {
                Console.WriteLine($"# Player: {player.DisplayName}");

                foreach (var playerRank in playerSeasonFive.Value)
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

            // Search for players.
            var players2 = await client.SearchPlayerAsync("AeonLucid");

            // Retrieve the top 100 players of a ranked playlist.
            var topPlayers = await client.GetLeaderboardRankedAsync(RlsPlaylistRanked.Standard);

            // Retrieve the top 100 players of a stat type.
            var topPlayers2 = await client.GetLeaderboardStatAsync(RlsStatType.Wins);

            // Retrieve platform data.
            var platforms = await client.GetPlatformsAsync();

            // Retrieve season data.
            var seasons = await client.GetSeasonsAsync();

            // Retrieve playlist (& population) data.
            var playlists = await client.GetPlaylistsAsync();

            // Retrieve tiers data.
            var tiers = await client.GetTiersAsync();

            // Retrieve tiers data of the specified season.
            var tiers2 = await client.GetTiersAsync(RlsSeason.One);

            Console.WriteLine("Finished.");
        }
    }

}