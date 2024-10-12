using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SolitaireMahjongApp.Models;
using SolitaireMahjongApp.Services;
using SolitaireMahjongApp.Views;

namespace SolitaireMahjongApp.ViewModels
{
    public partial class PlayerViewModel : ObservableObject
    {
        private readonly PlayerService _playerService;
        private readonly SessionService _sessionService;

        public PlayerViewModel()
        {
            _playerService = new PlayerService();
            _sessionService = new SessionService();
            LoadPlayerCommand = new AsyncRelayCommand(LoadPlayerAsync);
            CreatePlayerCommand = new AsyncRelayCommand(CreatePlayerAsync);
            NavigateCommand = new AsyncRelayCommand(NavigateAsync);

            Task.Run(async () => await LoadPlayerAsync());
        }

        [ObservableProperty]
        private List<Player> _players;

        [ObservableProperty]
        private string _playerName;

        public IAsyncRelayCommand LoadPlayerCommand { get; }
        public IAsyncRelayCommand CreatePlayerCommand { get; }
        public IAsyncRelayCommand NavigateCommand { get; }

        private async Task LoadPlayerAsync()
        {
            Players = await _playerService.GetAllPlayersAsync();
        }

        private async Task CreatePlayerAsync()
        {
            try
            {
                var existingPlayer = _players.FirstOrDefault(player => player.nome == PlayerName);

                if (existingPlayer != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Jogador já existe", "OK");
                }
                else
                {
                    var newPlayer = new Player
                    {
                        nome = PlayerName,
                        pontuacao = 0
                    };

                    _sessionService.currentPlayer = newPlayer;

                    await _playerService.CreatePlayerAsync(_sessionService.currentPlayer);
                    await LoadPlayerAsync();
                    await Application.Current.MainPage.Navigation.PushAsync(new MahjongView());
                }
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Erro ao criar jogador", "OK");
            }
        }


        private async Task NavigateAsync()
        {
            // Navega para a nova página
            await Application.Current.MainPage.Navigation.PushAsync(new RankingView());
        }

        //private void OnSizeChanged(object sender, EventArgs e)
        //{
        //    // Obtém a largura da tela
        //    double width = Application.Current.MainPage.Width;

        //    if (width > 600)
        //    {
        //        // Modo para telas menores
        //        mainStack.Spacing = 40;  // Diminui o espaçamento
        //        mainStack.WidthRequest = 2;  // Ajusta a largura do stack layout
        //    }
        //    else
        //    {
        //        // Modo para telas maiores
        //        mainStack.Spacing = 80;  // Aumenta o espaçamento
        //        mainStack.WidthRequest = 500;  // Ajusta a largura do stack layout
        //    }
        //}
    }
}