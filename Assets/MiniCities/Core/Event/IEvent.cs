using MiniCities.Runtime.Block;
using MiniCities.Runtime.Blueprint;
using MiniCities.Systems.Game;
using UnityEngine;

namespace MiniCities.Core.Event
{
    public interface IEvent
    {
    }
    
    #region Game Events

    public readonly struct LevelStartedEvent : IEvent
    {
        public LevelContext Level { get; }

        public LevelStartedEvent(LevelContext level) => Level = level;
    }

    public readonly struct LevelCompletedEvent : IEvent
    {
    }

    public readonly struct LevelFailedEvent : IEvent
    {
    }

    public readonly struct EnterShopEvent : IEvent
    {
    }

    public readonly struct EnterLevelSelectorEvent : IEvent
    {
    }

    #endregion
    
    #region Blueprint Events
    
    public readonly struct BlueprintUsedEvent : IEvent 
    {
        public BlueprintRuntime Blueprint { get; }
        public Vector3Int Location { get; }

        public BlueprintUsedEvent(BlueprintRuntime blueprint, Vector3Int location)
        {
            Blueprint = blueprint;
            Location = location;
        }
    }

    public readonly struct BlueprintSelectedEvent : IEvent
    {
        public BlueprintRuntime Blueprint { get; }

        public BlueprintSelectedEvent(BlueprintRuntime blueprint) => Blueprint = blueprint;
    }

    public readonly struct BlueprintDrawnEvent : IEvent
    {
        public BlueprintRuntime Blueprint { get; }

        public BlueprintDrawnEvent(BlueprintRuntime blueprint) => Blueprint = blueprint;
    }
    
    #endregion

    #region Building Events

    public readonly struct BuildingPlacedEvent : IEvent
    {
        public BuildingRuntime Building { get; }

        public BuildingPlacedEvent(BuildingRuntime building) => Building = building;
    }

    public readonly struct BuildingRemovedEvent : IEvent
    {
        public BuildingRuntime Building { get; }
        
        public BuildingRemovedEvent(BuildingRuntime building) => Building = building;
    }

    public readonly struct BuildingMovedEvent : IEvent
    {
        public BuildingRuntime Building { get; }
        public Vector3Int To { get; }
        public Vector3Int From { get; }

        public BuildingMovedEvent(BuildingRuntime building, Vector3Int to, Vector3Int from)
        {
            Building = building;
            To = to;
            From = from;
        }
    }

    #endregion
    
    #region Camera Events

    public readonly struct CameraRotationStartedEvent : IEvent
    {
    }

    public readonly struct CameraRotationEndedEvent : IEvent
    {
    }

    #endregion
}