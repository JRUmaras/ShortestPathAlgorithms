using System;
using System.Threading;
using System.Threading.Tasks;
using BruteForceSpaService.Options;
using EasyNetQTools;
using EasyNetQTools.NamingConventions.Models;
using EasyNetQTools.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ShortestPathAlgorithms.Algorithms;
using ShortestPathAlgorithms.CostCalculators;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;

namespace BruteForceSpaService;

public sealed class BruteForceSpaService : IHostedService, IDisposable
{
    private readonly Server<Request, Response<double>> _server;
    private Task? _startupTask;

    public BruteForceSpaService(IOptions<RabbitMqOptions> rabbitMqOptions, IOptions<BruteForceSpaServiceOptions> bruteForceSpaServiceOptions)
    {
        var customNaming = new CustomNaming
        {
            ExchangeName = bruteForceSpaServiceOptions.Value.ExchangeName,
            RequestQueueName = bruteForceSpaServiceOptions.Value.RequestQueueName,
        };
            
        _server = new Server<Request, Response<double>>(Processor, rabbitMqOptions.Value.ConnString, customNaming);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _startupTask = Task.Run(() => _server.Start(), cancellationToken);

        return _startupTask.IsCompleted
            ? _startupTask
            : Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_startupTask is not null)  await _startupTask;
            
        await _server.StopAsync();
    }
        
    public void Dispose()
    {
        _server.Dispose();
    }

    private static Task<Response<double>> Processor(Request request, CancellationToken ct)
    {
        return Task.Run(() =>
        {
            var costCalc = new UnitCostCalculator();
            var startState = new State
            {
                Time = DateTime.UtcNow.Date
            };

            var path = DijkstraDynamic<IState>.Find(request.Graph, request.Start, request.End, costCalc, startState);
            return new Response<double>(path);
        }, ct);
    }
}