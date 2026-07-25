using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace MiniCities.Core.Grid
{
    public readonly struct GridHit
    {
        public Vector3Int Cell { get; }
        public Vector3 WorldPosition { get; }
        public Vector3 Normal { get; }
        public bool IsTop => Vector3.Dot(Normal, Vector3.up) > 0.9f;

        public GridHit(Vector3Int cell, Vector3 worldPosition, Vector3 normal)
        {
            Cell = cell;
            WorldPosition = worldPosition;
            Normal = normal;
        }
    }
}