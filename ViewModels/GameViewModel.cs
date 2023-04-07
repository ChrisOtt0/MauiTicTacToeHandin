using MauiTicTacToeHandin.Models;
using MauiTicTacToeHandin.Services;
using MauiTicTacToeHandin.Tools;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using ServerTicTacToeHandin.Models;

namespace MauiTicTacToeHandin.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    public HubConnection _hub = null;

    private GameService _gameService;
    private HighscoreDatabase _highscoreDatabase;

    [ObservableProperty]
    private bool isTurn = false;

    [ObservableProperty]
    private bool hasEnded = false;

    [ObservableProperty]
    private string groupName = string.Empty;

    [ObservableProperty]
    private string feedbackLabel = "Test";

    [ObservableProperty]
    private string btnTl = string.Empty;

    [ObservableProperty]
    private string btnTm = string.Empty;

    [ObservableProperty]
    private string btnTr = string.Empty;

    [ObservableProperty]
    private string btnMl = string.Empty;

    [ObservableProperty]
    private string btnMm = string.Empty;

    [ObservableProperty]
    private string btnMr = string.Empty;

    [ObservableProperty]
    private string btnBl = string.Empty;

    [ObservableProperty]
    private string btnBm = string.Empty;

    [ObservableProperty]
    private string btnBr = string.Empty;

    [ObservableProperty]
    private bool canPlayTl = false;

    [ObservableProperty]
    private bool canPlayTm = false;

    [ObservableProperty]
    private bool canPlayTr = false;

    [ObservableProperty]
    private bool canPlayMl = false;

    [ObservableProperty]
    private bool canPlayMm = false;

    [ObservableProperty]
    private bool canPlayMr = false;

    [ObservableProperty]
    private bool canPlayBl = false;

    [ObservableProperty]
    private bool canPlayBm = false;

    [ObservableProperty]
    private bool canPlayBr = false;

    public GameViewModel(GameService gameService, HighscoreDatabase highscoreDatabase)
    {
        _gameService = gameService;
        _highscoreDatabase = highscoreDatabase;
    }

    [RelayCommand]
    private async void CreateGame()
    {
        if (_gameService.PlayerName == null || _gameService.PlayerName == string.Empty)
        {
            FeedbackLabel = "Please set player name first.";
            return;
        }

        await _hub.InvokeCoreAsync("CreateGame", args: new[] { _gameService.PlayerName, GroupName });
    }

    [RelayCommand]
    private async void JoinGame()
    {
        if (_gameService.PlayerName == null || _gameService.PlayerName == string.Empty)
        {
            FeedbackLabel = "Please set player name first.";
            return;
        }

        await _hub.InvokeCoreAsync("JoinGame", args: new[] { _gameService.PlayerName, GroupName });
    }

    [RelayCommand]
    private async void ResetGame()
    {
        await _hub.InvokeCoreAsync("ResetGame", args: new[] { GroupName });
    }

    [RelayCommand]
    private async void BtnPress(object parameter)
    {
        int[] xy = ConvertBtnToXY(parameter);
        Field f = _gameService.Session.Players[(int)_gameService.PlayerId].AssignedField;
        MoveRequest mr = new MoveRequest() { Row = xy[0], Col = xy[1], Move = f };
        await _hub.InvokeCoreAsync("UpdateGame", args: new object[] { GroupName, mr });
    }

    public void PlayerLeft(string playerName)
    {
        _gameService.Session = null;
        _gameService.PlayerId = null;
    }

    public void UpdateGame(string session)
    {
        Session s = JsonConvert.DeserializeObject<Session>(session);
        _gameService.Session = s;

        if (_gameService.PlayerId == null)
        {
            foreach (var player in _gameService.Session.Players)
            {
                if (player.Name == _gameService.PlayerName) _gameService.PlayerId = player.Id;
            }
        }

        SetBtnText();

        IsTurn = _gameService.Session.Players[(int)_gameService.PlayerId].IsTurn && _gameService.Session.Status == Status.On;
        EnableBtns();

        if (_gameService.Session.Status == Status.Over)
        {
            string winner = "";

            Highscore h = new Highscore()
            {
                PlayerName = _gameService.PlayerName,
                EnemyName = _gameService.Session.Players.Where(p => p.Name != _gameService.PlayerName).First().Name,
                Ended = (DateTime)_gameService.Session.Ended
            };

            bool hasWinner = false;
            foreach (Player p in _gameService.Session.Players)
            {
                if (p.AssignedField == _gameService.Session.Winner)
                {
                    hasWinner = true;
                    winner = p.Name;
                    if (winner == _gameService.PlayerName) h.Status = HighscoreStatus.Win;
                    else h.Status = HighscoreStatus.Loss;
                    break;
                }
            }
            if (!hasWinner) 
            { 
                winner = "None - Draw.";
                h.Status = HighscoreStatus.Draw;
            }

            new Action( async () => await _highscoreDatabase.SaveHighscoreAsync(h))();
            MessagingCenter.Send<GameViewModel, Highscore>(this, MessengerKeys.AddHighscore, h);
            FeedbackLabel = "Game over! Winner: " + winner;
            HasEnded = true;
            return;
        }
        HasEnded = false;


        string isturn = "";
        foreach (Player p in _gameService.Session.Players)
        {
            if (p.IsTurn) isturn = p.Name;
        }

        if (isturn.EndsWith('s')) isturn += "'";
        else isturn += "'s";
        isturn += " turn.";
        FeedbackLabel = isturn;
    }

    private int[] ConvertBtnToXY(object btn)
    {
        int[] output = new int[] { 0, 0 };

        switch (Convert.ToInt32(btn))
        {
            case 1:
                output[0] = 0;
                output[1] = 1;
                break;

            case 2:
                output[0] = 0;
                output[1] = 2;
                break;

            case 3:
                output[0] = 1;
                output[1] = 0;
                break;

            case 4:
                output[0] = 1;
                output[1] = 1;
                break;

            case 5:
                output[0] = 1;
                output[1] = 2;
                break;

            case 6:
                output[0] = 2;
                output[1] = 0;
                break;

            case 7:
                output[0] = 2;
                output[1] = 1;
                break;

            case 8:
                output[0] = 2;
                output[1] = 2;
                break;

            default:
                break;
        }

        return output;
    }

    private void SetBtnText()
    {
        BtnTl = ConvertFieldToText(_gameService.Session.Board[0][0]);
        BtnTm = ConvertFieldToText(_gameService.Session.Board[0][1]);
        BtnTr = ConvertFieldToText(_gameService.Session.Board[0][2]);
        BtnMl = ConvertFieldToText(_gameService.Session.Board[1][0]);
        BtnMm = ConvertFieldToText(_gameService.Session.Board[1][1]);
        BtnMr = ConvertFieldToText(_gameService.Session.Board[1][2]);
        BtnBl = ConvertFieldToText(_gameService.Session.Board[2][0]);
        BtnBm = ConvertFieldToText(_gameService.Session.Board[2][1]);
        BtnBr = ConvertFieldToText(_gameService.Session.Board[2][2]);
    }

    private void EnableBtns()
    {
        if (IsTurn)
        {
            CanPlayTl = CanPlay(_gameService.Session.Board[0][0]);
            CanPlayTm = CanPlay(_gameService.Session.Board[0][1]);
            CanPlayTr = CanPlay(_gameService.Session.Board[0][2]);
            CanPlayMl = CanPlay(_gameService.Session.Board[1][0]);
            CanPlayMm = CanPlay(_gameService.Session.Board[1][1]);
            CanPlayMr = CanPlay(_gameService.Session.Board[1][2]);
            CanPlayBl = CanPlay(_gameService.Session.Board[2][0]);
            CanPlayBm = CanPlay(_gameService.Session.Board[2][1]);
            CanPlayBr = CanPlay(_gameService.Session.Board[2][2]);
        }
        else
        {
            CanPlayTl = false;
            CanPlayTm = false;
            CanPlayTr = false;
            CanPlayMl = false;
            CanPlayMm = false;
            CanPlayMr = false;
            CanPlayBl = false;
            CanPlayBm = false;
            CanPlayBr = false;
        }
    }

    private bool CanPlay(Field f)
    {
        if (f == Field.Empty) return true;
        else return false;
    }

    private string ConvertFieldToText(Field f)
    {
        switch (f)
        {
            case Field.X:
                return "X";

            case Field.O:
                return "O";

            default:
                return string.Empty;
        }
    }

    #region ChatTest
    //[ObservableProperty]
    //private string chat;

    //[ObservableProperty]
    //private string message;


    //[RelayCommand]
    //private async void SendMessage()
    //{
    //    await _hub.InvokeCoreAsync("SendMessage", args: new[] { Message });
    //    Message = string.Empty;
    //}

    //public void AppendChat(string message)
    //{
    //    if (Chat == null) Chat = "";

    //    Chat += $"{Environment.NewLine}Anon: {message}";
    //}
    #endregion
}
