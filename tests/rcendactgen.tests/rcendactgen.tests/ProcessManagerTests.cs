using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using rcendactgen.Business;
using rcendactgen.Models;
using Xunit;

namespace rcendactgen.tests;

public class ProcessManagerTests
{
    [Theory]
    [InlineData("/Users/someuser/App/bin/Release/net6.0/osx-arm64/DotNet.Docker 5")]
    [InlineData("/Users/someuser/App/bin/Release/net6.0/osx-arm64/DotNet.Docker")]
    public void StartProcess_Should_StartGivenProcess_And_Pass_ProcessStartActivityToLogManager(string fullCommand)
    {
        // arrange
        var commandArr = fullCommand.Split(" ").ToList();
        string baseCommand = commandArr[0];
        commandArr.Remove(commandArr.First());
        string args = string.Join(" ", commandArr);
        var mockLogManager = new Mock<ILogManager>();
        var mockProcessWrapper = new Mock<IProcessWrapper>();
        var expectedDateTime = DateTime.Now;
        var procWrapperModel = new ProcessWrapperModel
        {
            Id = 1,
            ProcessName = "foo",
            StartTime = expectedDateTime
        };
        var expectedDTString = expectedDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        mockProcessWrapper
            .Setup(x => x.Start(baseCommand, args))
            .Returns(procWrapperModel)
            .Verifiable();
        var processStartActivity = new ProcessStartActivity();
        mockLogManager
            .Setup(x => x.WriteLog(It.IsAny<ProcessStartActivity>()))
            .Callback((ProcessStartActivity act) => processStartActivity = act )
            .Verifiable();
        
        // act
        new ProcessManager(mockLogManager.Object, mockProcessWrapper.Object).StartProcess(fullCommand);
        
        // assert
        mockLogManager.Verify();
        mockProcessWrapper.Verify();
        processStartActivity.Should().NotBeNull();
        using (new AssertionScope())
        {
            processStartActivity.ProcessId.Should().BeGreaterThan(0);
            processStartActivity.ProcessName.Should().NotBeNullOrEmpty();
            processStartActivity.UserName.Should().NotBeNullOrEmpty();
            processStartActivity.ProcessCommandLine.Should().Be(fullCommand);
            processStartActivity.StartTime.Should().Be(expectedDTString);
        }
    }
    
    [Fact]
    public void StartProcess_Should_LogError_WhenAnErrorOccursWhileStartingAProcess()
    {
        // arrange
        string fullCommand = "ls -0";
        var commandArr = fullCommand.Split(" ").ToList();
        string baseCommand = commandArr[0];
        commandArr.Remove(commandArr.First());
        string args = string.Join(" ", commandArr);
        var mockProcessWrapper = new Mock<IProcessWrapper>();
        var exception = new Exception("Invalid arguments passed");
        mockProcessWrapper
            .Setup(x => x.Start(baseCommand, args))
            .Throws(exception)
            .Verifiable();
        var mockLogManager = new Mock<ILogManager>();
        mockLogManager
            .Setup(x => x.WriteError(It.IsAny<string>(), It.IsAny<Exception>()))
            .Verifiable();

        // act
        new ProcessManager(mockLogManager.Object, mockProcessWrapper.Object).StartProcess(fullCommand);
        // assert
        mockProcessWrapper.Verify();
        mockLogManager.Verify();
    }
    
    [Fact]
    public void StartProcess_Should_LogError_WhenAnErrorOccursLoggingActivity()
    {
        // arrange
        string fullCommand = "/Users/someuser/App/bin/Release/net6.0/osx-arm64/DotNet.Docker";
        var commandArr = fullCommand.Split(" ").ToList();
        string baseCommand = commandArr[0];
        commandArr.Remove(commandArr.First());
        string args = string.Join(" ", commandArr);
        var mockLogManager = new Mock<ILogManager>();
        var mockProcessWrapper = new Mock<IProcessWrapper>();
        var expectedDateTime = DateTime.Now;
        var procWrapperModel = new ProcessWrapperModel
        {
            Id = 1,
            ProcessName = "foo",
            StartTime = expectedDateTime
        };
        mockProcessWrapper
            .Setup(x => x.Start(baseCommand, args))
            .Returns(procWrapperModel)
            .Verifiable();
        var exception = new Exception("logging failed");
        mockLogManager
            .Setup(x => x.WriteLog(It.IsAny<ProcessStartActivity>()))
            .Throws(exception)
            .Verifiable();
        mockLogManager
            .Setup(x => x.WriteError(It.IsAny<string>(), It.IsAny<Exception>()))
            .Verifiable();
        
        // act
        new ProcessManager(mockLogManager.Object, mockProcessWrapper.Object).StartProcess(fullCommand);
        // assert
        mockLogManager.Verify();
        mockProcessWrapper.Verify();
    }
}