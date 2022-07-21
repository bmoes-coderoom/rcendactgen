// See https://aka.ms/new-console-template for more information

using rcendactgen.Business;
using rcendactgen.Common;
using rcendactgen.Models;

Globals.ABSOLUTE_FILEPATH = $"activitylog_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.json";
Console.WriteLine(Globals.ABSOLUTE_FILEPATH);

var logManager = new LogManager();
logManager.CreateLog();
ProcessStartActivity activity = new ProcessStartActivity
{
    ProcessId = 1,
    ProcessName = "dotnet"
};
logManager.WriteLog(activity);
ProcessStartActivity activity1 = new ProcessStartActivity
{
    ProcessId = 2,
    ProcessName = "zsh"
};
logManager.WriteLog(activity1);
FileActivity fileActivity = new FileActivity
{
    ActivityDescriptor = "create",
    Timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
    ProcessId = 3
    
};
NetworkActivity networkActivity = new NetworkActivity
{
    ProcessId = 4,
    Timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
    Username = "bmboe",
    Destination = new AddressAndPort
    {
        Address = "https://www.google.com",
        Port = 443
    },
    Source = new AddressAndPort
    {
        Address = "https://example.com",
        Port = 443
    },
    AmountOfDataSent = 1024L,
    ProtocolOfDataSent = "tcp",
    ProcessName = "zsh",
    ProcessCommandLine = "zsh"
};
logManager.WriteLog(fileActivity);
logManager.WriteLog(networkActivity);