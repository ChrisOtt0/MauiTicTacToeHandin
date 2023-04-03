namespace MauiTicTacToeHandin.Views;

public partial class HighscorePage : ContentPage
{
	public HighscorePage(HighscoreViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
