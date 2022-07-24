using System.Diagnostics;
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
        var proc = _processWrapper.GetCurrentProcess(true);
        _fileWrapper.Create(absPath);
        FileActivity activity = new FileActivity
        {
            ActivityDescriptor = "create",
            Timestamp = proc.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            FileFullPath = absPath,
            UserName = Environment.UserName,
            ProcessName = proc.ProcessName,
            ProcessCommandLine = Environment.CommandLine,
            ProcessId = proc.Id
        };
        _logManager.WriteLog(activity);
    }
    
    private void ModifyFile()
    {
        var proc = _processWrapper.GetCurrentProcess(true);
        _fileWrapper.WriteAllText(Globals.MODIFYFILE_ABSOLUTE_FILEPATH, "This file was modified");
        FileActivity activity = new FileActivity
        {
            ActivityDescriptor = "modified",
            Timestamp = proc.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            FileFullPath = Globals.MODIFYFILE_ABSOLUTE_FILEPATH,
            UserName = Environment.UserName,
            ProcessName = proc.ProcessName,
            ProcessCommandLine = Environment.CommandLine,
            ProcessId = proc.Id
        };
        _logManager.WriteLog(activity);
    }
    
    private void DeleteFile()
    {
        var proc = _processWrapper.GetCurrentProcess(true);
        _fileWrapper.Delete(Globals.DELETEFILE_ABSOLUTE_FILEPATH);
        FileActivity activity = new FileActivity
        {
            ActivityDescriptor = "delete",
            Timestamp = proc.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            FileFullPath = Globals.DELETEFILE_ABSOLUTE_FILEPATH,
            UserName = Environment.UserName,
            ProcessName = proc.ProcessName,
            ProcessCommandLine = Environment.CommandLine,
            ProcessId = proc.Id
        };
        _logManager.WriteLog(activity);
    }
}