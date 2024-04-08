using System;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Core
{
    public interface ITaskAction
    {
        public void Execute();
    }

    class Task : IComparable<Task>
    {
        public readonly object owner;
        public readonly Action action;
        public readonly float delay;
        public readonly bool repeat;
        public float timer;

        public Task(object owner, Action action, float delay, bool repeat)
        {
            this.owner = owner;
            this.action = action;
            this.delay = delay;
            this.timer = delay;
            this.repeat = repeat;
        }

        public int CompareTo(Task other)
        {
            if (timer < other.timer) return -1;
            if (timer > other.timer) return 1;
            return 0;
        }
    }

    public class TaskManager : IDisposable
    {
        public static TaskManager Instance;
        readonly List<Task> tasks;
        readonly List<Task> tasksToDelete;

        TaskManager() 
        { 
            tasks = new List<Task>();
            tasksToDelete = new List<Task>();
        }

        public static void Init()
        {
            Instance = new TaskManager();
        }

        void AddTask(object owner, Action action, float delay, bool repeat = false)
        {
            tasks.Add(new Task(owner, action, delay, repeat));
            tasks.Sort();
        }

        void RemoveTask(Action action)
        {
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                if (tasks[i].action == action)
                {
                    tasks.RemoveAt(i);
                }
            }
        }

        void RemoveAllTaskFromOwner(object owner)
        {
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                if (tasks[i].owner == owner)
                {
                    tasks.RemoveAt(i);
                }
            }
        }

        void OnUpdate()
        {
            if(tasksToDelete.Count > 0)
            {
                foreach (var task in tasksToDelete)
                {
                    tasks.Remove(task);
                }
                tasksToDelete.Clear();
                tasks.Sort();
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                task.timer -= Time.deltaTime;

                if(task.owner == null)
                {
                    tasksToDelete.Add(task);
                    continue;
                }

                if (task.timer <= 0)
                {
                    task.action();
                    if(task.repeat)
                    {
                        task.timer = task.delay;
                    }
                    else
                    {
                        tasksToDelete.Add(task);
                    }
                }
            }

        }

        public static void Add(object owner, Action action, float delay, bool repeat = false)
        {
            Instance.AddTask(owner, action, delay, repeat);
        }

        public static void Remove(Action action)
        {
            Instance.RemoveTask(action);
        }

        public static void RemoveAllFromOwner(object owner)
        {
            Instance.RemoveAllTaskFromOwner(owner);
        }

        public static void Update()
        {
            Instance.OnUpdate();
        }

        public void Dispose()
        {
            Instance = null;
            tasks.Clear();
        }
    }
}