using MiniCities.Definitions.Block;
using MiniCities.Runtime.Blueprint;
using UnityEngine;

namespace MiniCities.Core.Event
{
    public interface IEvent
    {
    }

    public sealed class BlockPlacedEvent : IEvent // swap with building placed soon? // proof of concept
    {
        public Vector3Int Cell { get; }
        public BlockDefinition Block { get; }

        public BlockPlacedEvent(Vector3Int cell, BlockDefinition block)
        {
            Cell = cell;
            Block = block;
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