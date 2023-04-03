namespace MauiTicTacToeHandin;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
				fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
				fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<NameViewModel>();

		builder.Services.AddSingleton<NamePage>();

		builder.Services.AddSingleton<GameViewModel>();

		builder.Services.AddSingleton<GamePage>();

		builder.Services.AddSingleton<HighscoreViewModel>();

		builder.Services.AddSingleton<HighscorePage>();

		return builder.Build();
	}
}
