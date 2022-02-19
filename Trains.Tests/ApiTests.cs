using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Trains.Api.Controllers;
using Trains.Api.Models;
using Trains.Api.Services;

namespace Trains.Tests
{
    public class ApiTests
    {
        private TrainsController _controller;
        private Mock<IScheduleService> _mockService;
        private const string TEST_TRAIN_ID = "ABCD";

        [SetUp]
        public void Init()
        {
            _mockService = new Mock<IScheduleService>();
        }

        #region GetScheduleTests
        [Test]
        public void InvalidInput()
        {
            _controller = new TrainsController(_mockService.Object);
            var response = _controller.GetSchedule(string.Empty);

            var objResponse = response as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objResponse.StatusCode);
            Assert.AreEqual("Invalid train id provided", objResponse.Value);
            
        }

        [Test]
        public void ScheduleNotFound()
        {
            _mockService.Setup(mock => mock.GetSchedule(It.IsAny<string>())).Returns((List<string>)null);
            _controller = new TrainsController(_mockService.Object);

            var response = _controller.GetSchedule(TEST_TRAIN_ID);
            var objResponse = response as ObjectResult;

            Assert.AreEqual((int)HttpStatusCode.NotFound, objResponse.StatusCode);
            Assert.AreEqual($"Train id {TEST_TRAIN_ID} not found", objResponse.Value);
        }

        [Test]
        // Happy path test
        public void GetSchedule()
        {
            _mockService.Setup(mock => mock.GetSchedule(It.IsAny<string>())).Returns(new List<string>());
            _controller = new TrainsController(_mockService.Object);
            var response = _controller.GetSchedule(TEST_TRAIN_ID);

            var objResponse = response as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.OK, objResponse.StatusCode);

        }
        #endregion

        #region AddScheduleTests
        [Test]
        public void HandleArgumentExceptions()
        {
            _mockService.Setup(mock => mock.CreateSchedule(It.IsAny<string>(), It.IsAny<List<string>>())).Throws<ArgumentException>();
            _controller = new TrainsController(_mockService.Object);

            var response = _controller.AddSchedule(new TrainSchedule());
            var objResponse = response as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objResponse.StatusCode);
        }

        [Test]
        // Happy path test
        public void AddSchedule()
        {
            _mockService.Setup(mock => mock.CreateSchedule(It.IsAny<string>(), It.IsAny<List<string>>()));
            _controller = new TrainsController(_mockService.Object);
            var response = _controller.AddSchedule(new TrainSchedule()
            {
                TrainId = TEST_TRAIN_ID,
                Schedules = new List<string>()
            });

            var objResponse = response as StatusCodeResult;
            Assert.AreEqual((int)HttpStatusCode.Created, objResponse.StatusCode);

        }
        #endregion

        #region GetNextTests
        [Test]
        public void InvalidTime()
        {
            _controller = new TrainsController(_mockService.Object);
            var response = _controller.GetNext(string.Empty);

            var objResponse = response as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objResponse.StatusCode);
            Assert.AreEqual("Invalid time provided", objResponse.Value);
        }

        [Test]
        public void InvalidTimeFormat()
        {
            _controller = new TrainsController(_mockService.Object);
            var response = _controller.GetNext("abcd");

            var objResponse = response as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objResponse.StatusCode);
            Assert.AreEqual("Please ensure time is in the HH:MM format. Exxample 9:23 or 12:03", objResponse.Value);
        }

        [Test]
        public void HandlesException()
        {
            _mockService.Setup(mock => mock.GetNext(It.IsAny<string>())).Throws<ArgumentException>();
            _controller = new TrainsController(_mockService.Object);

            var response = _controller.GetNext("12:94");
            var objResponse = response as ObjectResult;

            Assert.AreEqual((int)HttpStatusCode.BadRequest, objResponse.StatusCode);            
        }

        [Test]
        // Happy path tests
        public void GetNextTest()
        {
            string retValue = "01:00";
            _mockService.Setup(mock => mock.GetNext(It.IsAny<string>())).Returns(retValue);
            _controller = new TrainsController(_mockService.Object);

            var response = _controller.GetNext("12:34");
            var objResponse = response as ObjectResult;

            Assert.AreEqual((int)HttpStatusCode.OK, objResponse.StatusCode);
            Assert.AreEqual(retValue, objResponse.Value);
        }
        #endregion
    }
}
