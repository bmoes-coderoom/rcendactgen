using System.Text;
using Newtonsoft.Json;
using rcendactgen.Models;

namespace rcendactgen.Business;

public class NetworkManager
{
    private readonly HttpClient _client;
    private readonly IProcessWrapper _processWrapper;
    private readonly ILogManager _logManager;

    public NetworkManager(HttpClient client, IProcessWrapper processWrapper, ILogManager logManager)
    {
        _client = client;
        _processWrapper = processWrapper;
        _logManager = logManager;
    }

    public NetworkManager()
    {
        _client = new HttpClient();
        _processWrapper = new ProcessWrapper();
        _logManager = new LogManager();
    }

    public async Task TransmitDataAsync()
    {
        var proc = _processWrapper.GetCurrentProcess(true);
        var post = new Post
        {
            Title = "foo",
            Body = "bar",
            UserId = 1
        };
        string requestUri = "https://jsonplaceholder.typicode.com/posts";
        var strBody = JsonConvert.SerializeObject(post);
        var content = new StringContent(strBody, Encoding.UTF8);
        await _client.PostAsync(requestUri, content);
        NetworkActivity activity = new NetworkActivity
        {
            Timestamp = proc.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            Destination = new AddressAndPort
            {
                Address = requestUri,
                Port = 443
            },
            Source = new AddressAndPort
            {
                Address = System.Net.Dns.GetHostName(),
                Port = 443
            },
            AmountOfDataSent = (await content.ReadAsByteArrayAsync()).Length,
            ProtocolOfDataSent = "tcp",
            Username = Environment.UserName,
            ProcessName = proc.ProcessName,
            ProcessCommandLine = Environment.CommandLine,
            ProcessId = proc.Id
        };
        _logManager.WriteLog(activity);
    }

    class Post
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
    }
}