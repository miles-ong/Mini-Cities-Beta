using UnityEngine;

namespace MiniCities.Core.Grid
{
    public static class GridTransform
    {
        private const float CellSize = 1f;
        private static readonly Vector3 GridOffset = new(0.5f, 0, 0.5f);

        public static Vector3Int WorldToCell(Vector3 world) => Vector3Int.FloorToInt((world + GridOffset) / CellSize);
        public static Vector3 CellToWorld(Vector3Int cell) => (Vector3)cell * CellSize;
    }
}