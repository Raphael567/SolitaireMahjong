using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SolitaireMahjongApp.Models;
using SolitaireMahjongApp.Services;
using SolitaireMahjongApp.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace SolitaireMahjongApp.ViewModels
{
    public partial class MahjongViewModel : ObservableObject
    {
        private readonly TileService _tileService;
        private readonly PlayerService _playerService;
        private readonly SessionService _sessionService;
        private readonly IAnimationService _animationService;

        [ObservableProperty]
        private string timerText;

        [ObservableProperty]
        private string scoreText;

        [ObservableProperty]
        private Player currentPlayer;

        [ObservableProperty]
        private ObservableCollection<Tile> _tiles;

        [ObservableProperty]
        private bool isPaused = false;

        [ObservableProperty]
        private bool isGameOver = false;

        [ObservableProperty]
        private bool isLoading = false;

        public Tile _firstTileSelected = null;
        public Tile _secondTileSelected = null;
        private int _score = 0;
        private int _timeLeft = 3600;
        private bool shuffleTiles = false;
        private bool pauseTimer = false;
        private bool forcedGameOver = false;

        private List<int[,]> layers;
        private Dictionary<(int layer, int row, int col), Tile> tileMap;
        private (Tile, Tile)? lastHintPair = null;
        public Dictionary<Tile, ImageButton> TileButtonMapping { get; set; }

        public ICommand TileCommand { get; }

        public IAsyncRelayCommand LoadTilesCommand { get; }
        public IAsyncRelayCommand HintCommand { get; }

        public IRelayCommand PauseCommand { get; }
        public IRelayCommand ResumeCommand { get; }
        public IRelayCommand EndGameCommand { get; }
        public IRelayCommand GameOverCommand { get; }

        public MahjongViewModel(SessionService sessionService, IAnimationService animationService)
        {
            try
            {
                _tileService = new TileService();
                _playerService = new PlayerService();
                _sessionService = sessionService;
                _animationService = animationService;

                InitializePlayerAsync();

                Tiles = new ObservableCollection<Tile>();
                TileButtonMapping = new Dictionary<Tile, ImageButton>();

                TileCommand = new RelayCommand<Tile>(OnTileClicked);
                LoadTilesCommand = new AsyncRelayCommand(LoadTilesAsync);
                HintCommand = new AsyncRelayCommand(ShowHint);

                PauseCommand = new RelayCommand(PauseGame);
                ResumeCommand = new RelayCommand(ResumeGame);
                EndGameCommand = new RelayCommand(EndGame);
                GameOverCommand = new RelayCommand(GameOver);

                tileMap = new Dictionary<(int layer, int row, int col), Tile>();

                Task.Run(async () =>
                {
                    IsLoading = true;
                    await LoadTilesAsync();
                    IsLoading = false;
                    StartTimer();
                    
                });
            }
            catch (COMException ex)
            {
                Debug.WriteLine($"Erro ao inciar o jogo: {ex.Message}");
            }
        }

        public async void InitializePlayerAsync()
        {
            CurrentPlayer = _sessionService.currentPlayer;

            if (CurrentPlayer == null || CurrentPlayer.id == 0)
            {
                await _playerService.CreatePlayerAsync(CurrentPlayer);
                _sessionService.currentPlayer = CurrentPlayer;
                Debug.WriteLine($"Jogador existente carregado: ID {CurrentPlayer.id} com o nome {CurrentPlayer.nome}");
            }
            else
            {
                Debug.WriteLine($"Jogador existente carregado: ID {CurrentPlayer.id} com o nome {CurrentPlayer.nome}");
            }
        }

        private async Task<List<Tile>> ShuffleTiles()
        {
            var shuffledTiles = Tiles.OrderBy(t => Guid.NewGuid()).ToList();

            return shuffledTiles;
        }

        public void MapTileToButton(Tile tile, ImageButton button)
        {
            if (tile == null) throw new ArgumentNullException(nameof(tile), "Tile cannot be null.");
            if (button == null) throw new ArgumentNullException(nameof(button), "Button cannot be null.");

            if (!TileButtonMapping.ContainsKey(tile))
            {
                TileButtonMapping[tile] = button;
                Debug.WriteLine($"Tile {tile.id} mapeado para o botão.");
            }
            else
            {
                Debug.WriteLine($"Tile {tile.id} já está mapeado.");
            }
        }

        private async Task LoadTilesAsync()
        {
            try
            {
                Debug.WriteLine("Iniciando a carga de tiles...");

                var tilesFromApi = await _tileService.GetTilesAsync();
                Debug.WriteLine($"Tiles recebidos: {tilesFromApi.Count}");

                var shuffledTiles = tilesFromApi.OrderBy(t => Guid.NewGuid()).ToList();
                Debug.WriteLine("Tiles embaralhados.");

                var layerManager = new LayerManager();
                layers = layerManager.GetRandomLayer();
                Debug.WriteLine("Camadas geradas.");

                // Mapeia as peças
                MapTilesToLayers(shuffledTiles, layers);

                //// Inicializa o mapeamento de tiles para botões
                //InitializeTileButtonMapping(shuffledTiles, layers);

                await Task.Delay(500);
            }
            catch (COMException ex)
            {
                Debug.WriteLine($"Erro na navegação das peças: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar peças: {ex.Message}");
            }
        }

        private void MapTilesToLayers(List<Tile> shuffledTiles, List<int[,]> layers)
        {
            tileMap.Clear();

            int tileIndex = 0;

            Debug.WriteLine("Iniciando o mapeamento das peças...");

            if (shuffledTiles.Count != 144 && !shuffleTiles) // Verifica se está recebendo todas as 144 peças
            {
                Debug.WriteLine($"Número incorreto de peças recebidas da API: {shuffledTiles.Count} (esperado: 144)");
                return;
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
                            var button = new ImageButton();

                            tile.Layer = layer;
                            tile.Row = row;
                            tile.Col = col;
                            tileMap.Add((layer, row, col), tile);
                            MapTileToButton(tile, button);
                            tileIndex++;

                            Debug.WriteLine($"Tile mapeado: Layer: {layer}, Row: {row}, Col: {col}, Tile ID: {tile.id}");
                        }
                    }
                }
            }

            Debug.WriteLine("Mapeamento completo.");
            Debug.WriteLine(tileMap.Count);

            // Configura o layout após o mapeamento
            SetTileLayout(layers);
        }

        private void SetTileLayout(List<int[,]> layers)
        {
            Tiles.Clear();

            int tileWidth = 60;
            int tileHeight = 75;
            int zSpacing = 15; // Espaçamento entre as camadas

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

                                tile.Color = Colors.Transparent;

                                double posX = centerX - (layer.GetLength(1) * tileWidth / 2) + x * tileWidth;
                                double posY = centerY - (layer.GetLength(0) * tileHeight / 2) + y * tileHeight - z * zSpacing;

                                tile.LayoutBounds = new Rect(posX, posY, tileWidth, tileHeight);

                                Application.Current.Dispatcher.Dispatch(() =>
                                {
                                    Tiles.Add(tile);
                                });

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

            //Verificar se está bloqueada por cima(camada acima)
            if (layer + 1 < layers.Count) // Há uma camada superior
            {
                // Verificar se row e col estão dentro dos limites da camada superior
                int rowsInLayerAbove = layers[layer + 1].GetLength(0);
                int colsInLayerAbove = layers[layer + 1].GetLength(1);

                if (row < rowsInLayerAbove && col < colsInLayerAbove)
                {
                    bool topBlocked = layers[layer + 1][row, col] == 1;  // Se a peça acima está presente
                    if (topBlocked)
                    {
                        Debug.WriteLine($"Peça bloqueada por cima ({row}, {col}) na camada {layer}.");
                        return false;
                    }
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

            //var freeTiles = GetFreeTiles();

            //if (!freeTiles.Any(pair => pair.Item1 == mappedTile))
            //{
            //    Debug.WriteLine("Peça não está livre e não pode ser selecionada.");
            //    return;
            //}

            if (_firstTileSelected == mappedTile)
            {
                return;
            }

            if (_firstTileSelected == null)
            {
                _firstTileSelected = mappedTile;
                mappedTile.Color = Colors.Blue;
                mappedTile.IsHighlighted = true;
                Debug.WriteLine($"Primeira peça selecionada: Layer: {mappedTile.Layer}, Row: {mappedTile.Row}, Col: {mappedTile.Col}");
            }

            else if (_secondTileSelected == null)
            {
                _secondTileSelected = mappedTile;
                mappedTile.Color = Colors.Blue;
                mappedTile.IsHighlighted = true;
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

                        var imageButton1 = TileButtonMapping[tileToRemove1];
                        var imageButton2 = TileButtonMapping[tileToRemove2];

                        if (imageButton1.IsVisible && imageButton2.IsVisible)
                        {

                            if (imageButton1.IsVisible && imageButton2.IsVisible)
                            {
                                await Task.Run(() =>
                                {
                                    Application.Current?.Dispatcher.Dispatch(() =>
                                    {
                                        // Este código é executado na UI thread
                                        _animationService.AnimateButtonAsync(imageButton1);
                                        _animationService.AnimateButtonAsync(imageButton2);
                                    });
                                });
                            }
                        }

                        await Task.Delay(200);

                        Tiles.Remove(tileToRemove1);
                        RemoveTile(firstTilePos.layer, firstTilePos.row, firstTilePos.col, layers);

                        Tiles.Remove(tileToRemove2);
                        RemoveTile(secondTilePos.layer, secondTilePos.row, secondTilePos.col, layers);

                        Debug.WriteLine($"Peças restantes: {Tiles.Count}");

                        // Atualizar a pontuação
                        _score += tile.pontuacao;
                        ScoreText = $"Score: {_score}";
                        
                        CheckGameOver();

                        // Verificar se o jogo foi ganho
                        if (Tiles.Count == 0)
                        {
                            Application.Current.MainPage.DisplayAlert("Você Ganhou", "Todas as peças foram removidas", "OK");
                        }

                        _firstTileSelected = null;
                        _secondTileSelected = null;
                    }
                    else
                    {
                        Debug.WriteLine("As peças não correspondem, resetando seleção.");

                        // Resetar a cor se não houver correspondência
                        _firstTileSelected.Color = Colors.Transparent;
                        _firstTileSelected.IsHighlighted = false;

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
                    _firstTileSelected.IsHighlighted = false;
                    _secondTileSelected.IsHighlighted = false;

                    _firstTileSelected = null;
                    _secondTileSelected = null;
                }
            }
        }

        public List<(Tile, Tile)> GetFreeTiles()
        {
            var freeTiles = new List<(Tile, Tile)>();

            foreach (var tile in Tiles)
            {
                var tilePos = tileMap.FirstOrDefault(x => x.Value.id == tile.id).Key;
                if (IsTileFree(layers, tilePos.layer, tilePos.row, tilePos.col))
                {
                    freeTiles.Add((tile, tile));
                }
            }

            return freeTiles;
        }

        public List<(Tile, Tile)> GetFreePairs()
        {
            var freeTiles = new List<(Tile, Tile)>();

            foreach (var tile in Tiles)
            {
                var tilePos = tileMap.FirstOrDefault(x => x.Value.id == tile.id).Key;
                if (IsTileFree(layers, tilePos.layer, tilePos.row, tilePos.col))
                {
                    freeTiles.Add((tile, tile));
                }
            }

            var pairs = new List<(Tile, Tile)>();

            for (int i = 0; i < freeTiles.Count; i++)
            {
                for (int j = i + 1; j < freeTiles.Count; j++)
                {
                    if (freeTiles[i].Item1.simbolo == freeTiles[j].Item1.simbolo && freeTiles[i].Item1.cor == freeTiles[j].Item1.cor)
                        pairs.Add((freeTiles[i].Item1, freeTiles[j].Item2));
                }
            }

            return pairs;
        }

        public Task<(Tile, Tile)?> ShowHint()
        {
            if (lastHintPair != null)
            {
                lastHintPair.Value.Item1.Color = Colors.Transparent;
                lastHintPair.Value.Item2.Color = Colors.Transparent;

                lastHintPair = null;
            }

            var freeTilesPairs = GetFreePairs();

            if (freeTilesPairs.Count > 0)
            {
                var random = new Random();
                int index = random.Next(freeTilesPairs.Count);
                var hintPair = freeTilesPairs[index];

                hintPair.Item1.Color = Colors.Green;
                hintPair.Item2.Color = Colors.Green;

                hintPair.Item1.IsHighlighted = true;
                hintPair.Item2.IsHighlighted = true;

                lastHintPair = hintPair;
                _score--;
                ScoreText = $"Score: {_score}";

                return Task.FromResult<(Tile, Tile)?>(hintPair);
            }

            Application.Current.MainPage.DisplayAlert("Sem dicas", "Não há mais jogadas disponíveis", "OK");

            return Task.FromResult<(Tile, Tile)?>(null);
        }

        private async void StartTimer()
        {
            Debug.WriteLine("Iniciando o temporizador.");
            while (_timeLeft > 0 && Tiles.Count != 0)
            {
                while(IsPaused || pauseTimer)
                {
                    await Task.Delay(10);
                }

                await Task.Delay(1000);
                _timeLeft--;
                TimerText = $"{TimeSpan.FromSeconds(_timeLeft):mm\\:ss}";
            }

            if (_timeLeft == 0 && !IsGameOver)
            {
                Debug.WriteLine("O tempo acabou.");
                CheckGameOver();
            }
        }

        private async void CheckGameOver()
        {
            if (IsGameOver) return;

            var freeTilesPairs = GetFreePairs();

            // Linha para testar o embaralhmento do tabuleiro caso não haja mais pares disponíveis
            //freeTilesPairs.Clear();

            if (_timeLeft <= 0)
            {
                Debug.WriteLine("O tempo acabou.");
                IsGameOver = true;
                return;
            }

            if (freeTilesPairs.Count == 0)
            {
                pauseTimer = true;

                shuffleTiles = await Application.Current.MainPage.DisplayAlert(
                              "Jogo encerrado",
                              "Não há mais pares disponíveis. Deseja embaralhar as peças?",
                              "Embaralhar",
                              "Encerrar Jogo");

                if (shuffleTiles)
                {
                    _score -= 10;
                    ScoreText = $"Score: {_score}";

                    var shuffledTiles = await ShuffleTiles();

                    Tiles.Clear();

                    MapTilesToLayers(shuffledTiles, layers);

                    pauseTimer = false;

                    var freeTiles = GetFreeTiles();

                    if (freeTiles.Count == 0)
                    {
                        Application.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Application.Current.MainPage.DisplayAlert("Fim de jogo", "Não há mais peças disponíveis após embaralhamento", "OK");
                        });

                        IsGameOver = true;
                    }
                }
                else
                    IsGameOver = true;
            }

            else if (_timeLeft == 0)
            {
                Debug.WriteLine("O tempo acabou");
                IsGameOver = true;
            }
        }

        private async void GameOver()
        {
            try
            {
                pauseTimer = true;

                Debug.WriteLine("Tentando encerrar o jogo...");

                if (CurrentPlayer == null)
                {
                    Debug.WriteLine("currentPlayer é nulo.");
                    return;
                }

                CurrentPlayer.pontuacao = _score;
                _score = 0;
                ScoreText = $"Score: {_score}";

                if(Tiles.Any())
                {
                    Application.Current.Dispatcher.Dispatch(() =>
                    {
                        Tiles.Clear();
                    });
                }

                if(!IsPaused)
                {
                    Debug.WriteLine($"Tentando salvar a pontuação do jogador com o id {currentPlayer.id}");
                    bool isSuccess = await _playerService.UpdatePlayerAsync(currentPlayer);
                    if (isSuccess)
                    {
                        Debug.WriteLine("Pontuação salva com sucesso.");
                        Application.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Application.Current.MainPage.DisplayAlert("Fim de Jogo", "Pontuação salva com sucesso", "OK");
                        });
                    }
                    else
                    {
                        Debug.WriteLine("Erro ao salvar a pontuação.");
                        Application.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível salvar a pontuação", "OK");
                        });
                    }

                    Debug.WriteLine("Navegando para PlayerView...");
                    Application.Current.Dispatcher.Dispatch(async () =>
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new PlayerView());
                    });
                } 
                else
                {
                    await _playerService.DeletePlayerAsync(CurrentPlayer.id);

                    Application.Current.Dispatcher.Dispatch(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert("Jogo encerrado", "O jogo foi encerrado", "OK");
                        await Application.Current.MainPage.Navigation.PushAsync(new PlayerView());
                    });
                }
            }
            catch (COMException comEx)
            {
                Debug.WriteLine($"COMException: {comEx.Message}");
                Application.Current.Dispatcher.Dispatch(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao encerrar o jogo: {comEx.Message}", "OK");
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
                Application.Current.Dispatcher.Dispatch(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", $"Não foi possível salvar a pontuação: {e.Message}", "OK");
                });
            }
            finally
            {
                Debug.WriteLine("Finalizando o jogo.");
            }
        }

        private void PauseGame()
        {
            IsPaused = true;
            pauseTimer = true;
        }

        private void ResumeGame()
        {
            IsPaused = false;
            pauseTimer = false;
        }

        private void EndGame()
        {
            forcedGameOver = true;
            GameOver();
        }
    }
}