using System;
using UnityEngine;

namespace MiniCities.Core.Grid
{
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
        
        public bool InBounds(Vector3Int coord)
            => coord.x >= 0 && coord.x < Width && coord.y >= 0 && coord.y < Height && coord.z >= 0 && coord.z < Length;
        
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