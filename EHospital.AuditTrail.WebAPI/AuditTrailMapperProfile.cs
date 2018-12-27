using AutoMapper;
using EHospital.AuditTrail.Model;
using EHospital.Shared.AuditTrail.Models;
using Newtonsoft.Json;

namespace EHospital.AuditTrail.WebAPI
{
    /// <summary>
    /// Represents auto mapper profile for this micro-service
    /// which embed mapping configuration.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class AuditTrailMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditTrailMapperProfile"/> class.
        /// </summary>
        public AuditTrailMapperProfile()
        {
            // ActionLog map for POST request
            this.CreateMap<AuditTrailModel, ActionLog>()
                .ConvertUsing(arg => new ActionLog
            {
                ActionItem = arg.ActionItem,
                ActionType = arg.ActionType,
                Description = arg.Description,
                ItemId = arg.ItemId,
                ItemState = JsonConvert.SerializeObject(arg.ItemState),
                Module = arg.Module,
                UserId = arg.UserId
            });
        }
    }
}