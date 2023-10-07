using AV00_Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AV00_Control_Application.Models.Configuration
{
    internal class LogMessageConfiguration : IEntityTypeConfiguration<LogMessage>
    {
        public void Configure(EntityTypeBuilder<LogMessage> builder)
        {
            builder.ToTable("LogMessages");

            builder.Property(x => x.TimeStamp);
            builder.Property(x => x.Message).IsRequired();
            builder.Property(x => x.LogType).IsRequired();
            builder.Property(x => x.ServiceName).IsRequired();
            builder.HasKey(x => x.Id);
        }
    }
}
