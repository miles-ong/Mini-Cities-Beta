using UnityEngine;

namespace _.Enums
{
    public enum BlockGroup
    {
        Land,
        Water
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