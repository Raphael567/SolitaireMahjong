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
            var tempTiles = new ObservableCollection<Tile>();

            // Gerar pares de peças
            for (int i = 1; i <= 8; i++)  // Exemplo com 8 pares
            {
                tempTiles.Add(new Tile { Name = $"Tile{i}", Color = Colors.LightGray });
                tempTiles.Add(new Tile { Name = $"Tile{i}", Color = Colors.LightGray });
            }

            // Embaralhar as peças
            var shuffledTiles = tempTiles.OrderBy(t => Guid.NewGuid()).ToList();

            // Configurar o layout das peças em um grid piramidal
            int index = 0;
            for (int row = 0; row < 7; row++) // 7 camadas
            {
                for (int col = 0; col <= row; col++)
                {
                    if (index < shuffledTiles.Count)
                    {
                        var tile = shuffledTiles[index++];
                        tile.Row = row;
                        tile.Column = col;
                        tile.IsExposed = row == 6;
                        Tiles.Add(tile);
                    }
                }
            }
        }

        private void UpdateTileExposure()
        {
            foreach (var tile in Tiles)
            {
                tile.IsExposed = true; // Inicializar todas as peças como não expostas
            }

            //// Verificar todas as camadas de cima para baixo
            //for (int row = 6; row >= 0; row--) // Camada mais baixa primeiro
            //{
            //    for (int col = 0; col <= row; col++)
            //    {
            //        var tile = Tiles.FirstOrDefault(t => t.Row == row && t.Column == col);
            //        if (tile != null)
            //        {
            //            if (row == 6) // A camada mais baixa, sempre exposta
            //            {
            //                tile.IsExposed = true;
            //            }
            //            else
            //            {
            //                // Verificar se há peças bloqueando acima (tanto à esquerda quanto à direita)
            //                var pieceAboveLeft = Tiles.FirstOrDefault(t => t.Row == row + 1 && t.Column == col);
            //                var pieceAboveRight = Tiles.FirstOrDefault(t => t.Row == row + 1 && t.Column == col + 1);

            //                // Peça exposta se nenhuma peça estiver bloqueando à esquerda e à direita
            //                tile.IsExposed = (pieceAboveLeft == null && pieceAboveRight == null);
            //            }
            //        }
            //    }
            //}
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
                if (_firstTileSelected.Name == _secondTileSelected.Name && 
                    _firstTileSelected.isExposed && _secondTileSelected.isExposed)
                {

                    var _firstTileToRemove = _firstTileSelected;
                    var _secondTileToRemove = _secondTileSelected;

                    // Remover as peças correspondentes
                    Tiles.Remove(_firstTileToRemove);
                    Tiles.Remove(_secondTileToRemove);

                    UpdateTileExposure();

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
