using System;

namespace MiniCities.Definitions.Blueprint.Stat
{
    [Serializable]
    public struct StatValue
    {
        public BuildingStat Stat;
        public int Value;
    }
}