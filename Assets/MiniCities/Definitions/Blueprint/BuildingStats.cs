using System;
using MiniCities.Core;

namespace MiniCities.Definitions.Blueprint
{
    public struct BuildingStats
    {
        public int Population;
        public int Culture;
        public int Pollution;
    }

    public static class BuildingStatsExtensions
    {
        public static ref int GetStat(this ref BuildingStats stats, BuildingStat stat)
        {
            switch (stat)
            {
                case BuildingStat.Population: return ref stats.Population;
                case BuildingStat.Culture: return ref stats.Culture;
                case BuildingStat.Pollution: return ref stats.Pollution;
                default: throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
            }
        }

        public static void ModifyStat(
            this ref BuildingStats stats, 
            BuildingStat stat, 
            Operation operation,
            float amount)
        {
            ref var value = ref stats.GetStat(stat);
            value = (int)operation.Apply(value, amount);
        }
    }
}