using Microsoft.EntityFrameworkCore;
using EHospital.AuditTrail.Model;
using System.Threading.Tasks;

namespace EHospital.AuditTrail.Data
{
    /// <summary>
    /// AuditTrail DataProvider interface.
    /// </summary>
    public interface IAuditTrailDataProvider
    {
        /// <summary>
        /// Gets or sets the actions log records.
        /// Represents set of actions log records which store in the database.
        /// Helps to interact with ActionsLog table placed in the database.
        /// </summary>
        DbSet<ActionLog> ActionsLog { get; set; }

        /// <summary>
        /// Saves the changes to database asynchronous.
        /// </summary>
        /// <returns>Task object.</returns>
        Task SaveAsync();
    }
}