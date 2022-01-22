using System;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Internals;
using EasyNetQTools.Options;
using Microsoft.Extensions.Options;

namespace EasyNetQTools
{
    // Class is sealed to ensure that finalizer is not implemented in derived classes, making Disposable pattern simpler
    public sealed class Server<TRequest, TResponse> : IDisposable
    {
        private readonly IBus _bus;
        private const string _connString = "host=localhost;port=5672;virtualHost=/;username=server;password=server";
        private readonly Func<TRequest, Task<TResponse>> _processor;
        private AwaitableDisposable<IDisposable> _responder;
        private bool _disposed;

        public Server(Func<TRequest, Task<TResponse>> processor, IOptions<RabbitMqOptions> rabbitMqConfigOptions)
        {
            _bus = RabbitHutch.CreateBus(rabbitMqConfigOptions.Value.ConnString);
            _processor = processor;
        }

        public void Start()
        {
            ThrowIfDisposed();
            _responder = _bus.Rpc.RespondAsync(_processor);
        }

        public async Task StopAsync()
        {
            ThrowIfDisposed();
            if (_responder.AsTask() is null) return;
            (await _responder).Dispose();
        }

        private void ThrowIfDisposed()
        {
            if (!_disposed) return;
            
            const string msg = $"{nameof(Server<TRequest, TResponse>)} has already been disposed.";
            throw new ObjectDisposedException(msg);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _bus.Dispose();
            _disposed = true;
        }
    }
}

