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
            return await _httpClient.GetFromJsonAsync<List<Player>>("http://localhost:8080/players");
        }

        public async Task<Player> CreatePlayerAsync(Player player)
        {
            var response = await _httpClient.PostAsJsonAsync<Player>("http://localhost:8080/players", player);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Player>();
        }
    }
}
