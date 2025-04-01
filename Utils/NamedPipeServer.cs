using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GreenHat.Utils
{
    public static class NamedPipeServer
    {
        private static NamedPipeServerStream _server;
        private static readonly object _lock = new object();
        private static List<Action<string>> _callbacks = new List<Action<string>>();
        private static bool _isDisposed;

        public static void StartServer()
        {
            lock (_lock)
            {
                if (_isDisposed) return;
                if (_server != null) return;

                _server = new NamedPipeServerStream("GreenHat", PipeDirection.InOut, 1);
                _ = ListenLoop();
            }
        }

        public static void AddCallback(Action<string> callback)
        {
            lock (_lock)
            {
                _callbacks.Add(callback);
            }
        }

        public static void SendMessage(string message)
        {
            lock (_lock)
            {
                if (_isDisposed || !_server?.IsConnected == true) return;

                try
                {
                    var writer = new StreamWriter(_server) { AutoFlush = true };
                    writer.WriteLine(message);
                }
                catch (ObjectDisposedException)
                {
                    RestartServer();
                }
                catch (IOException ex) when ((ex.InnerException as Win32Exception)?.NativeErrorCode == 232)
                {
                    RestartServer();
                }
            }
        }

        private static async Task ListenLoop()
        {
            while (!_isDisposed)
            {
                try
                {
                    await _server.WaitForConnectionAsync();
                    var reader = new StreamReader(_server);

                    string msg;
                    while ((msg = await reader.ReadLineAsync()) != null && !_isDisposed)
                    {
                        List<Action<string>> tempCallbacks;
                        lock (_lock)
                        {
                            tempCallbacks = new List<Action<string>>(_callbacks);
                        }
                        foreach (var cb in tempCallbacks)
                        {
                            try { cb.Invoke(msg); }
                            catch { }
                        }
                    }
                }
                catch (ObjectDisposedException) { break; }
                catch (IOException) { break; }
                finally
                {
                    if (_server != null)
                    {
                        _server.Disconnect();
                        RestartServer();
                    }
                }
            }
        }

        private static void RestartServer()
        {
            lock (_lock)
            {
                _server?.Dispose();
                _server = null;
                StartServer();
            }
        }

        public static void Dispose()
        {
            _isDisposed = true;
            _server?.Dispose();
        }
    }
}