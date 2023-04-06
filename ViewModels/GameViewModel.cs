using MauiTicTacToeHandin.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using ServerTicTacToeHandin.Models;

namespace MauiTicTacToeHandin.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    public HubConnection _hub = null;

    private GameService _gameService;
    private bool _isTurn = false;
    private int connected = 0;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string groupName;

    [ObservableProperty]
    private string result = "Null";

    public GameViewModel(GameService gameService)
    {
        _gameService = gameService;
    }

    [RelayCommand]
    private async void CreateGame()
    {
        await _hub.InvokeCoreAsync("CreateGame", args: new[] { Name, GroupName });
    }

    [RelayCommand]
    private async void JoinGame()
    {
        await _hub.InvokeCoreAsync("JoinGame", args: new[] { Name, GroupName });
    }

    public void PlayerLeft(string playerName)
    {
        _gameService.Session = null;
        _gameService.PlayerId = null;
    }

    public void UpdateGame(string session)
    {
        Debug.WriteLine("UpdateGame called");
        Session s = JsonConvert.DeserializeObject<Session>(session);
        _gameService.Session = s;

        if (_gameService.PlayerId != null)
        {
            foreach (var player in _gameService.Session.Players)
            {
                if (player.Name == Name) _gameService.PlayerId = player.Id;
            }
        }

        connected += 1;
        Result = "Success: " + connected;
        Debug.WriteLine("Success!");
    }

    #region ChatTest
    [ObservableProperty]
    private string chat;

    [ObservableProperty]
    private string message;


    [RelayCommand]
    private async void SendMessage()
    {
        await _hub.InvokeCoreAsync("SendMessage", args: new[] { Message });
        Message = string.Empty;
    }

    public void AppendChat(string message)
    {
        if (Chat == null) Chat = "";

        Chat += $"{Environment.NewLine}Anon: {message}";
    }
    #endregion
}
