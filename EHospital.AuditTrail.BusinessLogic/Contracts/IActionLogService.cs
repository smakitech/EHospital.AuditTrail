using System.Linq;
using System.Threading.Tasks;
using EHospital.AuditTrail.Model;

namespace EHospital.AuditTrail.BusinessLogic.Contracts
{
    /// <summary>
    /// Represents interface of action log service.
    /// </summary>
    public interface IActionLogService
    {
        /// <summary>
        /// Inserts the record to database table ActionsLog.
        /// in asynchronous mode.
        /// </summary>
        /// <param name="item">
        /// The item contains properties, which describe the action occurred.
        /// </param>
        /// <returns>Record has been added to database.</returns>
        Task<ActionLog> InsertRecordAsync(ActionLog item);

        /// <summary>
        /// Gets the records from database table ActionsLog
        /// by name and identifier of ActionItem
        /// in asynchronous mode.
        /// </summary>
        /// <param name="name">The action item name.</param>
        /// <param name="id">The action item identifier.</param>
        /// <returns>Set of records by specified action item name and id.</returns>
        Task<IQueryable<ActionLog>> GetActionItemRecordsAsync(string name, int id);
    }
}
