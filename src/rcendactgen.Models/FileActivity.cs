namespace rcendactgen.Models;

public class FileActivity
{
    public string ActivityDescriptor { get; set; }
    public string Timestamp { get; set; }
    public string? FileFullPath { get; set; }
    public string UserName { get; set; }
    public string ProcessName { get; set; }
    public string ProcessCommandLine { get; set; }
    public int ProcessId { get; set; }
    
}