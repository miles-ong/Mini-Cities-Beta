using System;
using System.Collections.Generic;

namespace MiniCities.Core.Event
{
    public static class EventQueue
    {
        public static event Action OnDrained;
        
        private static readonly Queue<Action> Events = new();
        private static bool _isPumping;

        public static void Enqueue(Action dispatch) => Events.Enqueue(dispatch);

        public static void Pump()
        {
            if (_isPumping) return;

            _isPumping = true;

            try
            {
                while (Events.Count > 0)
                {
                    var dispatch = Events.Dequeue();
                    dispatch.Invoke();
                }

                OnDrained?.Invoke();
            }
            finally
            {
                _isPumping = false;
            }
        }
    }
}