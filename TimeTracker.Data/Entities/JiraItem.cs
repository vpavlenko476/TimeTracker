using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimeTracker.Data.Entities
{
    /// <summary>
    /// Задача в Jira 
    /// </summary>
    public class JiraItem
    {
        /// <summary>
        /// ИД
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Номер
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// Название
        /// </summary>
        public string Summary { get; set; }
    }

    internal class JiraItemConfiguration : IEntityTypeConfiguration<JiraItem>
    {
        public void Configure(EntityTypeBuilder<JiraItem> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Key).IsRequired();
            entity.Property(e => e.Summary).IsRequired();
        }
    }
}