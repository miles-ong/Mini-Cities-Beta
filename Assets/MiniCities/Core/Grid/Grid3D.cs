using System;
using System.Collections.Generic;
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

        public List<Vector3Int> GetLocationsInScope(
            Vector3Int coord, 
            GridScope scope, 
            GridLevel level,
            bool includeSelf)
        {
            var leveled = coord + level.ToVector();

            if (!InBounds(leveled)) return new List<Vector3Int>();
            
            List<Vector3Int> locations;

            switch (scope)
            {
                case GridScope.Adjacent:
                    locations = new List<Vector3Int>(4)
                    {
                        leveled + Vector3Int.left,
                        leveled + Vector3Int.forward,
                        leveled + Vector3Int.right,
                        leveled + Vector3Int.back
                    };
                    
                    if (includeSelf) locations.Add(leveled);
                    
                    break;
                
                case GridScope.Nearby:
                    locations = new List<Vector3Int>(8)
                    {
                        leveled + Vector3Int.left,
                        leveled + Vector3Int.left + Vector3Int.forward,
                        leveled + Vector3Int.forward,
                        leveled + Vector3Int.forward + Vector3Int.right,
                        leveled + Vector3Int.right,
                        leveled + Vector3Int.right + Vector3Int.back,
                        leveled + Vector3Int.back,
                        leveled + Vector3Int.back + Vector3Int.left
                    };
                    
                    if (includeSelf) locations.Add(leveled);
                    
                    break;
                
                case GridScope.All:
                    locations = new List<Vector3Int>(Width * Height);
                    
                    for (var z = 0; z < Length; z++)
                    for (var x = 0; x < Width; x++)
                        locations.Add(new Vector3Int(x, leveled.y, z));
                    
                    break;
                
                case GridScope.On:
                    locations = new List<Vector3Int> { leveled };
                    break;
                
                default:
                    throw new ArgumentException($"{scope} not handled!");
            }

            var inBounds = new List<Vector3Int>();

            foreach (var location in locations)
                if (InBounds(location)) inBounds.Add(location);

            return inBounds;
        }

        public HashSet<T> GetItemsInScope(
            Vector3Int coord, 
            GridScope scope, 
            GridLevel level, 
            bool includeSelf)
        {
            var locations = GetLocationsInScope(coord, scope, level, includeSelf);
            var items = new HashSet<T>();

            foreach (var location in locations)
            {
                var item = GridItems[GetIndex(location, GridDimensions)];

                if (item != null) items.Add(item);
            }

            return items;
        }
        
        private bool InBounds(Vector3Int coord)
            => coord.x >= 0 && coord.x < Width && coord.y >= 0 && coord.y < Height && coord.z >= 0 && coord.z < Length;
        
        private int GetCheckedIndex(Vector3Int coord)
        {
            if (!InBounds(coord))
                throw new ArgumentOutOfRangeException(nameof(coord),
                    $"Out of bounds for dimensions {GridDimensions}");

            return GetIndex(coord, GridDimensions);
        }
        
        private static int GetIndex(Vector3Int coord, Vector3Int dim) 
            => coord.x + (coord.z * dim.x) + (coord.y * dim.x * dim.z);
    }
}