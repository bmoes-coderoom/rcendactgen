namespace rcendactgen.Models;

/// <summary>
/// Main LogRecord object
/// This object will hold the structure of the ActivityLog in-memory
/// </summary>
public class LogRecord
{
    public LogRecord()
    {
        ProcessStartActivities = new List<ProcessStartActivity>();
        FileActivities = new List<FileActivity>();
        NetworkActivities = new List<NetworkActivity>();
    }
    public List<ProcessStartActivity> ProcessStartActivities { get; set; }
    public List<FileActivity> FileActivities { get; set; }
    public List<NetworkActivity> NetworkActivities { get; set; }
}