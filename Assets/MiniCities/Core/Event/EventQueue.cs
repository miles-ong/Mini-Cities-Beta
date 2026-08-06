using System;
using System.Collections.Generic;

namespace MiniCities.Core.Event
{
    public static class EventQueue
    {
        public static event Action OnDrained;
        
        private static readonly Queue<Action> _queue = new();
        private static bool _isPumping;

        public static void Enqueue(Action dispatch) => _queue.Enqueue(dispatch);

        public static void Pump()
        {
            if (_isPumping) return;

            _isPumping = true;

            try
            {
                while (_queue.Count > 0)
                {
                    var dispatch = _queue.Dequeue();
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