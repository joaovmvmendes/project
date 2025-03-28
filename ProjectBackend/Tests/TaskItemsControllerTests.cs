/*using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectBackend.Controllers;
using ProjectBackend.Services;
using ProjectBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectBackend.Tests
{
    public class TaskItemsControllerTests
    {
        private readonly Mock<ITaskItemService> _mockService;
        private readonly TaskItemsController _controller;

        public TaskItemsControllerTests()
        {
            _mockService = new Mock<ITaskItemService>();
            _controller = new TaskItemsController(null, _mockService.Object);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsOkResult()
        {
            // Arrange
            var taskItems = new List<TaskItem> 
            { 
                new TaskItem { Id = 1, Title = "Task 1" },
                new TaskItem { Id = 2, Title = "Task 2" }
            };

            _mockService.Setup(service => service.GetAllTasksAsync()).ReturnsAsync(taskItems);

            // Act
            var result = await _controller.GetAllTasks();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<TaskItem>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }
    }
}*/