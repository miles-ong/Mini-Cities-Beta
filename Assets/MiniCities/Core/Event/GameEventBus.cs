using System;
using JetBrains.Annotations;

namespace MiniCities.Core.Event
{
    public static class GameEventBus<T> where T : IEvent
    {
        [CanBeNull] public static event Action<T> OnEvent;

        public static void Fire(T evt) => OnEvent?.Invoke(evt);
    }
}