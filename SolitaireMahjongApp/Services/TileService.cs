using SolitaireMahjongApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolitaireMahjongApp.Services
{
    public class TileService
    {
        public readonly HttpClient _httpClient;

        public TileService() 
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080");
        }

        public async Task<List<Tile>> GetTilesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("pecas");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Tile>>(json);
            }

            return new List<Tile>();
        }
    }
}
