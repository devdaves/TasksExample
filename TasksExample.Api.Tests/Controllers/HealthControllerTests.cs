using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TasksExample.Api.Controllers;
using TasksExample.Api.Features.Health;
using TasksExample.Api.Models;

namespace TasksExample.Api.Tests.Controllers
{
    [TestClass]
    public class HealthControllerTests
    {
        public Mock<IMediator> MediatorMock { get; set; }

        public HealthController HealthController { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.MediatorMock = new Mock<IMediator>();
            this.HealthController = new HealthController(this.MediatorMock.Object);
        }

        [TestClass]
        public class GetTests : HealthControllerTests
        {
            [TestMethod]
            public async Task DoesNotSendKeyInMessage_ToMediator()
            {
                await this.HealthController.Get();

                this.MediatorMock.Verify(m => m.Send(It.Is<HealthItemsList.QueryAsync>(x => x.Key == ""), default(CancellationToken)));
            }

            [TestMethod]
            public async Task MediatorReturnsNull_ReturnEmptyCollection()
            {
                this.MediatorMock.Setup(x => x.Send(It.IsAny<HealthItemsList.QueryAsync>(), default(CancellationToken)))
                    .ReturnsAsync((List<HealthItem>) null);

                var result = (OkNegotiatedContentResult<List<HealthItem>>) await this.HealthController.Get();

                Assert.IsFalse(result.Content.Any());
            }

            [TestMethod]
            public async Task MediatorReturns2Items_Return2Items()
            {
                var expectedCount = 2;
                this.MediatorMock.Setup(x => x.Send(It.IsAny<HealthItemsList.QueryAsync>(), default(CancellationToken)))
                    .ReturnsAsync(new List<HealthItem>() {new HealthItem(), new HealthItem()});

                var result = (OkNegotiatedContentResult<List<HealthItem>>) await this.HealthController.Get();

                Assert.AreEqual(expectedCount, result.Content.Count);
            }
        }

        [TestClass]
        public class GetWithKeyTests : HealthControllerTests
        {
            [TestMethod]
            public async Task SendsKeyInMessage_ToMediator()
            {
                var expectedKey = "test1";

                await this.HealthController.Get(expectedKey);

                this.MediatorMock.Verify(m => m.Send(It.Is<HealthItemsList.QueryAsync>(x => x.Key == expectedKey), default(CancellationToken)));
            }
            
            [TestMethod]
            public async Task MediatorReturnsNull_Return404()
            {
                var expectedType = typeof(NotFoundResult);

                this.MediatorMock.Setup(x => x.Send(It.IsAny<HealthItemsList.QueryAsync>(), default(CancellationToken)))
                    .ReturnsAsync((List<HealthItem>)null);

                var result = await this.HealthController.Get("somekey");

                Assert.IsInstanceOfType(result, expectedType);
            }

            [TestMethod]
            public async Task MediatorReturns2Items_ReturnSingleItem()
            {
                var expectedKey = "test1";

                this.MediatorMock.Setup(x => x.Send(It.IsAny<HealthItemsList.QueryAsync>(), default(CancellationToken)))
                    .ReturnsAsync(new List<HealthItem>() { new HealthItem() {Key = expectedKey}, new HealthItem() });

                var result = (OkNegotiatedContentResult<HealthItem>)await this.HealthController.Get(expectedKey);

                Assert.AreEqual(expectedKey, result.Content.Key);
            }
        }

    }
}
