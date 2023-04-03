namespace MauiTicTacToeHandin.Views;

public partial class NamePage : ContentPage
{
	public NamePage(NameViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
