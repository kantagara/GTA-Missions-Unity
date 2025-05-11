using System;

public interface IEvent
{
}

public static class EventSystem<T> where T : IEvent
{
    private static Action<T> _action;

    public static void Subscribe(Action<T> action)
    {
        _action += action;
    }

    public static void Unsubscribe(Action<T> action)
    {
        _action -= action;
    }

    public static void Invoke(T value)
    {
        _action?.Invoke(value);
    }
}