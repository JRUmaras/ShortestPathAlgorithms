using System;
using System.Threading;
using System.Threading.Tasks;
using BruteForceSpaService.Options;
using EasyNetQTools;
using EasyNetQTools.NamingConventions.Models;
using EasyNetQTools.Options;
using Microsoft.Extensions.Hosting;
using ShortestPathAlgorithms.Algorithms;

namespace BruteForceSpaService
{
    public class BruteForceSpaService : IHostedService, IDisposable
    {
        private readonly Server<Request, Response> _server;
        private Task? _startupTask;

        public BruteForceSpaService(RabbitMqOptions rabbitMqOptions, BruteForceSpaServiceOptions bruteForceSpaServiceOptions)
        {
            var customNaming = new CustomNaming
            {
                ExchangeName = bruteForceSpaServiceOptions.ExchangeName,
                RequestQueueName = bruteForceSpaServiceOptions.RequestQueueName,
            };
            
            _server = new Server<Request, Response>(Processor, rabbitMqOptions.ConnString, customNaming);
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
            if (_startupTask != null)  await _startupTask;
            
            await _server.StopAsync();
        }
        
        public void Dispose()
        {
            _server.Dispose();
        }

        private static Task<Response> Processor(Request request, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                var path = DepthFirstBruteForce.FindShortestPath(request.Graph, request.Start, request.End);
                return new Response
                {
                    Path = path
                };
            }, ct);
        }
    }
}