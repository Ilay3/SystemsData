using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemsData.Data;
using SystemsData.Views;
using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace SystemsData
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public static int CurrentEmployeeId { get; set; } = 0;

        protected override void OnStartup(StartupEventArgs e)
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Регистрируем ApplicationDbContext
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql("Host=localhost;Port=5432;Database=systemsdata;Username=postgres;Password=postgres"));

                    // Регистрируем окна приложения
                    services.AddTransient<AuthorizationWindow>();
                    services.AddTransient<MainWindow>();
                    services.AddTransient<AcceptanceView>();

                })
                .Build();

            using (var scope = AppHost.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            // Запускаем окно авторизации через DI
            var authWindow = AppHost.Services.GetRequiredService<AuthorizationWindow>();
            authWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (AppHost != null)
            {
                await AppHost.StopAsync(TimeSpan.FromSeconds(5));
                AppHost.Dispose();
            }
            base.OnExit(e);
        }
    }
}
