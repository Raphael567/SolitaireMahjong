using SolitaireMahjongApp.Models;
using SolitaireMahjongApp.Services;
using SolitaireMahjongApp.ViewModels;
using System.Diagnostics;
using System.Globalization;

namespace SolitaireMahjongApp.Views;

public partial class MahjongView : ContentPage
{
    private readonly IAnimationService _animationService;

    public MahjongView(SessionService sessionService)
	{
		InitializeComponent();
        _animationService = new AnimationService();
        BindingContext = new MahjongViewModel(sessionService, _animationService);
    }

    private async void OnTileClicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        var tile = button.BindingContext as Tile;

        if (tile == null)
        {
            Debug.WriteLine("Tile is null.");
            return; // Saia se tile for nulo
        }

        // Acesse a ViewModel
        var viewModel = BindingContext as MahjongViewModel;

        // Mapeia o tile ao botão na ViewModel
        viewModel.MapTileToButton(tile, button);

        // Chama a animação ou outra lógica
        await AnimateTile(button);
    }

    private async Task AnimateTile(ImageButton imageButton)
    {
        // Aumentar a escala
        await imageButton.ScaleTo(1.1, 100, Easing.CubicIn); // Aumenta de tamanho

        // Esperar um pouco antes de reverter a animação
        await Task.Delay(200);

        // Retornar ao tamanho original
        await imageButton.ScaleTo(1.0, 100, Easing.CubicOut); // Volta ao tamanho original
    }
}