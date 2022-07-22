namespace rcendactgen.Models;

public class NetworkActivity
{
    public string Timestamp { get; set; }
    public string Username { get; set; }
    public AddressAndPort Destination { get; set; }
    public AddressAndPort Source { get; set; }
    public long AmountOfDataSent { get; set; }
    public string ProtocolOfDataSent { get; set; }
    public string ProcessName { get; set; }
    public string ProcessCommandLine { get; set; }
    public int ProcessId { get; set; }
}