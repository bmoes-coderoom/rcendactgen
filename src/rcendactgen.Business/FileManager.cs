using rcendactgen.Common;
using rcendactgen.Models;

namespace rcendactgen.Business;

public class FileManager
{
    private readonly ILogManager _logManager;
    private readonly IProcessWrapper _processWrapper;
    private readonly IFileWrapper _fileWrapper;

    public FileManager(ILogManager logManager, IProcessWrapper processWrapper, IFileWrapper fileWrapper)
    {
        _logManager = logManager;
        _processWrapper = processWrapper;
        _fileWrapper = fileWrapper;
    }

    public FileManager()
    {
        _logManager = new LogManager();
        _processWrapper = new ProcessWrapper();
        _fileWrapper = new FileWrapper();
    }

    public void DoFileAction(string fileAction, string? absPath = null)
    {
        if (fileAction == "create")
        {
            CreateFile(absPath);
        }
        else if (fileAction == "modify")
        {
            ModifyFile();
        }
        else if (fileAction == "delete")
        {
            DeleteFile();
        }
        
    }

    private void CreateFile(string? absPath)
    {
        ProcessWrapperModel proc = null;
        FileWrapObj fileWrapObj = null;
        try
        {
            proc = _processWrapper.GetCurrentProcess(true);
            fileWrapObj = _fileWrapper.Create(absPath);
        }
        catch (Exception ex)
        {
            _logManager.WriteError(@"There was an error creating the file.
The current activity will not be logged to the activity log", ex);
            return;
        }

        try
        {
            FileActivity activity = new FileActivity
            {
                ActivityDescriptor = "create",
                Timestamp = proc.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                FileFullPath = fileWrapObj.AbsoluteFilePath,
                UserName = CurrentUserInfo.GetCurrentUserWithDomain(),
                ProcessName = proc.ProcessName,
                ProcessCommandLine = Environment.CommandLine,
                ProcessId = proc.Id
            };
            _logManager.WriteLog(activity);
        }
        catch (Exception ex)
        {
            _logManager.WriteError(@"There was an error logging the activity", ex);
        }

    }
    
    private void ModifyFile()
    {
        ProcessWrapperModel proc = null;
        try
        {
            proc = _processWrapper.GetCurrentProcess(true);
            _fileWrapper.WriteAllText(Globals.MODIFYFILE_ABSOLUTE_FILEPATH, "This file was modified");
        }
        catch (Exception ex)
        {
            _logManager.WriteError(@"There was an error modifying the file.
The current activity will not be logged to the activity log", ex);
            return;
        }
        
        try
        {
            FileActivity activity = new FileActivity
            {
                ActivityDescriptor = "modified",
                Timestamp = proc.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                FileFullPath = Globals.MODIFYFILE_ABSOLUTE_FILEPATH,
                UserName = CurrentUserInfo.GetCurrentUserWithDomain(),
                ProcessName = proc.ProcessName,
                ProcessCommandLine = Environment.CommandLine,
                ProcessId = proc.Id
            };
            _logManager.WriteLog(activity);
        }
        catch (Exception ex)
        {
            _logManager.WriteError(@"There was an error logging the activity",ex);
        }
    }
    
    private void DeleteFile()
    {
        ProcessWrapperModel proc = null;
        try
        {
            proc = _processWrapper.GetCurrentProcess(true);
            _fileWrapper.Delete(Globals.DELETEFILE_ABSOLUTE_FILEPATH);
        }
        catch (Exception ex)
        {
            _logManager.WriteError(@"There was an error deleting the file.
The current activity will not be logged to the activity log", ex);
            return;
        }


        try
        {
            FileActivity activity = new FileActivity
            {
                ActivityDescriptor = "delete",
                Timestamp = proc.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                FileFullPath = Globals.DELETEFILE_ABSOLUTE_FILEPATH,
                UserName = CurrentUserInfo.GetCurrentUserWithDomain(),
                ProcessName = proc.ProcessName,
                ProcessCommandLine = Environment.CommandLine,
                ProcessId = proc.Id
            };
            _logManager.WriteLog(activity);
        }
        catch (Exception ex)
        {
            _logManager.WriteError(@"There was an error logging the activity", ex);
        }
    }
}