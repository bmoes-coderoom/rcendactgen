using System.Diagnostics;
using rcendactgen.Models;

namespace rcendactgen.Business;

public class ProcessWrapper : IProcessWrapper
{
    public ProcessWrapperModel Start(string command, string args)
    {
        DateTime procStartTime;
        string procName;
        int id = -1;
        var proc = Process.Start(command, args);
        if (proc == null) throw new NullReferenceException("Process Object is Null");
        
        // Sometimes process exits too quickly and some of the values are not available anymore
        try
        {
            id = proc.Id;
            procStartTime = proc.StartTime;
            procName = proc.ProcessName;

        }
        catch (Exception)
        {
            procStartTime = DateTime.Now;
            procName = $"{command} {args}";

        }
        
        return new ProcessWrapperModel
        {
            Id = id,
            ProcessName = procName,
            StartTime = procStartTime
        };
    }

    public ProcessWrapperModel GetCurrentProcess(bool useDifferentTimestamp = false)
    {
        var proc = Process.GetCurrentProcess();
        return new ProcessWrapperModel
        {
            Id = proc.Id,
            ProcessName = proc.ProcessName,
            StartTime = useDifferentTimestamp ? DateTime.Now : proc.StartTime
        };
    }
}