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
        private string timerText;

        [ObservableProperty]
        private string scoreText;

        [ObservableProperty]
        private ObservableCollection<Tile> _tiles;

        private Tile _firstTileSelected = null;
        private Tile _secondTileSelected = null;
        private int _score = 0;
        private int _timeLeft = 20000; // 2 minutos em segundos

        public ICommand TileCommand { get; }

        public MahjongViewModel()
        {
            _tileService = new TileService();
            TileCommand = new RelayCommand<Tile>(OnTileClicked);
            LoadTilesCommand = new AsyncRelayCommand(LoadTilesAsync);

            Task.Run(async () =>
            {
                await LoadTilesAsync();
                StartTimer();
            });
        }

        public IAsyncRelayCommand LoadTilesCommand { get; }

        private async Task LoadTilesAsync()
        {
            var tilesFromApi = await _tileService.GetTilesAsync();
            var shuffledTiles = tilesFromApi.OrderBy(t => Guid.NewGuid()).ToList();

            Tiles = new ObservableCollection<Tile>();

            int[,] layer1 = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            int[,] layer2 = new int[,]
            {
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 }
            };

            int[,] layer3 = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            int[,] layer4 = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            int[,] layer5 = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            int[,] layer6 = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            // Define a organização das camadas
            var layers = new List<int[,]>
            {
                layer1, // Camada 1
                layer2, // Camada 2
                layer3, // Camada 3
                layer4, // Camada 4
                layer5, // Camada 5
                layer6  // Camada 6
            };

            int tileWidth = 60;
            int tileHeight = 75;
            int zSpacing = 5; // Espaçamento entre as camadas

            //Ajusta ao centro da tela
            double centerX = 300;
            double centerY = 400;

            int tileIndex = 0;
            for (int z = 0; z < layers.Count; z++)
            {
                var layer = layers[z];
                for (int y = 0; y < layer.GetLength(0); y++)
                {
                    for (int x = 0; x < layer.GetLength(1); x++)
                    {
                        if (layer[y, x] == 1 && tileIndex < shuffledTiles.Count)
                        {
                            var tile = shuffledTiles[tileIndex];
                            tileIndex++;

                            double posX = centerX - (layer.GetLength(1) * tileWidth / 2) + x * tileWidth;
                            double posY = centerY - (layer.GetLength(0) * tileHeight / 2) + y * tileHeight - z * zSpacing;

                            tile.LayoutBounds = new Rect(posX, posY, tileWidth, tileHeight);

                            Tiles.Add(tile);
                        }
                    }
                }
            }
        }

        private async void StartTimer()
        {
            while (_timeLeft > 0 && Tiles.Count != 0)
            {
                await Task.Delay(1000);
                _timeLeft--;
                TimerText = $"Time {TimeSpan.FromSeconds(_timeLeft):mm\\:ss}";

                if (_timeLeft == 0)
                {
                    GameOver();
                }
            }
        }

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