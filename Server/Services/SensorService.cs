using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace Server
{
    public class SensorService : Sensor.SensorBase
    {
        public override async Task GetReading(SensorReadingRequest request, IServerStreamWriter<SensorReadingReply> responseStream, ServerCallContext context)
        {
            var rand = new Random();
            float last = 0.01F;

            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(50);

                SensorReadingReply response = new SensorReadingReply();

                // var step = (float)rand.NextDouble();

                // if(rand.NextDouble() < 0.5) {
                //   step *= -1;
                // }

                // response.Value = lastValue + step;
                Console.WriteLine(last);
                last += 0.1f;
                response.Value = (float) Math.Sin(last);

                response.Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow);

                await responseStream.WriteAsync(response);
            }
        }
    }
}
