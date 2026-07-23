using System;
using _.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Scriptable Objects/MapData")]
public class MapData : ScriptableObject
{
    public MapSize size;
    public int terrainFactor;
    public Vector3Int dimensions;
    public BlockData[] layout;

    public int Width => dimensions.x;
    public int Height => dimensions.y;
    public int Length => dimensions.z;

    public BlockData this[Vector3Int coord]
    {
        get
        {
            CheckBounds(coord);
            return layout[GetIndex(coord)];
        }
        set
        {
            CheckBounds(coord);
            layout[GetIndex(coord)] = value;
        }
    }

    private int GetIndex(Vector3Int coord) => coord.x + (coord.z * Width) + (coord.y * Width * Length);

    private void CheckBounds(Vector3Int coord)
    {
        var x = coord.x;
        var y = coord.y;
        var z = coord.z;

        if (x < 0 || x >= Width || y < 0 || y >= Height || z < 0 || z >= Length)
            throw new ArgumentException($"{coord} are out of bounds of the specified dimensions ({dimensions})");
    }
}
