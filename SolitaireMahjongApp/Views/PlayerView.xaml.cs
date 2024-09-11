using Microsoft.Maui.Controls;
using SolitaireMahjongApp.Services;
using SolitaireMahjongApp.ViewModels;

namespace SolitaireMahjongApp.Views
{
    public partial class PlayerView : ContentPage
    {
        public PlayerView(PlayerViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
