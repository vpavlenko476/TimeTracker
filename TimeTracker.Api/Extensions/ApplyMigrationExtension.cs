using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TimeTracker.Api.Extensions
{
    public static class ApplyMigrationExtension
    {
        public static void EnsureMigrate<T>(this IApplicationBuilder app) where T : DbContext
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<T>();
            context.Database.Migrate();
        }
    } 
}