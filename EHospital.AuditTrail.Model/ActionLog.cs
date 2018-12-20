using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHospital.AuditTrail.Model
{
    /// <summary>
    /// Represents model which maps on EHospital database table ActionsLog.
    /// Stores information about operations performed on database entities.
    /// </summary>
    public class ActionLog
    {
        /// <summary>Gets or sets the record identifier.</summary>
        [Key]
        public long Id { get; set; }

        /// <summary>Gets or sets the action time.</summary>
        [Column(TypeName = "datetime")]
        [Required]
        public DateTime ActionTime { get; set; }

        /// <summary>Gets or sets the user identifier.</summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of module in which action has occurred.
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Module { get; set; }

        /// <summary>Gets or sets the type of the action (operation).</summary>
        [Required]
        public ActionMode ActionType { get; set; }

        /// <summary>
        /// Gets or sets the name of entity has been affected.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ActionItem { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the record affected in the entity.</summary>
        [Required]
        public int ItemId { get; set; }

        /// <summary>Gets or sets the state of the item.</summary>
        [Column(TypeName = "xml")]
        public string ItemState { get; set; }

        /// <summary>Gets or sets the description.</summary>
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
    }
}