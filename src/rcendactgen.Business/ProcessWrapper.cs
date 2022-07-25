using System.Diagnostics;
using rcendactgen.Models;

namespace rcendactgen.Business;

public class ProcessWrapper : IProcessWrapper
{
    public ProcessWrapperModel Start(string command, string args)
    {
        var proc = Process.Start(command, args);
        return new ProcessWrapperModel
        {
            Id = proc.Id,
            ProcessName = proc.ProcessName,
            StartTime = proc.StartTime
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