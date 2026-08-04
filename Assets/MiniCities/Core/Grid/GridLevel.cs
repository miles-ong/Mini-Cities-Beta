using UnityEngine;

namespace MiniCities.Core.Grid
{
    public enum GridLevel
    {
        Below,
        Level,
        Above
    }

    public static class GridLevelExtensions
    {
        public static Vector3Int ToVector(this GridLevel level) => level switch
        {
            GridLevel.Below => Vector3Int.down,
            GridLevel.Level => Vector3Int.zero,
            GridLevel.Above => Vector3Int.up,
            _ => Vector3Int.zero
        };
    }
}