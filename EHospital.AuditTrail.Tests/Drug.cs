using System;

namespace EHospital.AuditTrail.Tests
{
    /// <summary>
    /// Represent model of drug.
    /// Using for testing.
    /// </summary>
    public class Drug
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets when drug has been produced.
        /// </summary>
        public DateTime ProduceDate { get; set; }
    }
}