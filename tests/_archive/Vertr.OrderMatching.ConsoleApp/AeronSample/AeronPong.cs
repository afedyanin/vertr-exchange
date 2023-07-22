using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adaptive.Aeron.LogBuffer;
using Adaptive.Aeron;
using Adaptive.Agrona.Concurrent;
using Adaptive.Agrona;

namespace Vertr.OrderMatching.ConsoleApp.AeronSample;

public static class AeronPong
{
    private static readonly int PingStreamID = SampleConfiguration.PING_STREAM_ID;
    private static readonly int PongStreamID = SampleConfiguration.PONG_STREAM_ID;
    private static readonly string PingChannel = SampleConfiguration.PING_CHANNEL;
    private static readonly string PongChannel = SampleConfiguration.PONG_CHANNEL;
    private static readonly int FrameCountLimit = SampleConfiguration.FRAGMENT_COUNT_LIMIT;

    private static readonly IIdleStrategy PingHandlerIdleStrategy = new BusySpinIdleStrategy();

    public static void Start()
    {
        var ctx = new Aeron.Context()
            .AvailableImageHandler(SampleUtil.PrintAvailableImage)
            .UnavailableImageHandler(SampleUtil.PrintUnavailableImage);

        IIdleStrategy idleStrategy = new BusySpinIdleStrategy();

        Console.WriteLine("Subscribing Ping at " + PingChannel + " on stream Id " + PingStreamID);
        Console.WriteLine("Publishing Pong at " + PongChannel + " on stream Id " + PongStreamID);

        var running = new AtomicBoolean(true);
        Console.CancelKeyPress += (_, e) => running.Set(false);

        using (var aeron = Aeron.Connect(ctx))
        using (var pongPublication = aeron.AddPublication(PongChannel, PongStreamID))
        using (var pingSubscription = aeron.AddSubscription(PingChannel, PingStreamID))
        {
            var dataHandler = HandlerHelper.ToFragmentHandler(
                (buffer, offset, length, header) => PingHandler(pongPublication, buffer, offset, length)
            );

            while (running)
            {
                idleStrategy.Idle(pingSubscription.Poll(dataHandler, FrameCountLimit));
            }

            Console.WriteLine("Shutting down...");
        }
    }

    private static void PingHandler(Publication pongPublication, IDirectBuffer buffer, int offset, int length)
    {
        if (pongPublication.Offer(buffer, offset, length) > 0L)
        {
            return;
        }

        PingHandlerIdleStrategy.Reset();

        while (pongPublication.Offer(buffer, offset, length) < 0L)
        {
            PingHandlerIdleStrategy.Idle();
        }
    }
}
