using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Trains.Api.Controllers;
using Trains.Api.Models;
using Trains.Api.Repository;
using Trains.Api.Services;

namespace Trains.Tests
{
    public class DatabaseTests
    {
        private Database _db;

        [SetUp]
        public void Init()
        {
            _db = new Database();
        }

        #region GetNextTests
        [Test]
        public void GetNextTests()
        {
            int length = Database.ScheduleCounts.Length;
            Database.ScheduleCounts[0] = 3;
            Database.ScheduleCounts[length - 1] = 1;
            Database.ScheduleCounts[250] = 1;
            Database.ScheduleCounts[400] = 5;

            int index = _db.GetNext(300);
            Assert.AreEqual(400, index);

            // wrap around test
            index = _db.GetNext(1000);
            Assert.AreEqual(0, index);
        }

        
        #endregion
    }
}
