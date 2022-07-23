using System;
using System.Collections.Generic;

namespace Utilities.Patterns
{
    static class EventBusDispatcher<T> where T : IEvent
    {
        static readonly Dictionary<EventBus, Action<T>> Actions = new Dictionary<EventBus, Action<T>>();

        public static void Subscribe(EventBus bus, Action<T> listener)
        {
            if (!Actions.ContainsKey(bus))
            {
                Actions.Add(bus, delegate { });
            }

            Actions[bus] += listener;
        }

        public static void Unsubscribe(EventBus bus, Action<T> listener)
        {
            if (Actions.ContainsKey(bus))
            {
                Actions[bus] -= listener;
            }
        }

        public static void Dispatch(EventBus bus, T @event)
        {
            if (Actions.TryGetValue(bus, out var action))
            {
                action.Invoke(@event);
            }
        }
    }
}
