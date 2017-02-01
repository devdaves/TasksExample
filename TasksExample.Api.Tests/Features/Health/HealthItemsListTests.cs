using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TasksExample.Api.Features.Health;

namespace TasksExample.Api.Tests.Features.Health
{
    [TestClass]
    public class HealthItemsListTests
    {
        public HealthItemsList.HandlerAsync HandlerAsync { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.HandlerAsync = new HealthItemsList.HandlerAsync();
        }

        [TestClass]
        public class HandleTests : HealthItemsListTests
        {
            [TestMethod]
            public async Task NoKey_Returns3Items()
            {
                var expectedCount = 3;

                var result = await this.HandlerAsync.Handle(new HealthItemsList.QueryAsync(""));

                Assert.AreEqual(expectedCount, result.Count);
            }

            [TestMethod]
            public async Task WithExistingKey_Returns1Item()
            {
                var expectedCount = 1;

                var result = await this.HandlerAsync.Handle(new HealthItemsList.QueryAsync("test1"));

                Assert.AreEqual(expectedCount, result.Count);
            }

            [TestMethod]
            public async Task WithInvalidKey_Returns0Item()
            {
                var expectedCount = 0;

                var result = await this.HandlerAsync.Handle(new HealthItemsList.QueryAsync("test111"));

                Assert.AreEqual(expectedCount, result.Count);
            }
        }
    }
}
