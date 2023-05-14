using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TimeTracker.Data;

namespace TimeTrackerData.ContextFactory
{
    
    /*
     dotnet-ef  migrations add "InitialCreate" --context TimeTrackerContext --project C:\Users\Vladislav\RiderProjects\TimeTracker\TimeTracker.Data\TimeTracker.Data.csproj --startup-project C:\Users\Vladislav\RiderProjects\TimeTracker\TimeTrackerData.ContextFactory\TimeTrackerData.ContextFactory.csproj
     dotnet ef database update --context DataContext --project Cid.Data.csproj --startup-project Cid.Data.Context.ConsoleRunner.csproj
     */
    public class DataContextDbContextFactory : IDesignTimeDbContextFactory<TimeTrackerContext>
    {
        public TimeTrackerContext CreateDbContext(string[] args)
        {
            var settingsConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<TimeTrackerContext>().UseNpgsql(settingsConfiguration["POSTGRES_CONNECTION_STRING"]);
            var dbContext = new TimeTrackerContext(builder.Options);
            return dbContext;
        }
    }
}