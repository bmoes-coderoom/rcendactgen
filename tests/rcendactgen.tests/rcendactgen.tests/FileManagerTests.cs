using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using rcendactgen.Business;
using rcendactgen.Common;
using rcendactgen.Models;
using Xunit;

namespace rcendactgen.tests;

public class FileManagerTests
{
    [Fact]
    public void DoFileAction_Should_Handle_CreateFile()
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
        var mockFileWrapper = new Mock<IFileWrapper>();
        mockFileWrapper
            .Setup(x => x.Create(It.IsAny<string>()))
            .Verifiable();
        var mockLogManager = new Mock<ILogManager>();
        var activity = new FileActivity();
        mockLogManager
            .Setup(x => x.WriteLog(It.IsAny<FileActivity>()))
            .Callback((FileActivity act) => activity = act )
            .Verifiable();
        
        // act
        new FileManager(mockLogManager.Object, mockProcessWrapper.Object, mockFileWrapper.Object).DoFileAction("create", "somePath");
        
        // assert
        mockLogManager.Verify();
        mockProcessWrapper.Verify();
        activity.Should().NotBeNull();
        using (new AssertionScope())
        {
            activity.ActivityDescriptor.Should().Be("create");
            activity.FileFullPath.Should().NotBeNullOrEmpty();
            activity.ProcessId.Should().Be(procWrapModel.Id);
            activity.ProcessName.Should().Be(procWrapModel.ProcessName);
            activity.UserName.Should().NotBeNullOrEmpty();
            activity.ProcessCommandLine.Should().NotBeNullOrEmpty();
            activity.Timestamp.Should().Be(expectedDtString);
        }
    }

    [Fact]
    public void DoFileAction_Should_Handle_ModifyFile()
    {
        // arrange
        Globals.MODIFYFILE_ABSOLUTE_FILEPATH = "somepath/file.txt";
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
        var mockFileWrapper = new Mock<IFileWrapper>();
        mockFileWrapper
            .Setup(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();
        var mockLogManager = new Mock<ILogManager>();
        var activity = new FileActivity();
        mockLogManager
            .Setup(x => x.WriteLog(It.IsAny<FileActivity>()))
            .Callback((FileActivity act) => activity = act )
            .Verifiable();
        
        // act
        new FileManager(mockLogManager.Object, mockProcessWrapper.Object, mockFileWrapper.Object).DoFileAction("modify");
        
        // assert
        mockLogManager.Verify();
        mockProcessWrapper.Verify();
        activity.Should().NotBeNull();
        using (new AssertionScope())
        {
            activity.ActivityDescriptor.Should().Be("modified");
            activity.ProcessId.Should().Be(procWrapModel.Id);
            activity.FileFullPath.Should().Be(Globals.MODIFYFILE_ABSOLUTE_FILEPATH);
            activity.ProcessName.Should().Be(procWrapModel.ProcessName);
            activity.UserName.Should().NotBeNullOrEmpty();
            activity.ProcessCommandLine.Should().NotBeNullOrEmpty();
            activity.Timestamp.Should().Be(expectedDtString);
        }
    }
    
    [Fact]
    public void DoFileAction_Should_Handle_DeleteFile()
    {
        // arrange
        Globals.DELETEFILE_ABSOLUTE_FILEPATH = "somepath/file.txt";
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
        var mockFileWrapper = new Mock<IFileWrapper>();
        mockFileWrapper
            .Setup(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();
        var mockLogManager = new Mock<ILogManager>();
        var activity = new FileActivity();
        mockLogManager
            .Setup(x => x.WriteLog(It.IsAny<FileActivity>()))
            .Callback((FileActivity act) => activity = act )
            .Verifiable();
        
        // act
        new FileManager(mockLogManager.Object, mockProcessWrapper.Object, mockFileWrapper.Object)
            .DoFileAction("delete");
        
        // assert
        mockLogManager.Verify();
        mockProcessWrapper.Verify();
        activity.Should().NotBeNull();
        using (new AssertionScope())
        {
            activity.ActivityDescriptor.Should().Be("delete");
            activity.ProcessId.Should().Be(procWrapModel.Id);
            activity.FileFullPath.Should().Be(Globals.DELETEFILE_ABSOLUTE_FILEPATH);
            activity.ProcessName.Should().Be(procWrapModel.ProcessName);
            activity.UserName.Should().NotBeNullOrEmpty();
            activity.ProcessCommandLine.Should().NotBeNullOrEmpty();
            activity.Timestamp.Should().Be(expectedDtString);
        }
    }
}