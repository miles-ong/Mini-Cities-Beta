using System;
using System.Collections.Generic;
using MiniCities.Definitions.Blueprint.Stat;

namespace MiniCities.Definitions.Blueprint
{
    public enum BlueprintFile
    {
        Modern
    }

    public static class BlueprintFileExtensions
    {
        private static readonly Dictionary<BlueprintFile, BuildingStat[]> Map = new()
        {
            { BlueprintFile.Modern, new[] { BuildingStat.Population, BuildingStat.Culture, BuildingStat.Pollution } }
        };

        public static BuildingStat[] GetRequiredStats(this BlueprintFile file) =>
            Map.TryGetValue(file, out var stats) ? stats : Array.Empty<BuildingStat>();
    }
}