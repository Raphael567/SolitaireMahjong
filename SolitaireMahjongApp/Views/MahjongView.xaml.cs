using SolitaireMahjongApp.ViewModels;

namespace SolitaireMahjongApp.Views;

public partial class MahjongView : ContentPage
{
	public MahjongView()
	{
		InitializeComponent();
		BindingContext = new MahjongViewModel();
	}
}