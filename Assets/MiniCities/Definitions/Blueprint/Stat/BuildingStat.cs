using System;
using MiniCities.Systems.City.Stat;

namespace MiniCities.Definitions.Blueprint.Stat
{
    public enum BuildingStat
    {
        Population,
        Culture,
        Pollution
    }

    public static class BuildingStatExtensions
    {
        public static CityStat ToCityStat(this BuildingStat stat) => stat switch
        {
            BuildingStat.Population => CityStat.Population,
            BuildingStat.Culture => CityStat.Culture,
            BuildingStat.Pollution => CityStat.Pollution,
            _ => throw new ArgumentException($"{stat} not handled!")
        };
    }
}