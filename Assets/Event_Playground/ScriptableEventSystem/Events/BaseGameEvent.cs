using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameEvent<T> : ScriptableObject
{
    private readonly List<IGameEventListener<T>> eventListers = new List<IGameEventListener<T>>();

    public void Raise(T item) {
        for (int i = eventListers.Count - 1; i >= 0; i--)
        {
            eventListers[i].OnEventRaised(item);
        }
    }
    public void RegisterListener(IGameEventListener<T> listener)
    {
        if (!eventListers.Contains(listener))
        {
            eventListers.Add(listener);
        }
    }

    public void UnregisterListener(IGameEventListener<T> listener)
    {
        if (eventListers.Contains(listener))
        {
            eventListers.Remove(listener);
        }
    }
}
