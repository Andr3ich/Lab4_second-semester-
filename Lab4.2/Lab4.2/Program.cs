using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4_2
{
    public class PriorityEventArgs : EventArgs
    {
        public int Priority { get; }

        public PriorityEventArgs(int priority)
        {
            Priority = priority;
        }
    }

    public interface ISubscriber<T> where T : EventArgs
    {
        void HandleEvent(object sender, T args);
    }

    public class Publisher
    {
        private readonly Dictionary<int, List<object>> subscribersByPriority;

        public Publisher()
        {
            subscribersByPriority = new Dictionary<int, List<object>>();
        }

        public void Subscribe<T>(int priority, ISubscriber<T> subscriber) where T : PriorityEventArgs
        {
            if (!subscribersByPriority.ContainsKey(priority))
            {
                subscribersByPriority[priority] = new List<object>();
            }
            subscribersByPriority[priority].Add(subscriber);
        }

        public void Unsubscribe<T>(int priority, ISubscriber<T> subscriber) where T : PriorityEventArgs
        {
            if (subscribersByPriority.ContainsKey(priority))
            {
                subscribersByPriority[priority].Remove(subscriber);
            }
        }

        public void RaiseEvent(int priority)
        {
            var subscribers = subscribersByPriority.ContainsKey(priority) ? subscribersByPriority[priority] : new List<object>();

            var args = new PriorityEventArgs(priority);
            foreach (var subscriber in subscribers.OfType<ISubscriber<PriorityEventArgs>>())
            {
                subscriber.HandleEvent(this, args);
            }
        }
    }

    public class PrioritySubscriber<T> : ISubscriber<T> where T : PriorityEventArgs
    {
        private readonly int priority;

        public PrioritySubscriber(int priority)
        {
            this.priority = priority;
        }

        public void HandleEvent(object sender, T args)
        {
            Console.WriteLine($"Priority {priority} event handled by {GetType().Name}.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var publisher = new Publisher();

            var subscriber1 = new PrioritySubscriber<PriorityEventArgs>(1);
            var subscriber2 = new PrioritySubscriber<PriorityEventArgs>(2);
            var subscriber3 = new PrioritySubscriber<PriorityEventArgs>(3);

            publisher.Subscribe(1, subscriber1);
            publisher.Subscribe(2, subscriber2);
            publisher.Subscribe(3, subscriber3);

            publisher.RaiseEvent(2);

            publisher.RaiseEvent(1);

            publisher.RaiseEvent(3);

            publisher.Unsubscribe(1, subscriber1);
            publisher.Unsubscribe(2, subscriber2);
            publisher.Unsubscribe(3, subscriber3);
        }
    }
}



