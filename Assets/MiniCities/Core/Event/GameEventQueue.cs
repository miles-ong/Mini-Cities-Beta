using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniCities.Core.Event
{
    public static class GameEventQueue
    {
        public static event Action OnDrained;
        public static event Action OnDispatch;
        
        private const float MaxSpeed = 0.1f;
        
        private static readonly Queue<Action> Events = new();
        private static bool _isPumping;
        
        public static void Enqueue(Action dispatch) => Events.Enqueue(dispatch);

        public static IEnumerator Pump(float intervalSeconds = 0f, float speedRamp = 0f)
        {
            if (_isPumping) yield break;

            _isPumping = true;
            var currentInterval = intervalSeconds;
                
            while (Events.Count > 0)
            {
                var dispatch = Events.Dequeue();
                dispatch.Invoke();
                OnDispatch?.Invoke();

                if (currentInterval <= 0f) continue;
                
                currentInterval = Mathf.Max(currentInterval - speedRamp, MaxSpeed);
                    
                yield return new WaitForSeconds(currentInterval);
            }

            OnDrained?.Invoke();
            _isPumping = false;
        }
    }
}