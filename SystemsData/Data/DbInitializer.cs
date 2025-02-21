using Microsoft.EntityFrameworkCore;

namespace SystemsData.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Применяем миграции и создаём базу, если она ещё не создана
            context.Database.Migrate();
        }
    }
}
