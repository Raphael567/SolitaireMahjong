using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SolitaireMahjongApp.Models;
using SolitaireMahjongApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolitaireMahjongApp.ViewModels
{
    public partial class PlayerViewModel : ObservableObject
    {
        private readonly PlayerService _playerService;

        public PlayerViewModel()
        {
            LoadPlayerCommand = new AsyncRelayCommand(LoadPlayerAsync);
            CreatePlayerCommand = new AsyncRelayCommand(CreatePlayerAsync);
        }

        [ObservableProperty]
        private List<Player> _players;

        [ObservableProperty]
        private string _playerName;

        [ObservableProperty]
        private int _playerScore;

        public IAsyncRelayCommand LoadPlayerCommand { get; }
        public IAsyncRelayCommand CreatePlayerCommand { get; }

        private async Task LoadPlayerAsync()
        {
            Players = await _playerService.GetAllPlayersAsync();
        }

        private async Task CreatePlayerAsync()
        {
            var newPlayer = new Player
            {
                Name = PlayerName,
                Score = PlayerScore
            };

            await _playerService.CreatePlayerAsync(newPlayer);
            await LoadPlayerAsync();
        }
    }
}
