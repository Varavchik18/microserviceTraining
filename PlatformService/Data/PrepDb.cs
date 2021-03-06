using PlatformService.models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                System.Console.WriteLine(" -- > Seeding data ... ");

                context.Platforms.AddRange(
                    new Platform()
                    {
                        Name = "Dotnet",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },

                    new Platform()
                    {
                        Name = "Kubernetes",
                        Publisher = "Cloud Native computing Foundation",
                        Cost = "Free"
                    },

                    new Platform()
                    {
                        Name = "SQL server express",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    }
                );

                context.SaveChanges();
            }
            else
            {
                System.Console.WriteLine(" -- > we already have data");
            }
        }
    }
}