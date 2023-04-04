using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }

        }
        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"--> Could not start migration: {ex.Message}");
                }
            }
            if (!context.Platforms.Any())
            {
                Console.WriteLine($"Seeding Data... isProd:${isProd}");
                context.Platforms.AddRange(
                    new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "SQL Srever Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );
                context.SaveChanges();
            }
        }
    }
}