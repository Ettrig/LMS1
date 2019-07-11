using LMS1.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LMS1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();

            IWebHost webHost = CreateWebHostBuilder(args).Build();

            // This code is copied from method Main in Program.cs in LexiconGym
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                var config = webHost.Services.GetRequiredService<IConfiguration>();

                using (var innerScope = webHost.Services.CreateScope())
                {
                    var innerServices = innerScope.ServiceProvider;
                    SeedData.Initialize(innerServices); // TODO CJA: check if this may cause unsynched database and corrupt DB.
                }

                //Behöver sättas via komandotolken i projektkatalogen.
                // dotnet user-secrets set "Gym:AdminPW" "FooBar77!"
                //Läser in lösenordet
                var adminPW = config["Gym:AdminPW"];
                try
                {
                    SeedData.InitializeRoleManagement(services, adminPW).Wait();
                }
                catch (Exception ex)
                {
                    //Om seeden inte går som tänkt logga vad som gått fel
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex.Message, "Seed fail");
                }

            }

            //using (var scope = webHost.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    SeedData.Initialize(services); // TODO: CJA check if unsynched database
            //}

            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
