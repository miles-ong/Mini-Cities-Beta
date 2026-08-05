using MiniCities.Runtime.Block;
using MiniCities.Runtime.Blueprint;
using UnityEngine;

namespace MiniCities.Core.Event
{
    public interface IEvent
    {
    }

    public sealed class BlueprintPlacedEvent : IEvent 
    {
        public BlueprintRuntime Blueprint { get; }
        public Vector3Int Location { get; }

        public BlueprintPlacedEvent(BlueprintRuntime blueprint, Vector3Int location)
        {
            Blueprint = blueprint;
            Location = location;
        }
    }

    public sealed class BlueprintSelectedEvent : IEvent
    {
        public BlueprintRuntime Blueprint { get; }

        public BlueprintSelectedEvent(BlueprintRuntime blueprint)
        {
            Blueprint = blueprint;
        }
    }
}