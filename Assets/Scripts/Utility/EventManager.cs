using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>();

    public static void AddListener(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (events.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            events.Add(eventName, thisEvent);
        }
    }

    public static void RemoveListener(string eventName, UnityAction listener)
    {
        if (events.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Dispatch(string eventName)
    {
        if (events.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.Invoke();
            Debug.Log("Event Dispatched: " + eventName);
        }
    }   
}