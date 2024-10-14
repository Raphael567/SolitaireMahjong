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
}