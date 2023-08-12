namespace Vertr.Exchange.MatchingEngine.Tests.Stubs;

internal static class OrderGen
{
    private static readonly long _timeIncrement = 100;

    private static long _nextId = 1000L;
    private static DateTime _nextTime = new DateTime(2023, 08, 04, 16, 04, 45);

    public static long NextId => _nextId++;

    public static DateTime NextTime
    {
        get
        {
            _nextTime = _nextTime.AddTicks(_timeIncrement);
            return _nextTime;
        }
    }

    public static long UserId => 56879L;
}
