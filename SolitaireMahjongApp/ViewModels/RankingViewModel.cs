using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SolitaireMahjongApp.Models;
using SolitaireMahjongApp.Services;
using System.Diagnostics;

namespace SolitaireMahjongApp.ViewModels
{
    public partial class RankingViewModel : ObservableObject
    {
        private readonly PlayerService _playerService;

        public RankingViewModel()
        {
            _playerService = new PlayerService();
            NavigateBackCommand = new AsyncRelayCommand(NavigateBackAsync);
            LoadPlayersCommand = new AsyncRelayCommand(LoadPlayersAsync);

            Task.Run(async () => await LoadPlayersAsync());
        }

        [ObservableProperty]
        private List<RankedPlayer> _players;

        public IAsyncRelayCommand LoadPlayersCommand { get; }
        public IAsyncRelayCommand NavigateBackCommand { get; }

        private async Task LoadPlayersAsync()
        {
            var players = await _playerService.GetAllPlayersAsync();
            if (players != null && players.Count > 0)
            {
                Debug.WriteLine($"Total players loaded: {players.Count}");
                var rankedPlayers = players.Select((player, index) => new RankedPlayer
                {
                    rank = index + 1,
                    player = player
                }).ToList();

                Players = rankedPlayers;
            }
        }

        private async Task NavigateBackAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
