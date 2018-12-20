using Microsoft.EntityFrameworkCore;
using EHospital.AuditTrail.Model;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EHospital.AuditTrail.Data
{
    /// <summary>
    /// Represents database context for audit trail.
    /// Provide access to audit trail tables and data.
    /// </summary>
    /// <seealso cref="DbContext"/>
    public class AuditTrailContext : DbContext, IActionLogDataProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditTrailContext"/> class.
        /// </summary>
        public AuditTrailContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditTrailContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public AuditTrailContext(DbContextOptions<AuditTrailContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the records of ActionsLog table.
        /// Represents set of records which store in the database.
        /// Helps to interact with ActionsLog table placed in the database.
        /// </summary>
        public DbSet<ActionLog> ActionsLog { get; set; }

        /// <summary>
        /// Saves the changes to database asynchronous.
        /// </summary>
        /// <returns>
        /// Task object.
        /// </returns>
        public async Task SaveAsync()
        {
            await this.SaveChangesAsync();
        }

        /// <summary>
        /// Defines configuration of the connection to database.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this.Database.GetDbConnection().ConnectionString);
            }
        }

        /// <summary>
        /// Maps classes of models on database tables using Fluent API.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionLog>(entity =>
            {
                entity.Property(e => e.ActionItem).IsUnicode(false);

                entity.Property(e => e.ActionTime).HasDefaultValueSql("([dbo].[GETCURRENTDATE]())");

                entity.Property(e => e.Module).IsUnicode(false);
            });
        }
    }
}