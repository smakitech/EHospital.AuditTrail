using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using EHospital.AuditTrail.BusinessLogic.Contracts;
using EHospital.AuditTrail.Model;
using EHospital.AuditTrail.WebAPI.Views;

namespace EHospital.AuditTrail.WebAPI.Controllers
{
    [ApiController]
    public class ActionLogController : ControllerBase
    {
        private const string DEFAULT_ROOT = "api/audit-trail/";
        private readonly IActionLogService service;

        public ActionLogController(IActionLogService service)
        {
            this.service = service;
        }

        [HttpPost(DEFAULT_ROOT)]
        public async Task<IActionResult> LogAction([FromBody] ActionLogRequest item)
        {
            try
            {
                var result = await this.service.InsertRecord(Mapper.Map<ActionLog>(item));
                return this.Created(DEFAULT_ROOT, result);
            }
            catch (FormatException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet(DEFAULT_ROOT + "filter")]
        public async Task<IActionResult> GetConcreteItemActions(
            [FromQuery] string itemName, [FromQuery] int id)
        {
            var result = await this.service.GetItemId(itemName, id);
            return this.Ok(result);
        }
    }
}