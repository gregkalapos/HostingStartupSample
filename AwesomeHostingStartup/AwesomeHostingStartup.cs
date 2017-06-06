using System;
using System.Collections.Generic;
using System.Diagnostics;
using AwesomeHostingStartupLib;
using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AwesomeHostingStartup))]

namespace AwesomeHostingStartupLib
{
    internal class Program { public static void Main() { } }

    public class AwesomeHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            Console.WriteLine("==========================MY_AWESOME_HOSTINGSTARTUP_LOADED======================");

            var _diagnosticsListenersSubscription =
                DiagnosticListener.AllListeners.Subscribe(new DummyObserver<DiagnosticListener>((listener)
                => {
                    var _handledExceptionSubscription =
                       listener.Subscribe(new DummyObserver<KeyValuePair<string, object>>((diagnosticEvent) => {
                           Console.WriteLine($"DiagnosticSource Event: {diagnosticEvent.Key}");
                       }));
                }));
        }

        private class DummyObserver<T> : IObserver<T>
        {
            public DummyObserver(Action<T> callback) { _callback = callback; }
            public void OnCompleted() { }
            public void OnError(Exception error) { }
            public void OnNext(T value) { _callback(value); }

            private Action<T> _callback;
        }
    }
}
