using System;
using System.Collections.Generic;

public interface IEvent{}

public static class EventSystem<T> where T : IEvent
{
    private static Action<T> _action;
    private static readonly HashSet<Action<T>> Actions = new();

    public static void Subscribe(Action<T> action)
    {
        if (Actions.Add(action))
        {
            _action += action;
        }
        else throw new InvalidOperationException("Already subscribed!");
    }

    public static void Unsubscribe(Action<T> action)
    {
        if (Actions.Remove(action))
        {
            _action -= action;
        }
    }

    public static void Invoke(T value)
    {
        _action?.Invoke(value);
    }
    
}