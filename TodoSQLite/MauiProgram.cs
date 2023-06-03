using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TodoSQLite.Data;
using TodoSQLite.Views;

namespace TodoSQLite;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<TodoListPage>();
		builder.Services.AddTransient<TodoItemPage>();
        builder.Services.AddScoped<IDbRepository, DbRepository>();
        builder.Services.AddDbContext<DataContext>(options =>
        {
            options
                .UseNpgsql("Server=localhost;Port=5432;Database=Todo; User Id=postgres;Password=1234");
        });

        return builder.Build();
	}
}
