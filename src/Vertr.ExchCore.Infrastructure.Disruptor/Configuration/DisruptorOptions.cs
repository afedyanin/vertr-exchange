namespace Vertr.ExchCore.Infrastructure.Disruptor.Configuration;

internal class DisruptorOptions
{
    public static readonly string Disruptor = "Disruptor";
    public int RingBufferSize { get; set; } = 1024;
}
