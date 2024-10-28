using SolitaireMahjongApp.Services;
using SolitaireMahjongApp.ViewModels;
using System.Globalization;

namespace SolitaireMahjongApp.Views;

public partial class MahjongView : ContentPage
{
	public MahjongView(SessionService sessionService)
	{
		InitializeComponent();
		BindingContext = new MahjongViewModel(sessionService);
	}

    private async void AnimatedImageButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;

        // Aumentar a escala
        await imageButton.ScaleTo(1.1, 100, Easing.CubicIn); // Aumenta de tamanho

        // Esperar um pouco antes de reverter a animação
        await Task.Delay(200);

        // Retornar ao tamanho original
        await imageButton.ScaleTo(1.0, 100, Easing.CubicOut); // Volta ao tamanho original
    }

    //private async Task AnimateTileShaking(ImageButton button)
    //{
    //    const int shakes = 5;
    //    const double shakeDistance = 10;

    //    for (int i = 0; i < shakes; i++)
    //    {
    //        // Move para a esquerda
    //        await button.TranslateTo(-shakeDistance, 0, 50, Easing.Linear);
    //        // Move para a direita
    //        await button.TranslateTo(shakeDistance, 0, 50, Easing.Linear);
    //    }

    //    // Retorna à posição original
    //    await button.TranslateTo(0, 0, 50, Easing.Linear);
    //}

    //private async Task AnimateTileMatching(ImageButton imageButton)
    //{
    //    // Animação: Crescer
    //    await imageButton.ScaleTo(1.2, 200, Easing.CubicIn);

    //    // Animação: Voltar ao tamanho original
    //    await imageButton.ScaleTo(1.0, 200, Easing.CubicOut);

    //    // Opcional: Desvanecer a peça (se desejar)
    //    await imageButton.FadeTo(0, 200);
    //}
}