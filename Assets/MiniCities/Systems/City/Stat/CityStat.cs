using System;
using UnityEngine;

namespace MiniCities.Systems.City.Stat
{
    public enum CityStat
    {
        Population,
        Culture,
        Pollution,
        Money
    }

    public static class CityStatExtensions
    {
        public static Color32 ToColor(this CityStat stat) => stat switch
        {
            CityStat.Population => new Color32(129, 170, 228, 255),
            CityStat.Culture => new Color32(222, 138, 225, 255),
            CityStat.Pollution => new Color32(233, 104, 104, 255),
            CityStat.Money => new Color32(239, 234, 52, 255),
            _ => throw new ArgumentException($"{stat} not handled!")
        };
    }
}