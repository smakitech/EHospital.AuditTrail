using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Moq;
using EHospital.AuditTrail.BusinessLogic.Services;
using EHospital.AuditTrail.Data;
using EHospital.AuditTrail.Model;

namespace EHospital.AuditTrail.Tests
{
    [TestClass]
    public class ActionLogServiceTests
    {
        /// <summary>
        /// The mock for data provider.
        /// </summary>
        private Mock<IActionLogDataProvider> provider;

        /// <summary>
        /// The mock for database set.
        /// </summary>
        private Mock<DbSet<ActionLog>> mockDbSet;

        /// <summary>
        /// The test list for database set mocking.
        /// </summary>
        private IQueryable<ActionLog> testList;

        /// <summary>
        /// The service under testing.
        /// </summary>
        private ActionLogService service;

        /// <summary>
        /// Initializes required items before run of every test method.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.testList = this.GetRecords();
            this.mockDbSet = new Mock<DbSet<ActionLog>>();
            this.mockDbSet.As<IQueryable<ActionLog>>()
                .Setup(m => m.Provider)
                .Returns(this.testList.Provider);
            this.mockDbSet.As<IQueryable<ActionLog>>()
                .Setup(m => m.Expression)
                .Returns(this.testList.Expression);
            this.mockDbSet.As<IQueryable<ActionLog>>()
                .Setup(m => m.ElementType)
                .Returns(this.testList.ElementType);
            this.mockDbSet.As<IQueryable<ActionLog>>()
                .Setup(m => m.GetEnumerator())
                .Returns(this.testList.GetEnumerator);
            this.provider = new Mock<IActionLogDataProvider>();
            this.provider
                .Setup(m => m.ActionsLog)
                .Returns(this.mockDbSet.Object);
        }

        /// <summary>
        /// Check whether method under test InsertRecordAsync logs action correctly
        /// when ItemState property in passing model equals null.
        /// </summary>
        [TestMethod]
        public void InsertRecordAsync_SuccessInsertsRecordWithNullItemState()
        {
            // Arrange
            this.service = new ActionLogService(this.provider.Object);
            int newRecordId = (int)this.testList.Max(t => t.Id) + 1;
            ActionLog record = this.testList.Where(t => t.ItemState == null).FirstOrDefault();
            record.Id = newRecordId;
            ActionLog result;

            // Act
            result = this.service.InsertRecordAsync(record).Result;

            // Assert
            this.mockDbSet.Verify(m => m.Add(It.IsAny<ActionLog>()), Times.Once);
            this.provider.Verify(m => m.SaveAsync(), Times.Once);
            Assert.AreEqual(record.Id, result.Id);
        }

        /// <summary>
        /// Check whether method under test InsertRecordAsync logs action correctly
        /// when ItemState property in passing model contains valid JSON string
        /// of serialized model.
        /// </summary>
        [TestMethod]
        public void InsertRecordAsync_SuccessInsertsRecordWithItemState()
        {
            // Arrange
            this.service = new ActionLogService(this.provider.Object);
            int newRecordId = (int)this.testList.Max(t => t.Id) + 1;
            ActionLog record = this.testList.Where(t => t.ItemState != null).FirstOrDefault();
            record.Id = newRecordId;
            ActionLog result;

            // Act
            result = this.service.InsertRecordAsync(record).Result;

            // Assert
            // TODO: Check amount and identity Id or not?
            this.mockDbSet.Verify(m => m.Add(It.IsAny<ActionLog>()), Times.Once);
            this.provider.Verify(m => m.SaveAsync(), Times.Once);
            Assert.AreEqual(record.Id, result.Id);
        }

        /// <summary>
        /// Check whether method under test InsertRecordAsync throws <see cref="FormatException"/>
        /// logging action when ItemState property in passing model contains invalid JSON string
        /// of serialized model.
        /// </summary>
        /// <param name="itemState">State of the item.</param>
        /// <returns>Task object.</returns>
        [TestMethod]
        [DataRow("invalid json string")]
        [DataRow("")]
        [ExpectedException(typeof(FormatException))]
        public async Task InsertRecordAsync_ThrowsFormatExceptionWhenItemStateInvalidJSON(string itemState)
        {
            // Arrange
            this.service = new ActionLogService(this.provider.Object);
            int newRecordId = (int)this.testList.Max(t => t.Id) + 1;
            ActionLog record = this.testList.Where(t => t.ItemState != null).FirstOrDefault();
            record.Id = newRecordId;
            record.ItemState = itemState;
            ActionLog result;

            // Act
            result = await this.service.InsertRecordAsync(record);
        }

        /// <summary>
        /// Checks whether method under testing GetActionItemRecordsAsync
        /// returns set of records associated with item
        /// specified by name and id correctly.
        /// </summary>
        /// <param name="name">The name of action item.</param>
        /// <param name="id">The identifier of action.</param>
        [TestMethod]
        [DataRow("Drug", 1)]
        [DataRow("drug", 1)]
        public void GetActionItemRecordsAsync_ReturnsCorrectSetOfRecords(string name, int id)
        {
            // Arrange
            this.service = new ActionLogService(this.provider.Object);
            int expectedAmount = this.testList.Count();
            int actualAmount;

            // Act
            actualAmount = this.service.GetActionItemRecordsAsync(name, id).Result.Count();

            // Assert
            Assert.AreEqual(expectedAmount, actualAmount);

        }

        /// <summary>
        /// Returns list of fake <see cref="ActionLog"/> records for testing.
        /// </summary>
        /// <returns>List of <see cref="ActionLog"/> records.</returns>
        private IQueryable<ActionLog> GetRecords()
        {
            // Action item for testing.
            Drug drug = new Drug()
            {
                Id = 1,
                Name = "Aspirin",
                ProduceDate = new DateTime(2018, 1, 1)
            };
            List<ActionLog> records = new List<ActionLog>()
            {
                // Record without item state
                new ActionLog()
                {
                    Id = 1,
                    ActionTime = new DateTime(2018, 1, 1),
                    Module = "MedicationAPI",
                    UserId = 1,
                    ActionType = ActionMode.Create,
                    ActionItem = nameof(Drug),
                    ItemId = 1,
                    ItemState = null,
                    Description = "Drug created."
                },

                // Record with ItemState
                new ActionLog()
                {
                    Id = 2,
                    ActionTime = new DateTime(2018, 1, 2),
                    Module = "MedicationAPI",
                    UserId = 1,
                    ActionType = ActionMode.Create,
                    ActionItem = nameof(Drug),
                    ItemId = 1,
                    ItemState = JsonConvert.SerializeObject(drug),
                    Description = "Drug created."
                },
            };

            return records.AsQueryable();
        }
    }
}