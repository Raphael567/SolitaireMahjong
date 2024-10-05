using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SolitaireMahjongApp.Models;
using SolitaireMahjongApp.Services;

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
        private List<Player> _players;

        public IAsyncRelayCommand LoadPlayersCommand { get; }
        public IAsyncRelayCommand NavigateBackCommand { get; }

        private async Task LoadPlayersAsync()
        {
            Players = await _playerService.GetAllPlayersAsync();
        }

        private async Task NavigateBackAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
