using System;
using MiniCities.Core.Math;

namespace MiniCities.Systems.City.Stat
{
    public struct CityStats
    {
        public int Population;
        public int Culture;
        public int Pollution;
        public int Money;

        public override string ToString() =>
            $"Population: {Population} Culture: {Culture} Pollution: {Pollution} Money: {Money}";
    }

    public static class CityStatsExtensions
    {
        public static ref int GetStat(this ref CityStats stats, CityStat stat)
        {
            switch (stat)
            {
                case CityStat.Population: return ref stats.Population;
                case CityStat.Culture: return ref stats.Culture;
                case CityStat.Pollution: return ref stats.Pollution;
                case CityStat.Money: return ref stats.Money;
                default: throw new ArgumentOutOfRangeException(nameof(stat), "Not handled");
            }
        }

        public static void ModifyStat(
            this ref CityStats stats,
            CityStat stat,
            Operation operation,
            float amount)
        {
            ref var value = ref stats.GetStat(stat);
            value = (int)operation.Apply(value, amount);
        }
    }
}