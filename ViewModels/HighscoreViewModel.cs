using MauiTicTacToeHandin.Models;
using MauiTicTacToeHandin.Services;

namespace MauiTicTacToeHandin.ViewModels;

public partial class HighscoreViewModel : BaseViewModel
{
    private GameService _gameService;
    private HighscoreDatabase _database;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private int wins;

    [ObservableProperty]
    private int losses;

    [ObservableProperty]
    private int draws;

    [ObservableProperty]
    private List<Highscore> gameHistory = new List<Highscore>();

    public HighscoreViewModel(HighscoreDatabase database, GameService gameService)
    {
        _database = database;
        new Action(async () => await InitFromDB())();
        _gameService = gameService;
        Name = _gameService.PlayerName;
    }

    async Task InitFromDB()
    {
        (await _database.GetHighscoresAsync()).ForEach(this.gameHistory.Add);
        foreach (Highscore h in gameHistory)
        {
            AddToCount(h.Status);
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
