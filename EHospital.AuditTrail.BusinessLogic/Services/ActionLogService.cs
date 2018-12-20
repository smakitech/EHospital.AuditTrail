using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IQueryable<ActionLog>> GetItemId(int id)
        {
            return await Task.Run(() => this.provider.ActionsLog
                .Where(a => a.ItemId == id));
        }

        public async Task<ActionLog> InsertRecord(ActionLog item)
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
                    .DeserializeXNode(item.ItemState, nameof(item.ActionItem));
                item.ItemState = document.ToString();
            }

            this.provider.ActionsLog.Add(item);
            await this.provider.SaveAsync();
            return item;
        }
    }
}
