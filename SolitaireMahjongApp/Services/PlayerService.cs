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

        public async Task<Player> CreatePlayerAsync(Player player)
        {
            try
            {   
                //PlayerDTO sem Id
                var playerDTO = new PlayerDTO
                {
                    nome = player.nome,
                    pontuacao = player.pontuacao
                };

                string json = JsonSerializer.Serialize(playerDTO);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("players", content);
                response.EnsureSuccessStatusCode();

                var createdPlayerJson = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"Resposta da API: {createdPlayerJson}");
                var createdPlayer = JsonSerializer.Deserialize<Player>(createdPlayerJson);

                return createdPlayer;
            }

            catch(HttpRequestException ex)
            {

                Debug.WriteLine($"Erro ao criar jogador {ex.Message}");
                throw;
            }
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

        public async Task DeletePlayerAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"players/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Falha ao deletar jogador: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"Erro de requisição ao deletar jogador: {httpEx.Message}");
                throw;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Erro ao deletar jogador: {e.Message}");
                throw;
            }
        }
    }
}
