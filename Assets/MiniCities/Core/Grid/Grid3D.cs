using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MiniCities.Core.Grid
{
    [Serializable]
    public abstract class Grid3D<T>
    {
        protected abstract Vector3Int GridDimensions { get; set; }
        protected abstract T[] GridItems { get; set; }
        
        public int Width => GridDimensions.x;
        public int Height => GridDimensions.y;
        public int Length => GridDimensions.z;

        public T this[Vector3Int coord]
        {
            get =>  GridItems[GetCheckedIndex(coord)];
            set => GridItems[GetCheckedIndex(coord)] = value;
        }
        
        public void Resize(Vector3Int newDimensions)
        {
            var resizedItems = new T[newDimensions.x * newDimensions.y * newDimensions.z];
            var copyWidth = Mathf.Min(Width, newDimensions.x);
            var copyHeight = Mathf.Min(Height, newDimensions.y);
            var copyLength = Mathf.Min(Length, newDimensions.z);

            for (var y = 0; y < copyHeight; y++)
            for (var z = 0; z < copyLength; z++)
            for (var x = 0; x < copyWidth; x++)
            {
                var coord = new Vector3Int(x, y, z);

                if (GridItems.Length > 0)
                    resizedItems[GetIndex(coord, newDimensions)] = GridItems[GetIndex(coord, GridDimensions)];
            }

            GridDimensions = newDimensions;
            GridItems = resizedItems;
        }

        public HashSet<T> GetItemsInScope(Vector3Int coord, GridScope scope, GridLevel level, bool includeSelf)
        {
            var items = scope switch
            {
                GridScope.Adjacent => GetAdjacent(coord, level),
                GridScope.Nearby => GetNearby(coord, level),
                GridScope.All => GridItems.ToHashSet(),
                GridScope.On => new HashSet<T>(),
                _ => throw new ArgumentOutOfRangeException(nameof(scope), $"{scope} is not supported")
            };

            if (scope == GridScope.On && InBounds(coord + level.ToVector()))
                items.Add(GridItems[GetIndex(coord + level.ToVector(),  GridDimensions)]);

            if (includeSelf && InBounds(coord)) items.Add(GridItems[GetIndex(coord, GridDimensions)]);

            return items;
        }

        private HashSet<T> GetAdjacent(Vector3Int coord, GridLevel level)
        {
            var left = coord + Vector3Int.left + level.ToVector();
            var forward = coord + Vector3Int.forward + level.ToVector();
            var right = coord + Vector3Int.right + level.ToVector();
            var back = coord + Vector3Int.back + level.ToVector();
            
            var adjacent = new HashSet<T>();

            if (InBounds(left)) adjacent.Add(GridItems[GetIndex(left, GridDimensions)]);
            if (InBounds(forward)) adjacent.Add(GridItems[GetIndex(forward, GridDimensions)]);
            if (InBounds(right)) adjacent.Add(GridItems[GetIndex(right, GridDimensions)]);
            if (InBounds(back)) adjacent.Add(GridItems[GetIndex(back, GridDimensions)]);

            return adjacent;
        }

        private HashSet<T> GetNearby(Vector3Int coord, GridLevel level)
        {
            var nearby = new HashSet<T>();
            
            for (var x = -1; x <= 1; x++)
            for (var z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                
                var pos = new Vector3Int(coord.x + x, coord.y, coord.z + z) + level.ToVector();

                if (InBounds(pos)) nearby.Add(GridItems[GetIndex(pos, GridDimensions)]);
            }

            return nearby;
        }
        
        private bool InBounds(Vector3Int coord)
            => coord.x >= 0 && coord.x < Width && coord.y >= 0 && coord.y < Height && coord.z >= 0 && coord.z < Length;
        
        private static int GetIndex(Vector3Int coord, Vector3Int dim) 
            => coord.x + (coord.z * dim.x) + (coord.y * dim.x * dim.z);
        
        private int GetCheckedIndex(Vector3Int coord)
        {
            if (!InBounds(coord))
                throw new ArgumentOutOfRangeException(nameof(coord),
                    $"{coord} is out of bounds for dimensions {GridDimensions}");

            return GetIndex(coord, GridDimensions);
        }
    }
}