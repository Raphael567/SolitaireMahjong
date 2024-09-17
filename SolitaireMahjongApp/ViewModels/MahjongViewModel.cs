using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using SolitaireMahjongApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace SolitaireMahjongApp.ViewModels
{
    public partial class MahjongViewModel : ObservableObject
    {
        [ObservableProperty]
        private string timerText = "Time 2:00";

        [ObservableProperty]
        private string scoreText = "Score: 0";

        [ObservableProperty]
        private ObservableCollection<Tile> tiles = new ObservableCollection<Tile>();

        private Tile _firstTileSelected = null;
        private Tile _secondTileSelected = null;
        private int _score = 0;
        private int _timeLeft = 120; // 2 minutos em segundos

        public ICommand TileCommand { get; }

        public MahjongViewModel()
        {
            TileCommand = new RelayCommand<Tile>(OnTileClicked);
            GenerateTiles();
            StartTimer();
        }

        private void GenerateTiles()
        {
            // Gerar pares de peças
            for (int i = 1; i <= 2; i++)  // Exemplo com 8 pares
            {
                Tiles.Add(new Tile { Name = $"Tile{i}", Color = Colors.LightGray });
                Tiles.Add(new Tile { Name = $"Tile{i}", Color = Colors.LightGray });
            }

            //Embaralhar as peças
            var shuffledTiles = Tiles.OrderBy(t => Guid.NewGuid()).ToList();

            Tiles = new ObservableCollection<Tile>(shuffledTiles);

            // Adiciona as peças embaralhadas ao Tiles
            Tiles.Clear();
            foreach(var tile in shuffledTiles)
            {
                Tiles.Add(tile);
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
                if (_firstTileSelected.Name == _secondTileSelected.Name)
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
                    _firstTileSelected.Color = Colors.LightGray;

                    _firstTileSelected = _secondTileSelected;
                    _secondTileSelected = null;

                }
            }
        }

        private void GameOver()
        {
            Application.Current.MainPage.DisplayAlert("Game Over", "O tempo acabou", "OK");
            Tiles.Clear();
        }

        private void GameWin()
        {
            Application.Current.MainPage.DisplayAlert("Você Ganhou", "Todas as peças foram removidas", "OK");
        }
    }
}
