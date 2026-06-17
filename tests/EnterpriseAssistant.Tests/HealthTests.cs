using Microsoft.AspNetCore.Mvc;
using Xunit;
using EnterpriseAssistant.Web.Controllers;
using EnterpriseAssistant.Web.Models;

namespace EnterpriseAssistant.Tests;

public sealed class HealthControllerTests
{
    [Fact]
    public void Get_ReturnsHealthyStatus()
    {
        var controller = new HealthController();
        var result = controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsType<HealthResponse>(okResult.Value);

        Assert.Equal("healthy", value.Status);
    }
}
