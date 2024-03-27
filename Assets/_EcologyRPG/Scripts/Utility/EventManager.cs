using EcologyRPG.Utility.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Events;

public class EventData
{
    public object source;
}

public class DefaultEventData : EventData
{
    public object data;
}

public class DeferredEvent
{
    public EventData data;
    public string eventName;
}

public enum DeferredEventType
{
    Update,
    FixedUpdate,
    LateUpdate,
}

public static class EventManager
{
    public static Dictionary<string, UnityEvent<EventData>> events = new();
    public static PriorityQueue<DeferredEvent> deferredEvents = new();
    static Stopwatch stopwatch = new Stopwatch();

    public static void AddListener(string eventName, UnityAction<EventData> listener)
    {
        if (events.TryGetValue(eventName, out UnityEvent<EventData> thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<EventData>();
            thisEvent.AddListener(listener);
            events.Add(eventName, thisEvent);
        }
    }

    public static void RemoveListener(string eventName, UnityAction<EventData> listener)
    {
        if (events.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Dispatch(string eventName, EventData data)
    {
        if (events.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.Invoke(data);
            //Debug.Log("Event Dispatched: " + eventName);
        }
    }

    public static void Dispatch(string eventName, object data, object sender)
    {
        if (events.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.Invoke(new DefaultEventData() { data = data, source = sender });
            //Debug.Log("Event Dispatched: " + eventName);
        }
    }   

    /// <summary>
    /// Allows for deferring events to be dispatched at a later time (e.g. in the next update, fixed update, or late update).
    /// The event will be added to a queue and dispatched when it is next in line.
    /// Only use this if you need to defer the event, otherwise use Dispatch.
    /// Only works when game is in playing state.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="data"></param>
    /// <param name="deferredEventType"></param>
    public static void Defer(string eventName, EventData data, Priority priority = Priority.Normal)
    {
        deferredEvents.Enqueue(new DeferredEvent() { data = data, eventName = eventName }, priority);
    }

    public static void UpdateQueue()
    {
        if(deferredEvents.Count == 0) return;
        stopwatch.Reset();
        stopwatch.Start();

        while(deferredEvents.Count > 0 && stopwatch.Elapsed.TotalSeconds < 0.4f)
        {
            var e = deferredEvents.Dequeue();
            Dispatch(e.eventName, e.data);
        }
        stopwatch.Stop();
    }
}