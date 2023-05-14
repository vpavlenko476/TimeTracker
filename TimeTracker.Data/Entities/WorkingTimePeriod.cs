using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimeTracker.Data.Entities
{
    /// <summary>
    /// Временной промежуток работы над задачей
    /// </summary>
    public class WorkingTimePeriod
    {
        /// <summary>
        /// ИД
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ИД задачи
        /// </summary>
        public Guid JiraItemId { get; set; }

        /// <summary>
        /// Навигационное свойство для задачи в jira
        /// </summary>
        public JiraItem JiraItem { get; set; }

        /// <summary>
        /// Начало 
        /// </summary>
        public DateTimeOffset Begin { get; set; }

        /// <summary>
        /// Окончание
        /// </summary>
        public DateTimeOffset End { get; set; }

        /// <summary>
        /// Признак завершенного промежутка
        /// </summary>
        public bool IsClosed { get; set; }
    }

    internal class WorkingTimePeriodConfiguration : IEntityTypeConfiguration<WorkingTimePeriod>
    {
        public void Configure(EntityTypeBuilder<WorkingTimePeriod> entity)
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.JiraItem).WithMany().HasForeignKey(e => e.JiraItemId).IsRequired();
            entity.Property(e => e.Begin).IsRequired();
            entity.Property(e => e.End).IsRequired();
            entity.Property(e => e.IsClosed).IsRequired();
        }
    }
}