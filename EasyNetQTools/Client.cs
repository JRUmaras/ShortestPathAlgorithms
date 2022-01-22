using System.Threading.Tasks;
using EasyNetQ;

namespace EasyNetQTools;

public class Client<TRequest, TResponse>
{
    private readonly IBus _bus;
    private const string _connString = "host=localhost;port=5672;virtualHost=/;username=client;password=client";

    public Client()
    {
        _bus = RabbitHutch.CreateBus(_connString);
    }

    public async Task<TResponse> MakeRequestAsync(TRequest request) 
        => await _bus.Rpc
            .RequestAsync<TRequest, TResponse>(request)
            .ConfigureAwait(false);

    public void Dispose()
    {
        _bus.Dispose();
    }
}