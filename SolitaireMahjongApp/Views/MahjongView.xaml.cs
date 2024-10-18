using SolitaireMahjongApp.Services;
using SolitaireMahjongApp.ViewModels;

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
}