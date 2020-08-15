using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Server;

namespace ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var sensorClient = new Sensor.SensorClient(channel);
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            using var sensorCall = sensorClient.GetReading(new SensorReadingRequest(), cancellationToken: CancellationToken.None);

            try
            {
                while (await sensorCall.ResponseStream.MoveNext(CancellationToken.None))
                {
                    Console.WriteLine("Greeting: " + sensorCall.ResponseStream.Current.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
