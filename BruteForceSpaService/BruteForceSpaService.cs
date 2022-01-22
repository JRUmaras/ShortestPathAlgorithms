using System;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQTools;
using EasyNetQTools.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ShortestPathAlgorithms.Algorithms;

namespace BruteForceSpaService
{
    public class BruteForceSpaService : IHostedService, IDisposable
    {
        private readonly Server<Request, Response> _server;

        public BruteForceSpaService(IOptions<RabbitMqOptions> rabbitMqConfigOptions)
        {
            _server = new Server<Request, Response>(request
                => Task.Run(() =>
                {
                    var path = DepthFirstBruteForce.FindShortestPath(request.Graph, request.Start, request.End);
                    return new Response
                    {
                        Path = path
                    };
                }), rabbitMqConfigOptions);
        }

        public Task StartAsync(CancellationToken cancellationToken)
            => Task.Run(() => _server.Start(), cancellationToken);

        public async Task StopAsync(CancellationToken cancellationToken)
            => await _server.StopAsync();

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}