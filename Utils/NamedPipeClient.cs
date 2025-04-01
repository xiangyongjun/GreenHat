using System.IO.Pipes;
using System.IO;
using System.Threading.Tasks;

namespace GreenHat.Utils
{
    public static class NamedPipeClient
    {
        private static NamedPipeClientStream _pipeClient;

        public static void Connect()
        {
            _pipeClient?.Close();
            _pipeClient = new NamedPipeClientStream(".", "GreenHat", PipeDirection.InOut);
            _pipeClient.Connect();
        }

        public static string SendMessage(string message)
        {
            using (var writer = new StreamWriter(_pipeClient) { AutoFlush = true })
            using (var reader = new StreamReader(_pipeClient))
            {
                writer.WriteLine(message);
                return reader.ReadLine();
            }
        }

        public static async Task<string> SendMessageAsync(string message)
        {
            var writer = new StreamWriter(_pipeClient) { AutoFlush = true };
            await writer.WriteLineAsync(message);

            var reader = new StreamReader(_pipeClient);
            var result = await reader.ReadLineAsync();

            writer.Dispose();
            reader.Dispose();

            return result;
        }
    }
}
