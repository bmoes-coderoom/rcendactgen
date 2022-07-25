using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Moq.Contrib.HttpClient;
using rcendactgen.Business;
using rcendactgen.Models;
using Xunit;

namespace rcendactgen.tests;

public class NetworkManagerTests
{
    [Fact]
    public async Task TransmitDataAsync_Should_SendData_And_Call_LogManager()
    {
        // arrange
        var mockProcessWrapper = new Mock<IProcessWrapper>();
        var expectedDateTime = DateTime.Now;
        var procWrapModel = new ProcessWrapperModel
        {
            Id = 1,
            ProcessName = "name",
            StartTime = expectedDateTime
        };
        var expectedDtString = expectedDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        mockProcessWrapper
            .Setup(x => x.GetCurrentProcess(true))
            .Returns(procWrapModel);
        var handler = new Mock<HttpMessageHandler>();
        handler.SetupAnyRequest()
            .ReturnsResponse(HttpStatusCode.Accepted);
        
        var mockLogManager = new Mock<ILogManager>();
        var activity = new NetworkActivity();
        mockLogManager
            .Setup(x => x.WriteLog(It.IsAny<NetworkActivity>()))
            .Callback((NetworkActivity act) => activity = act )
            .Verifiable();
        
        // act
        await new NetworkManager(handler.CreateClient(), mockProcessWrapper.Object, mockLogManager.Object).TransmitDataAsync();
        
        // assert
        mockLogManager.Verify();
        mockProcessWrapper.Verify();
        activity.Should().NotBeNull();
        using (new AssertionScope())
        {
            activity.ProcessId.Should().Be(procWrapModel.Id);
            activity.ProcessName.Should().Be(procWrapModel.ProcessName);
            activity.Username.Should().NotBeNullOrEmpty();
            activity.ProcessCommandLine.Should().NotBeNullOrEmpty();
            activity.Timestamp.Should().Be(expectedDtString);
            activity.Destination.Should().NotBeNull();
            activity.Destination.Address.Should().NotBeNullOrEmpty();
            activity.Destination.Port.Should().BeGreaterOrEqualTo(0);
            activity.Source.Should().NotBeNull();
            activity.Source.Address.Should().NotBeNullOrEmpty();
            activity.Source.Port.Should().BeGreaterOrEqualTo(0);
            activity.AmountOfDataSent.Should().BeGreaterThan(0);
            activity.ProtocolOfDataSent.Should().NotBeNullOrEmpty();
        }
    }
    
        [Fact]
    public async Task TransmitDataAsync_Should_LogError_When_SendingData_ThrowsException()
    {
        // arrange
        var mockProcessWrapper = new Mock<IProcessWrapper>();
        var expectedDateTime = DateTime.Now;
        var procWrapModel = new ProcessWrapperModel
        {
            Id = 1,
            ProcessName = "name",
            StartTime = expectedDateTime
        };
        var expectedDtString = expectedDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        mockProcessWrapper
            .Setup(x => x.GetCurrentProcess(true))
            .Returns(procWrapModel);
        var handler = new Mock<HttpMessageHandler>();
        handler.SetupAnyRequest()
            .ThrowsAsync(new Exception());
        
        var mockLogManager = new Mock<ILogManager>();
        mockLogManager
            .Setup(x => x.WriteError(It.IsAny<string>(), It.IsAny<Exception>()))
            .Verifiable();
        
        // act
        await new NetworkManager(handler.CreateClient(), mockProcessWrapper.Object, mockLogManager.Object).TransmitDataAsync();
        
        // assert
        mockLogManager.Verify();
        mockProcessWrapper.Verify();
    }
    
        [Fact]
    public async Task TransmitDataAsync_Should_LogError_WhenLoggingNetworkActivityFails()
    {
        // arrange
        var mockProcessWrapper = new Mock<IProcessWrapper>();
        var expectedDateTime = DateTime.Now;
        var procWrapModel = new ProcessWrapperModel
        {
            Id = 1,
            ProcessName = "name",
            StartTime = expectedDateTime
        };
        var expectedDtString = expectedDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        mockProcessWrapper
            .Setup(x => x.GetCurrentProcess(true))
            .Returns(procWrapModel);
        var handler = new Mock<HttpMessageHandler>();
        handler.SetupAnyRequest()
            .ReturnsResponse(HttpStatusCode.Accepted);
        
        var mockLogManager = new Mock<ILogManager>();
        var activity = new NetworkActivity();
        mockLogManager
            .Setup(x => x.WriteLog(It.IsAny<NetworkActivity>()))
            .Throws(new Exception())
            .Verifiable();
        mockLogManager
            .Setup(x => x.WriteError(It.IsAny<string>(), It.IsAny<Exception>()))
            .Verifiable();
        
        // act
        await new NetworkManager(handler.CreateClient(), mockProcessWrapper.Object, mockLogManager.Object).TransmitDataAsync();
        
        // assert
        mockLogManager.Verify();
        mockProcessWrapper.Verify();
    }
}