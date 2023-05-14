using Microsoft.EntityFrameworkCore;
using TimeTracker.Data.Entities;

namespace TimeTracker.Data
{
    public class TimeTrackerContext : DbContext
    {
        public TimeTrackerContext(DbContextOptions<TimeTrackerContext> options) : base(options) { }
        
        /// <summary>
        /// Список задач
        /// </summary>
        public DbSet<JiraItem> JiraItems { get; set; }
        
        /// <summary>
        /// Времена работы над задачей
        /// </summary>
        public DbSet<WorkingTimePeriod> WorkingTimePeriods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new JiraItemConfiguration());
            modelBuilder.ApplyConfiguration(new WorkingTimePeriodConfiguration());
        }
    }
}