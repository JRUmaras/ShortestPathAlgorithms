using System;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQTools.NamingConventions;
using EasyNetQTools.NamingConventions.Models;

namespace EasyNetQTools;

public class Client<TRequest, TResponse> : IDisposable
{
    private readonly IBus _bus;
    private const string _connString = "host=localhost;port=5672;virtualHost=/;username=guest;password=guest";

    public Client(CustomNaming? customNaming)
    {
        _bus = customNaming is null 
            ? RabbitHutch.CreateBus(_connString) 
            : RabbitHutch.CreateBus(_connString, CustomNamingConvention.CreateRegistrationAction(customNaming.Value));
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