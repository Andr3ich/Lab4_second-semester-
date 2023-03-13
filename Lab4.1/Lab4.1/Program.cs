using System;
using System.Threading;

namespace Lab4_1
{
    public class EventLimitationBus
    {
        private readonly int maxEventsPerSecond;
        private readonly int eventInterval;
        private DateTime lastEventTime;
        private EventHandler eventHandlers;

        public EventLimitationBus(int maxEventsPerSecond)
        {
            this.maxEventsPerSecond = maxEventsPerSecond;
            this.eventInterval = 1000 / maxEventsPerSecond;
            this.lastEventTime = DateTime.MinValue;
            this.eventHandlers = null;
        }

        public void RegisterEventHandler(EventHandler handler)
        {
            eventHandlers += handler;
        }

        public void UnregisterEventHandler(EventHandler handler)
        {
            eventHandlers -= handler;
        }

        public void SendEvent(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var elapsedMilliseconds = (int)now.Subtract(lastEventTime).TotalMilliseconds;
            if (elapsedMilliseconds < eventInterval)
            {
                Thread.Sleep(eventInterval - elapsedMilliseconds);
            }
            lastEventTime = DateTime.Now;

            eventHandlers?.Invoke(sender, e);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bus = new EventLimitationBus(5); 

            bus.RegisterEventHandler((sender, e) =>
            {
                Console.WriteLine($"Event handled at {DateTime.Now}");
            });

            for (int i = 0; i < 10; i++)
            {
                bus.SendEvent(null, EventArgs.Empty);
            }

            Thread.Sleep(5000);
        }
    }
}
