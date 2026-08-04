using UnityEngine;

namespace MiniCities.Definitions.Block
{
    public enum BlockGroup
    {
        Land,
        Water,
        Terrain,
        Building
    }

    public static class BlockGroupExtensions
    {
        public static Color ToColor(this BlockGroup group) => group switch
        {
            BlockGroup.Land => Color.seaGreen,
            BlockGroup.Water => Color.cornflowerBlue,
            _ => Color.gray
        };
    }
}