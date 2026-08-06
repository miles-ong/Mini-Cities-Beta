using MiniCities.Runtime.Block;
using MiniCities.Runtime.Blueprint;
using UnityEngine;

namespace MiniCities.Core.Event
{
    public interface IEvent
    {
    }
    
    #region Blueprint Events
    public sealed class BlueprintUsedEvent : IEvent 
    {
        public BlueprintRuntime Blueprint { get; }
        public Vector3Int Location { get; }

        public BlueprintUsedEvent(BlueprintRuntime blueprint, Vector3Int location)
        {
            Blueprint = blueprint;
            Location = location;
        }
    }

    public sealed class BlueprintSelectedEvent : IEvent
    {
        public BlueprintRuntime Blueprint { get; }

        public BlueprintSelectedEvent(BlueprintRuntime blueprint) => Blueprint = blueprint;
    }

    public sealed class BlueprintDrawnEvent : IEvent
    {
        public BlueprintRuntime Blueprint { get; }

        public BlueprintDrawnEvent(BlueprintRuntime blueprint) => Blueprint = blueprint;
    }
    #endregion

    #region Building Events

    public sealed class BuildingPlacedEvent : IEvent
    {
        public BuildingRuntime Building { get; }

        public BuildingPlacedEvent(BuildingRuntime building) => Building = building;
    }

    public sealed class BuildingRemovedEvent : IEvent
    {
        public BuildingRuntime Building { get; }
        
        public BuildingRemovedEvent(BuildingRuntime building) => Building = building;
    }

    #endregion
}