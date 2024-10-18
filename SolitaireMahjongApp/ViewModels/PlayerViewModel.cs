using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SolitaireMahjongApp.Models;
using SolitaireMahjongApp.Services;
using SolitaireMahjongApp.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
            CreatePlayerCommand = new AsyncRelayCommand(CreatePlayerAsync);
            NavigateCommand = new AsyncRelayCommand(NavigateAsync);
        }

        [ObservableProperty]
        private List<Player> _players;

        [ObservableProperty]
        private string _playerName;

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
                if(_players == null)
                {
                    await LoadPlayerAsync();
                }

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

                    var createdPlayer = await _playerService.CreatePlayerAsync(newPlayer);
                    _sessionService.currentPlayer = createdPlayer;

                    await LoadPlayerAsync();

                    Debug.WriteLine($"Id do player {_sessionService.currentPlayer.id}");

                    await Application.Current.MainPage.Navigation.PushAsync(new MahjongView(_sessionService));
                }
            }
            catch(ArgumentNullException ex) 
            {
                Debug.WriteLine($"Erro ao criar jogador {ex.Message}");
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Erro ao criar jogador", "OK");
            }
        }

        private async Task NavigateAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RankingView());
        }
    }
}