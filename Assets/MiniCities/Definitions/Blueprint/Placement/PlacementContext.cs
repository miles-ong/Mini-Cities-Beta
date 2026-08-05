using MiniCities.Core.Grid;
using MiniCities.Systems.Map;

namespace MiniCities.Definitions.Blueprint.Placement
{
    public readonly struct PlacementContext
    {
        public GridHit Hit { get; }
        public MapManager Map { get; }

        public PlacementContext(GridHit hit, MapManager map)
        {
            Hit = hit;
            Map = map;
        }
    }
}