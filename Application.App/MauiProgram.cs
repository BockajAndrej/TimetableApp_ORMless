using application.App.Pages.View;
using application.App.Pages.ViewModel;
using application.BL.RAW.Facades;
using application.DAL.RAW.Entities;
using application.DAL.RAW.UnitOfWork;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace Application.App
{
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<MainPageView>();

            builder.Services.AddTransient<MainPageViewModel>();

            builder.Services.AddTransient<EmployeeFacade>();
            builder.Services.AddTransient<CityFacade>();
            builder.Services.AddTransient<CpFacade>();
            builder.Services.AddTransient<TransportFacade>();
            builder.Services.AddTransient<VehicleFacade>();

            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=applicationDb;Trusted_Connection=True;";
            builder.Services.AddSingleton(connectionString);

            builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
