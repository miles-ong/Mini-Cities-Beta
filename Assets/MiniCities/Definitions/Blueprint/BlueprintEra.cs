using System;
using System.Collections.Generic;
using MiniCities.Definitions.Blueprint.Stat;

namespace MiniCities.Definitions.Blueprint
{
    public enum BlueprintEra
    {
        Modern
    }

    public static class BlueprintFileExtensions
    {
        private static readonly Dictionary<BlueprintEra, BuildingStat[]> Map = new()
        {
            { BlueprintEra.Modern, new[] { BuildingStat.Population, BuildingStat.Culture, BuildingStat.Pollution } }
        };

        public static BuildingStat[] GetRequiredStats(this BlueprintEra era) =>
            Map.TryGetValue(era, out var stats) ? stats : Array.Empty<BuildingStat>();
    }
}