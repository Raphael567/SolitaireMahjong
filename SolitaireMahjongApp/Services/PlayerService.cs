using SolitaireMahjongApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolitaireMahjongApp.Services
{
    public class PlayerService
    {
        private readonly HttpClient _httpClient;

        public PlayerService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080");
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("players/ranking");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Player>>(json);
            }

            return new List<Player>();
        }

        public async Task CreatePlayerAsync(Player player)
        {
            string json = JsonSerializer.Serialize(player);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("players", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> UpdatePlayerAsync(Player player)
        {
            try
            {
                string json = JsonSerializer.Serialize(player);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"players/{player.id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Falha ao atualizar jogador: {response.StatusCode} - {response.ReasonPhrase}");
                    return false;
                }

                return true;
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"Erro de requisição ao atualizar jogador: {httpEx.Message}");
                throw;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Erro ao atualizar jogador: {e.Message}");
                throw;
            }
        }
    }
}
