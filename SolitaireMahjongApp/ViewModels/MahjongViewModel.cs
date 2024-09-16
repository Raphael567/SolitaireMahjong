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
            for (int i = 1; i <= 8; i++)  // Exemplo com 8 pares
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
            while (_timeLeft > 0)
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
            if (_firstTileSelected == null)
            {
                _firstTileSelected = tile;
                _firstTileSelected.Color = Colors.Blue;
            }
            else if (_secondTileSelected == null)
            {
                _secondTileSelected = tile;
                _secondTileSelected.Color = Colors.Blue;

                CheckForMatch();
            }
        }

        private void CheckForMatch()
        {
            if (_firstTileSelected != null && _secondTileSelected != null)
            {
                if (_firstTileSelected.Name == _secondTileSelected.Name)
                {
                    // Remover as peças correspondentes
                    Tiles.Remove(_firstTileSelected);
                    Tiles.Remove(_secondTileSelected);

                    // Atualizar a pontuação
                    _score += 1;
                    ScoreText = $"Score: {_score}";
                }
                else
                {
                    // Resetar a cor se não houver correspondência
                    _firstTileSelected.Color = Colors.LightGray;
                    _secondTileSelected.Color = Colors.LightGray;
                }

                _firstTileSelected = null;
                _secondTileSelected = null;
            }
        }

        private void GameOver()
        {
            // Lógica de fim de jogo (exibir mensagem ou reiniciar)
            Console.WriteLine("Game Over!");
        }
    }
}
