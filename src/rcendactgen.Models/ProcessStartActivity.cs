namespace rcendactgen.Models;

public class ProcessStartActivity
{
    public string StartTime { get; set; }
    public string UserName { get; set; }
    public string ProcessName { get; set; }
    public string ProcessCommandLine { get; set; }
    public int ProcessId { get; set; }
}