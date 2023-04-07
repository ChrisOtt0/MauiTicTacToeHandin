using MauiTicTacToeHandin.Models;
using MauiTicTacToeHandin.Services;
using MauiTicTacToeHandin.Tools;

namespace MauiTicTacToeHandin.ViewModels;

public partial class HighscoreViewModel : BaseViewModel, INotifyPropertyChanged
{
    private GameService _gameService;
    private HighscoreDatabase _database;
    private Highscore highscore;
    private List<Highscore> gameHistory;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private int wins;

    [ObservableProperty]
    private int losses;

    [ObservableProperty]
    private int draws;

    [ObservableProperty]
    private ObservableCollection<string> gameSummaries = new ObservableCollection<string>();

    public Highscore Highscore
    {
        get => highscore;
        set
        {
            highscore = value;
            if (value != null)
            {
                gameHistory.Add(highscore);
                GameSummaries.Add(
                " " + highscore.PlayerName + " vs. " + highscore.EnemyName +
                " | Status: " + highscore.Status.ToString() +
                " | DateTime: " + highscore.Ended);
                AddToCount(highscore.Status);
            }
        }
    }

    public HighscoreViewModel(HighscoreDatabase database, GameService gameService)
    {
        _database = database;
        new Action(async () => await InitFromDB())();
        _gameService = gameService;
        Name = _gameService.PlayerName;

        MessagingCenter.Subscribe<GameViewModel, Highscore>(this, MessengerKeys.AddHighscore, async (sender, arg) =>
        {
            gameHistory.Add(arg);
            GameSummaries.Add(
                " " + arg.PlayerName + " vs. " + arg.EnemyName +
                " | Status: " + arg.Status.ToString() +
                " | DateTime: " + arg.Ended);
            AddToCount(arg.Status);
        });
    }

    async Task InitFromDB()
    {
        gameHistory = new List<Highscore>(await _database.GetHighscoresAsync());
        foreach (Highscore h in gameHistory)
        {
            AddToCount(h.Status);
            GameSummaries.Add(
                " " + h.PlayerName + " vs. " + h.EnemyName + 
                " | Status: " + h.Status.ToString() + 
                " | DateTime: " + h.Ended);
        }
    }

    void AddToCount(HighscoreStatus status)
    {
        switch (status)
        {
            case HighscoreStatus.Win:
                Wins++;
                break;

            case HighscoreStatus.Loss:
                Losses++; 
                break;

            case HighscoreStatus.Draw:
                Draws++;
                break;
        }
    }
}
