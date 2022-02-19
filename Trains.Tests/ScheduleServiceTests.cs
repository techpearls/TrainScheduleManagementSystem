using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Trains.Api.Models;
using Trains.Api.Repository;
using Trains.Api.Services;

namespace Trains.Tests
{
    public class ScheduleServiceTests
    {
        private Mock<IDatabase> _mockDb = new Mock<IDatabase>();
        private IScheduleService _service;
        private const string TEST_TRAIN_ID = "ABCD";

        #region CreateScheduleTests
        [Test]
        public void HandlesException()
        {
            _mockDb.Setup(mock => mock.Get(It.IsAny<string>())).Returns(new List<string>());
            _service = new ScheduleService(_mockDb.Object);

            var ex = Assert.Throws<ArgumentException>(() => _service.CreateSchedule(TEST_TRAIN_ID, new List<string>()));
            Assert.That(ex.Message, Is.EqualTo($"Schedule already exists for train {TEST_TRAIN_ID}"));
        }

        [Test]
        public void InvalidTimesInput()
        {
            _mockDb.Setup(mock => mock.Get(It.IsAny<string>())).Returns((List<string>)null);
            _service = new ScheduleService(_mockDb.Object);

            List<string> timeValues = new() { "12:94", "ab:34", ",,,,l", "1:40" }; // One value is valid to help verify count
            var expectedStringMessage = string.Join(',', timeValues.Take(timeValues.Count - 1));

            var ex = Assert.Throws<ArgumentException>(() => _service.CreateSchedule(TEST_TRAIN_ID, timeValues));
            Assert.That(ex.Message, Is.EqualTo($"Please make sure times passed in are valid. Invalid values: {expectedStringMessage}"));
        }

        [Test]
        // Happy path test
        public void CreateScheduleTest()
        {
            _mockDb.Setup(mock => mock.Get(It.IsAny<string>())).Returns((List<string>)null);
            _service = new ScheduleService(_mockDb.Object);

            List<string> timeValues = new() { "0:34", "9:9", "13:40" };
            _service.CreateSchedule(TEST_TRAIN_ID, timeValues);

            List<string> sanitizedTimes = new() { "00:34", "09:09", "13:40" };
            List<int> indices = new() { 34, 549, 820 };

            _mockDb.Verify(db => db.Set(TEST_TRAIN_ID, sanitizedTimes), Times.Once());

            _mockDb.Verify(db => db.UpdateScheduleCounts(indices), Times.Once());
        }
        #endregion

        #region GetNextTests
        [Test]
        public void HandlesException_InvalidTime()
        {
            _service = new ScheduleService(_mockDb.Object);
            var ex = Assert.Throws<ArgumentException>(() => _service.GetNext("abcd"));
            Assert.That(ex.Message, Is.EqualTo("Please make sure time passed in is valid"));
        }

        [Test]
        public void NoTimeFound()
        {
            _mockDb.Setup(m => m.GetNext(It.IsAny<int>())).Returns(-1);
            _service = new ScheduleService(_mockDb.Object);
            var result = _service.GetNext("1:30");
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        // Happy path test
        public void GetsNextTest()
        {
            _mockDb.Setup(m => m.GetNext(It.IsAny<int>())).Returns(100);
            _service = new ScheduleService(_mockDb.Object);
            var result = _service.GetNext("1:15");
            Assert.AreEqual("01:40", result);
        }
        #endregion
    }
}
