using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SolitaireMahjongApp.Models;
using SolitaireMahjongApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

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
        private List<int[,]> layers;
        private Dictionary<(int layer, int row, int col), Tile> tileMap;

        public ICommand TileCommand { get; }

        public MahjongViewModel()
        {
            _tileService = new TileService();
            TileCommand = new RelayCommand<Tile>(OnTileClicked);
            LoadTilesCommand = new AsyncRelayCommand(LoadTilesAsync);

            layers = tilesLayers();
            tileMap = new Dictionary<(int layer, int row, int col), Tile>();

            Task.Run(async () =>
            {
                await LoadTilesAsync();
                StartTimer();
            });
        }

        public IAsyncRelayCommand LoadTilesCommand { get; }

        public List<int[,]> tilesLayers()
        {

            int[,] layer1 = new int[,]
            {
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 }
            };

            int[,] layer2 = new int[,]
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

            int[,] layer3 = new int[,]
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

            int[,] layer4 = new int[,]
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

            int[,] layer5 = new int[,]
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
            };

            return layers;
        }


        private async Task<List<Tile>> ShuffleTiles()
        {
            var tilesFromApi = await _tileService.GetTilesAsync();
            var shuffledTiles = tilesFromApi.OrderBy(t => Guid.NewGuid()).ToList();

            return shuffledTiles;
        }

        private async Task LoadTilesAsync()
        {
            var tilesFromApi = await _tileService.GetTilesAsync();
            var shuffledTiles = tilesFromApi.OrderBy(t => Guid.NewGuid()).ToList();

            Tiles = new ObservableCollection<Tile>();
            layers = tilesLayers();

            // Mapeia as peças
            MapTilesToLayers(shuffledTiles, layers);
        }

        private void MapTilesToLayers(List<Tile> shuffledTiles, List<int[,]> layers)
        {
            tileMap.Clear();

            int tileIndex = 0;
            Debug.WriteLine("Iniciando o mapeamento das peças...");

            if (shuffledTiles.Count != 144) // Verifica se está recebendo todas as 144 peças
            {
                Debug.WriteLine($"Número incorreto de peças recebidas da API: {shuffledTiles.Count} (esperado: 144)");
                return; // Impedir o mapeamento se o número de peças não for o esperado
            }

            for (int layer = 0; layer < layers.Count; layer++)
            {
                Debug.WriteLine($"Camada {layer}:");
                for (int row = 0; row < layers[layer].GetLength(0); row++)
                {
                    for (int col = 0; col < layers[layer].GetLength(1); col++)
                    {
                        if (layers[layer][row, col] == 1 && tileIndex < shuffledTiles.Count)
                        {
                            var tile = shuffledTiles[tileIndex];
                            tile.Layer = layer;
                            tile.Row = row;
                            tile.Col = col;
                            tileMap.Add((layer, row, col), tile);
                            tileIndex++;

                            Debug.WriteLine($"Tile mapeado: Layer: {layer}, Row: {row}, Col: {col}, Tile ID: {tile.id}");
                        }
                    }
                }
            }

            Debug.WriteLine("Mapeamento completo.");

            // Configura o layout após o mapeamento
            SetTileLayout(layers);
        }

        private void SetTileLayout(List<int[,]> layers)
        {
            int tileWidth = 60;
            int tileHeight = 75;
            int zSpacing = 5; // Espaçamento entre as camadas

            // Ajusta ao centro da tela
            double centerX = 300;
            double centerY = 400;

            for (int z = 0; z < layers.Count; z++)
            {
                var layer = layers[z];
                for (int y = 0; y < layer.GetLength(0); y++)
                {
                    for (int x = 0; x < layer.GetLength(1); x++)
                    {
                        if (layer[y, x] == 1)
                        {
                            var tilePosition = tileMap.FirstOrDefault(t => t.Key == (z, y, x));
                            if (tilePosition.Value != null)
                            {
                                var tile = tilePosition.Value;

                                double posX = centerX - (layer.GetLength(1) * tileWidth / 2) + x * tileWidth;
                                double posY = centerY - (layer.GetLength(0) * tileHeight / 2) + y * tileHeight - z * zSpacing;

                                tile.LayoutBounds = new Rect(posX, posY, tileWidth, tileHeight);
                                Tiles.Add(tile); // Adiciona à coleção que deve estar ligada à UI

                                Debug.WriteLine($"Tile ID: {tile.id}, Layer: {tile.Layer}, Row: {tile.Row}, Col: {tile.Col}, Posição: X: {posX}, Y: {posY}");
                            }
                        }
                    }
                }
            }
        }

        public bool IsTileFree(List<int[,]> layers, int layer, int row, int col)
        {
            // Verifica se a peça está bloqueada lateralmente (esquerda e direita)
            bool leftBlocked = col > 0 && layers[layer][row, col - 1] == 1;  // Se a peça à esquerda está presente
            bool rightBlocked = col < layers[layer].GetLength(1) - 1 && layers[layer][row, col + 1] == 1; // Se a peça à direita está presente

            // Se estiver bloqueada à esquerda e à direita, não pode ser removida
            if (leftBlocked && rightBlocked)
            {
                Debug.WriteLine($"Peça bloqueada lateralmente ({row}, {col}) na camada {layer}.");
                return false;
            }

            // Verificar se está bloqueada por cima (camada acima)
            if (layer < layers.Count - 1) // Há uma camada superior
            {
                bool topBlocked = layers[layer + 1][row, col] == 1;  // Se a peça acima está presente
                if (topBlocked)
                {
                    Debug.WriteLine($"Peça bloqueada por cima ({row}, {col}) na camada {layer}.");
                    return false;
                }
            }

            // Se não está bloqueada à esquerda, à direita ou por cima, a peça está livre
            Debug.WriteLine($"Peça livre ({row}, {col}) na camada {layer}.");
            return true;
        }

        public bool IsTileBlocked(int row, int col, int layer, List<int[,]> layers)
        {
            // Verifica se a posição está dentro dos limites do tabuleiro
            if (row < 0 || row >= layers[layer].GetLength(0) || col < 0 || col >= layers[layer].GetLength(1))
            {
                return false; // Fora do tabuleiro não pode estar bloqueada
            }

            return layers[layer][row, col] == 1;
        }

        public void RemoveTile(int layer, int row, int col, List<int[,]> layers)
        {
            // Verifica se a posição está dentro dos limites do tabuleiro
            if (layer < 0 || layer >= layers.Count || row < 0 || row >= layers[layer].GetLength(0) || col < 0 || col >= layers[layer].GetLength(1))
            {
                Debug.WriteLine("Tentativa de remover uma peça fora dos limites.");
                return; // Não remove fora do tabuleiro
            }

            // Atualiza a camada para indicar que a peça foi removida
            layers[layer][row, col] = 0; // Marca a posição como livre

            Debug.WriteLine($"Peça removida de ({row}, {col}) na camada {layer}.");
        }

        private void OnTileClicked(Tile tile)
        {
            var mappedTile = tileMap.Values.FirstOrDefault(t => t.id == tile.id);

            if (mappedTile == null)
            {
                Debug.WriteLine("Tile clicada não encontrada no mapa de tiles.");
                return;
            }

            if (_firstTileSelected == mappedTile)
            {
                return;
            }

            if (_firstTileSelected == null)
            {
                _firstTileSelected = mappedTile;
                mappedTile.Color = Colors.Blue;
                Debug.WriteLine($"Primeira peça selecionada: Layer: {mappedTile.Layer}, Row: {mappedTile.Row}, Col: {mappedTile.Col}");
            }
            else if (_secondTileSelected == null)
            {
                _secondTileSelected = mappedTile;
                mappedTile.Color = Colors.Blue;
                Debug.WriteLine($"Segunda peça selecionada: Layer: {mappedTile.Layer}, Row: {mappedTile.Row}, Col: {mappedTile.Col}");
                CheckForMatch(mappedTile, _firstTileSelected, _secondTileSelected);
            }
        }

        private async void CheckForMatch(Tile tile, Tile firstTile, Tile secondTile)
        {
            if (_firstTileSelected != null && _secondTileSelected != null)
            {
                // Obter a posição da primeira e da segunda peça
                var firstTilePos = tileMap.FirstOrDefault(x => x.Value.id == _firstTileSelected.id).Key;
                var secondTilePos = tileMap.FirstOrDefault(x => x.Value.id == _secondTileSelected.id).Key;

                // Verificar se ambas as peças estão livres
                bool isFirstTileFree = IsTileFree(layers, firstTilePos.layer, firstTilePos.row, firstTilePos.col);
                bool isSecondTileFree = IsTileFree(layers, secondTilePos.layer, secondTilePos.row, secondTilePos.col);

                // Obter as peças a serem removidas
                var tileToRemove1 = Tiles.FirstOrDefault(t => t.id == _firstTileSelected.id);
                var tileToRemove2 = Tiles.FirstOrDefault(t => t.id == _secondTileSelected.id);

                Debug.WriteLine($"Primeira peça livre: {isFirstTileFree}, Segunda peça livre: {isSecondTileFree}");

                if (isFirstTileFree && isSecondTileFree)
                {
                    // Verificar se os símbolos e as cores são iguais
                    bool tilesMatch = _firstTileSelected.simbolo == _secondTileSelected.simbolo
                                      && _firstTileSelected.cor == _secondTileSelected.cor;

                    Debug.WriteLine($"As peças correspondem: {tilesMatch}");

                    if (tilesMatch)
                    {
                        Debug.WriteLine("Removendo as peças correspondentes!");

                        // Remover as peças correspondentes
                        Tiles.Remove(tileToRemove1);
                        RemoveTile(firstTilePos.layer, firstTilePos.row, firstTilePos.col, layers);
                        Tiles.Remove(tileToRemove2);
                        RemoveTile(secondTilePos.layer, secondTilePos.row, secondTilePos.col, layers);

                        Debug.WriteLine($"Peças restantes: {Tiles.Count}");

                        // Atualizar a pontuação
                        _score += tile.pontuacao;
                        ScoreText = $"Score: {_score}";

                        // Verificar se o jogo foi ganho
                        if (Tiles.Count == 0)
                        {
                            GameWin();
                        }

                        _firstTileSelected = null;
                        _secondTileSelected = null;
                    }
                    else
                    {
                        Debug.WriteLine("As peças não correspondem, resetando seleção.");
                        // Resetar a cor se não houver correspondência
                        _firstTileSelected.Color = Colors.Transparent;

                        // Manter a segunda peça como a primeira para a próxima tentativa
                        _firstTileSelected = _secondTileSelected;
                        _secondTileSelected = null;
                    }
                }
                else
                {
                    Debug.WriteLine("Uma ou ambas as peças não estão livres, desmarcando seleção.");
                    // Se uma das peças não estiver livre, desmarcar a seleção
                    _firstTileSelected.Color = Colors.Transparent;
                    _secondTileSelected.Color = Colors.Transparent;

                    _firstTileSelected = null;
                    _secondTileSelected = null;
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