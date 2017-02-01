using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TasksExample.Api.Features.Tasks;
using TasksExample.Api.Infrastructure.Data;
using TasksExample.Api.Models;

namespace TasksExample.Api.Tests.Features.Tasks
{
    [TestClass]
    public class TasksListTests
    {
        public Mock<TasksContext> TasksContextMock { get; set; }

        public TasksList.HandlerAsync HandlerAsync { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.TasksContextMock = new Mock<TasksContext>();
            this.HandlerAsync = new TasksList.HandlerAsync(this.TasksContextMock.Object);
        }

        [TestClass]
        public class Handle : TasksListTests
        {
            [TestMethod]
            public async Task ReturnsDataFromContext()
            {
                var data = new List<TaskItem>()
                {
                    new TaskItem() {Id = 1},
                    new TaskItem() {Id = 2},
                    new TaskItem() {Id = 3},
                };
                this.TasksContextMock.Setup(x => x.Tasks).Returns(data);

                var results = await this.HandlerAsync.Handle(new TasksList.QueryAsync(1));

                Assert.AreEqual(data.Count, results.Count());
            }
        }
    }
}
