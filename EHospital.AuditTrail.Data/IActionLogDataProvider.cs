using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EHospital.AuditTrail.Model;

namespace EHospital.AuditTrail.Data
{
    /// <summary>
    /// AuditTrail DataProvider interface.
    /// </summary>
    public interface IActionLogDataProvider
    {
        /// <summary>
        /// Gets or sets the records of ActionsLog table.
        /// Represents set of records which store in the database.
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