namespace Tests.CSharp.Homework3;

public class SingleInitializationSingleton
{
    public const int DefaultDelay = 3_000;
    private static readonly object Locker = new();
    private static Lazy<SingleInitializationSingleton> _instance = 
            new(() => new SingleInitializationSingleton());
    private static volatile bool _isInitialized = false;

    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        // imitation of complex initialization logic
        Thread.Sleep(delay);
    }

    public int Delay { get; }

    public static SingleInitializationSingleton Instance => _instance.Value;

    internal static void Reset()
    {
        if(!_isInitialized) return;

        lock (Locker)
        {
            if (!_isInitialized) return;
            
            _instance = new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton());
            _isInitialized = false;
        }
    }

    public static void Initialize(int delay)
    {
        if (!_isInitialized)
        {
            lock (Locker)
            {
                if (!_isInitialized)
                {
                    _instance = new (() => new SingleInitializationSingleton(delay));
                    _isInitialized = true;
                    return;
                }
            }
        }
        throw new InvalidOperationException();
    }
}