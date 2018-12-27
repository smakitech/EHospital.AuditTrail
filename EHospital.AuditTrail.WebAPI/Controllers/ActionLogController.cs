using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using EHospital.AuditTrail.BusinessLogic.Contracts;
using EHospital.AuditTrail.Model;
using EHospital.Shared.AuditTrail.Models;

namespace EHospital.AuditTrail.WebAPI.Controllers
{
    /// <summary>
    /// Contains required REST request for logging actions on database object.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    public class ActionLogController : ControllerBase
    {
        /// <summary>
        /// The default root.
        /// </summary>
        private const string DEFAULT_ROOT = "api/audit-trail/";

        /// <summary>
        /// The service interface link.
        /// </summary>
        private readonly IActionLogService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionLogController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public ActionLogController(IActionLogService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Handles request [POST] : "api/audit-trail/
        /// Sends action log record to database
        /// in asynchronous mode.
        /// </summary>
        /// <param name="item"><see cref="FromBodyAttribute"/>The item.</param>
        /// <returns>
        /// Returns one of two action results.
        /// [Created] with created record and [Status Code] 201.
        /// [BadReques] with message and [Status Code] 400.
        /// </returns>
        [HttpPost(DEFAULT_ROOT)]
        public async Task<IActionResult> LogAction([FromBody] AuditTrailModel item)
        {
            try
            {
                var result = await this.service.InsertRecordAsync(Mapper.Map<ActionLog>(item));
                return this.Created(DEFAULT_ROOT, result);
            }
            catch (FormatException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles request [GET] : "api/audit-trail/filter?name={...}&id={...}"
        /// Retrieves action log records set associated with action item
        /// specified by name and identifier.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="name"><see cref="FromQueryAttribute"/> The action item name.</param>
        /// <param name="id"><see cref="FromQueryAttribute"/> The action item identifier.</param>
        /// <returns>
        /// Returns one of two action results.
        /// [Ok] with set of records and [Status Code] 200.
        /// [NoContent] with [Status Code] 204.
        /// </returns>
        [HttpGet(DEFAULT_ROOT + "filter")]
        public async Task<IActionResult> GetConcreteItemActions(
            [FromQuery] string name, [FromQuery] int id)
        {
            var result = await this.service.GetActionItemRecordsAsync(name, id);
            if (result.Any())
            {
                return this.Ok(result);
            }

            return this.NoContent();
        }
    }
}