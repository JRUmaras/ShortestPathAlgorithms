using System;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Internals;
using EasyNetQTools.NamingConventions;
using EasyNetQTools.NamingConventions.Models;

namespace EasyNetQTools
{
    // Class is sealed to ensure that finalizer is not implemented in derived classes, making Disposable pattern simpler
    public sealed class Server<TRequest, TResponse> : IDisposable
    {
        private readonly IBus _bus;
        private readonly Func<TRequest, CancellationToken, Task<TResponse>> _processor;
        private readonly CustomNaming? _customNaming;
        private AwaitableDisposable<IDisposable> _responder;
        private bool _disposed;

        public Server(Func<TRequest, CancellationToken, Task<TResponse>> processor, string connString, CustomNaming? customNaming = null)
        {
            _bus = customNaming is null 
                ? RabbitHutch.CreateBus(connString) 
                : RabbitHutch.CreateBus(connString, CustomNamingConvention.CreateRegistrationAction(customNaming.Value));

            _processor = processor;
            _customNaming = customNaming;
        }

        public void Start()
        {
            ThrowIfDisposed();
            _responder = _bus.Rpc.RespondAsync<TRequest, TResponse>(request => _processor(request, CancellationToken.None));
        }

        public async Task StopAsync()
        {
            ThrowIfDisposed();
            if (_responder.AsTask() is null) return;
            (await _responder).Dispose();
        }
        
        public void Dispose()
        {
            if (_disposed) return;
            _bus.Dispose();
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (!_disposed) return;
            
            const string msg = $"{nameof(Server<TRequest, TResponse>)} has already been disposed.";
            throw new ObjectDisposedException(msg);
        }
    }
}

