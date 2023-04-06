using Microsoft.AspNetCore.SignalR.Client;
using ServerTicTacToeHandin.Models;

namespace MauiTicTacToeHandin.Views;

public partial class GamePage : ContentPage
{
	public GamePage(GameViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        viewModel._hub = new HubConnectionBuilder()
            .WithUrl("http://192.168.1.244:5000/game")
            .Build();

        //viewModel._hub.On<string>("MessageReceived", viewModel.AppendChat);
        viewModel._hub.On<string>("UpdateGame", viewModel.UpdateGame);
        viewModel._hub.On<string>("PlayerLeft", viewModel.PlayerLeft);

        Task.Run(() =>
        {
            Dispatcher.Dispatch(async () => await viewModel._hub.StartAsync());
        });
    }
}
