using SolitaireMahjongApp.ViewModels;

namespace SolitaireMahjongApp.Views;

public partial class RankingView : ContentPage
{
	public RankingView()
	{
		InitializeComponent();
        BindingContext = new RankingViewModel();
    }
}