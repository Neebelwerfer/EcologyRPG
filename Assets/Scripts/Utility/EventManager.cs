using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static Dictionary<string, UnityEvent<object, object>> events = new Dictionary<string, UnityEvent<object, object>>();

    public static void AddListener(string eventName, UnityAction<object, object> listener)
    {
        UnityEvent<object, object> thisEvent = null;
        if (events.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<object, object>();
            thisEvent.AddListener(listener);
            events.Add(eventName, thisEvent);
        }
    }

    public static void RemoveListener(string eventName, UnityAction<object, object> listener)
    {
        if (events.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Dispatch(string eventName, object data, object sender)
    {
        if (events.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.Invoke(data, sender);
            Debug.Log("Event Dispatched: " + eventName);
        }
    }   
}