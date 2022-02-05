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
            => Task.Run(() => _server.Start(), cancellationToken);

        public async Task StopAsync(CancellationToken cancellationToken)
            => await _server.StopAsync();

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