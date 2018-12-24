using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using EHospital.AuditTrail.BusinessLogic.Contracts;
using EHospital.AuditTrail.Data;
using EHospital.AuditTrail.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EHospital.AuditTrail.BusinessLogic.Services
{
    /// <summary>
    /// Service which tracks action on database entities
    /// and saves information about occurred actions to database.
    /// </summary>
    public class ActionLogService : IActionLogService
    {
        /// <summary>
        /// Provides access to required database objects
        /// for this service needs.
        /// </summary>
        private readonly IActionLogDataProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionLogService"/> class.
        /// </summary>
        /// <param name="provider">Data provider.</param>
        public ActionLogService(IActionLogDataProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Gets the records from database table ActionsLog
        /// by name and identifier of ActionItem
        /// in asynchronous mode.
        /// </summary>
        /// <param name="name">The action item name.</param>
        /// <param name="id">The action item identifier.</param>
        /// <returns>Set of records by specified action item name and id.</returns>
        public async Task<IQueryable<ActionLog>> GetActionItemRecordsAsync(string name, int id)
        {
            // Search independent from register
            var result = await Task.Run(() => this.provider.ActionsLog
                .Where(a => a.ActionItem.ToLower() == name.ToLower() && a.ItemId == id));

            return result;
        }

        /// <summary>
        /// Inserts the record to database table ActionsLog.
        /// in asynchronous mode.
        /// </summary>
        /// <param name="item">
        /// The item contains properties, which describe the action occurred.
        /// </param>
        /// <returns>Record has been added to database.</returns>
        /// <exception cref="FormatException">
        /// Invalid JSON content can't be converted to XML.
        /// </exception>
        public async Task<ActionLog> InsertRecordAsync(ActionLog item)
        {
            // Get state of action item in JSON format.
            // Check JSON valid or not.
            // Convert JSON string to XML string.
            if (item.ItemState != null)
            {
                try
                {
                    JObject.Parse(item.ItemState);
                }
                catch (JsonReaderException ex)
                {
                    string message = "Invalid JSON content can't be converted to XML.";
                    throw new FormatException(message, ex);
                }

                XDocument document = JsonConvert
                    .DeserializeXNode(item.ItemState, item.ActionItem);
                item.ItemState = document.ToString();
            }

            this.provider.ActionsLog.Add(item);
            await this.provider.SaveAsync();
            return item;
        }
    }
}
