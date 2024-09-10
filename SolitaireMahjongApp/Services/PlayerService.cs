using SolitaireMahjongApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireMahjongApp.Services
{
    public class PlayerService
    {
        private readonly HttpClient _httpClient;

        public PlayerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Player>>("https://localhost:8080/player");
        }

        public async Task<Player> CreatePlayerAsync(Player player)
        {
            var response = await _httpClient.PostAsJsonAsync<Player>("https://localhost:8080/player", player);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Player>();
        }
    }
}
