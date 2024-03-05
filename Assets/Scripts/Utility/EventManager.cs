using System.Collections.Generic;
using UnityEngine;
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

    public static Queue<DeferredEvent> updateEvents = new();
    public static Queue<DeferredEvent> fixedUpdateEvents = new();
    public static Queue<DeferredEvent> lateUpdateEvents = new();

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
            Debug.Log("Event Dispatched: " + eventName);
        }
    }

    public static void Dispatch(string eventName, object data, object sender)
    {
        if (events.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.Invoke(new DefaultEventData() { data = data, source = sender });
            Debug.Log("Event Dispatched: " + eventName);
        }
    }   

    public static void Defer(string eventName, EventData data, DeferredEventType deferredEventType)
    {
        if(deferredEventType == DeferredEventType.Update)
            updateEvents.Enqueue(new DeferredEvent() { data = data, eventName = eventName });
        else if(deferredEventType == DeferredEventType.FixedUpdate)
            fixedUpdateEvents.Enqueue(new DeferredEvent() { data = data, eventName = eventName });
        else if(deferredEventType == DeferredEventType.LateUpdate)
            lateUpdateEvents.Enqueue(new DeferredEvent() { data = data, eventName = eventName });
    }

    public static void UpdateQueue()
    {
        if(updateEvents.Count == 0) return;

        var e = updateEvents.Dequeue();
        Dispatch(e.eventName, e.data);
    }

    public static void FixedUpdateQueue()
    {
        if(fixedUpdateEvents.Count == 0) return;

        var e = fixedUpdateEvents.Dequeue();
        Dispatch(e.eventName, e.data);
    }

    public static void LateUpdateQueue()
    {
        if(lateUpdateEvents.Count == 0) return;

        var e = lateUpdateEvents.Dequeue();
        Dispatch(e.eventName, e.data);
    }
}