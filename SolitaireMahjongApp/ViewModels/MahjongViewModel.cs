using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using SolitaireMahjongApp.Models;
using SolitaireMahjongApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace SolitaireMahjongApp.ViewModels
{
    public partial class MahjongViewModel : ObservableObject
    {
        private readonly TileService _tileService;

        [ObservableProperty]
        private string timerText = "Time 2:00";

        [ObservableProperty]
        private string scoreText = "Score: 0";

        [ObservableProperty]
        private ObservableCollection<Tile> _tiles;

        //[ObservableProperty]
        //private ObservableCollection<Tile> tiles = new ObservableCollection<Tile>();

        private Tile _firstTileSelected = null;
        private Tile _secondTileSelected = null;
        private int _score = 0;
        private int _timeLeft = 120; // 2 minutos em segundos

        public ICommand TileCommand { get; }

        public MahjongViewModel()
        {
            _tileService = new TileService();
            TileCommand = new RelayCommand<Tile>(OnTileClicked);
            LoadTilesCommand = new AsyncRelayCommand(LoadTilesAsync);
            //StartTimer();
        }

        public IAsyncRelayCommand LoadTilesCommand { get; }

        private async Task LoadTilesAsync()
        {
            var tilesFromApi = await _tileService.GetTilesAsync();
            var shuffledTiles = tilesFromApi.OrderBy(t => Guid.NewGuid()).ToList();

            Tiles = new ObservableCollection<Tile>();

            // Definindo o número de peças por camada
            int[] tilesPerLayer = { 40, 16, 4 }; // Número de peças por camada
            int layers = tilesPerLayer.Length; // Total de camadas
            double tileWidth = 100; // Largura da peça
            double tileHeight = 150; // Altura da peça
            double zSpacing = 10; // Espaçamento entre as camadas

            // Calcular o centro do layout
            double centerX = 300;
            double centerY = 400;

            for (int layer = 0; layer < layers; layer++)
            {
                for (int index = 0; index < tilesPerLayer[layer]; index++)
                {
                    int tileIndex = layer == 0
                        ? index // Para a camada 1, usa o índice diretamente
                        : index + (tilesPerLayer.Take(layer).Sum()); // Para camadas superiores, soma as peças das camadas anteriores

                    // Verifica se há peças suficientes
                    if (tileIndex >= shuffledTiles.Count)
                        break;

                    var tile = shuffledTiles[tileIndex];

                    // Calcula a posição X e Y com base na camada e no índice
                    double x = centerX - (layer == 0 ? 6 : (layer == 1 ? 4 : 2)) * tileWidth / 2 + (index % (layer == 0 ? 6 : (layer == 1 ? 4 : 2))) * tileWidth;
                    double y = centerY - (layer == 0 ? 6 : (layer == 1 ? 4 : 2)) * tileHeight / 2 + (index / (layer == 0 ? 6 : (layer == 1 ? 4 : 2))) * tileHeight - (layer * zSpacing);

                    // Define os limites de layout
                    tile.LayoutBounds = new Rect(x, y, tileWidth, tileHeight);

                    Tiles.Add(tile);
                }
            }
        }

        //private async void StartTimer()
        //{
        //    while (_timeLeft > 0 && Tiles.Count != 0)
        //    {
        //        await Task.Delay(1000);
        //        _timeLeft--;
        //        TimerText = $"Time {TimeSpan.FromSeconds(_timeLeft):mm\\:ss}";

        //        if (_timeLeft == 0)
        //        {
        //            GameOver();
        //        }
        //    }
        //}

        private void OnTileClicked(Tile tile)
        {

            if (_firstTileSelected == tile)
            {
                return;
            }

            if (_firstTileSelected == null)
            {
                _firstTileSelected = tile;
                tile.Color = Colors.Blue;
            }
            else if (_secondTileSelected == null)
            {
                _secondTileSelected = tile;
                tile.Color = Colors.Blue;

                CheckForMatch();
            }
        }

        private async void CheckForMatch()
        {
            if (_firstTileSelected != null && _secondTileSelected != null)
            {
                if (_firstTileSelected.simbolo == _secondTileSelected.simbolo
                    && _firstTileSelected.cor == _secondTileSelected.cor)
                {

                    var _firstTileToRemove = _firstTileSelected;
                    var _secondTileToRemove = _secondTileSelected;

                    // Remover as peças correspondentes
                    Tiles.Remove(_firstTileToRemove);
                    Tiles.Remove(_secondTileToRemove);

                    // Atualizar a pontuação
                    _score += 1;
                    ScoreText = $"Score: {_score}";

                    if (Tiles.Count == 0)
                    {
                        GameWin();
                    }

                    _firstTileSelected = null;
                    _secondTileSelected = null;
                }
                else
                {
                    // Resetar a cor se não houver correspondência
                    _firstTileSelected.Color = Colors.Transparent;

                    _firstTileSelected = _secondTileSelected;
                    _secondTileSelected = null;
                }
            }
        }

        private void GameOver()
        {
            Application.Current.MainPage.DisplayAlert("Game Over", "O tempo acabou", "OK");
            Tiles.Clear();
            _timeLeft = 120;
            _score = 0;
            ScoreText = $"Score: {_score}";
            LoadTilesAsync();
        }

        private void GameWin()
        {
            Application.Current.MainPage.DisplayAlert("Você Ganhou", "Todas as peças foram removidas", "OK");
        }
    }
}