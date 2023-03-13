public delegate bool RetryHandler();

public class RetryPolicy
{
    private int _maxAttempts;
    private TimeSpan _initialDelay;
    private TimeSpan _maxDelay;
    private Random _random;

    public RetryPolicy(int maxAttempts, TimeSpan initialDelay, TimeSpan maxDelay)
    {
        _maxAttempts = maxAttempts;
        _initialDelay = initialDelay;
        _maxDelay = maxDelay;
        _random = new Random();
    }

    public bool Execute(RetryHandler handler)
    {
        int attempt = 0;
        TimeSpan delay = _initialDelay;

        while (attempt < _maxAttempts)
        {
            bool success = handler();

            if (success)
            {
                return true;
            }

            attempt++;
            delay = TimeSpan.FromTicks(Math.Min(delay.Ticks * 2, _maxDelay.Ticks));
            Thread.Sleep(delay + TimeSpan.FromMilliseconds(_random.Next(0, 1000)));
        }

        return false;
    }
}

