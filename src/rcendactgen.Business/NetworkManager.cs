using System.Text;
using Newtonsoft.Json;
using rcendactgen.Common;
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
        ProcessWrapperModel proc = null;
        string requestUri = "https://jsonplaceholder.typicode.com/posts";
        StringContent content = null;
        try
        {
            proc = _processWrapper.GetCurrentProcess(true);
            var post = new Post
            {
                Title = "foo",
                Body = "bar",
                UserId = 1
            };

            var strBody = JsonConvert.SerializeObject(post);
            content = new StringContent(strBody, Encoding.UTF8);
            await _client.PostAsync(requestUri, content);
        }
        catch (Exception ex)
        {
            _logManager.WriteError(@"There was an error transmitting data.
The current activity will not be logged to the activity log", ex);
            return;
        }

        try
        {
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
                Username = CurrentUserInfo.GetCurrentUserWithDomain(),
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

    class Post
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
    }
}