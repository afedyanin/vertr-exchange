using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Adaptive.Aeron;
using Adaptive.Aeron.LogBuffer;
using Adaptive.Agrona;
using Adaptive.Agrona.Concurrent;

namespace Vertr.OrderMatching.ConsoleApp.AeronSample;

internal static class AeronHello
{
    public static void Start()
    {
        const string channel = "aeron:ipc";
        const int streamId = 42;

        var buffer = new UnsafeBuffer(new byte[256]);
        var handler = HandlerHelper.ToFragmentHandler(PrintMessage);

        try
        {
            using (var aeron = Aeron.Connect())
            using (var publisher = aeron.AddPublication(channel, streamId))
            using (var subscriber = aeron.AddSubscription(channel, streamId))
            {
                var message = buffer.PutStringWithoutLengthUtf8(0, "Hello World!");

                publisher.Offer(buffer, 0, message);
                Console.WriteLine("Message sent...");

                while (subscriber.Poll(handler, 1) == 0)
                {
                    Thread.Sleep(10);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private static void PrintMessage(IDirectBuffer buffer, int offset, int length, Header header)
    {
        var message = buffer.GetStringWithoutLengthUtf8(offset, length);

        Console.WriteLine($"Received message ({message}) to stream {header.StreamId:D} from session {header.SessionId:x} term id {header.TermId:x} term offset {header.TermOffset:D} ({length:D}@{offset:D})");
    }
}
