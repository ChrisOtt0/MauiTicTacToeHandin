using Microsoft.AspNetCore.SignalR.Client;

namespace MauiTicTacToeHandin.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    public HubConnection _hub = null;

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
}
