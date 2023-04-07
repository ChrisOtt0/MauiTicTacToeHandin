using MauiTicTacToeHandin.Models;
using MauiTicTacToeHandin.Services;

namespace MauiTicTacToeHandin.ViewModels;

public partial class NameViewModel : BaseViewModel
{
    NameDatabase _database;
    GameService _gameService;

    private NameSave ns = null;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string feedback;

    public NameViewModel(NameDatabase database, GameService gameService)
    {
        _database = database;
        new Action(async () => await InitFromDB())();

        _gameService = gameService;
    }

    async Task InitFromDB()
    {
        ns = await _database.GetNameSave(1);
        if (ns != null)
        {
            Name = ns.Name;
            _gameService.PlayerName = ns.Name;
        }
        else
            Name = string.Empty;
    }

    [RelayCommand]
    private async void SaveNameSave()
    {
        var result = await _database.SaveNameSave(new NameSave { ID = 1, Name = Name });
        if (result != 0)
        {
            Feedback = "Name saved.";
            _gameService.PlayerName = Name;
        }
        else
            Feedback = "Issue with the Database. Name not saved.";
    }
}
