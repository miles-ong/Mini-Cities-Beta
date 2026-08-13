using System;
using UnityEngine;

namespace MiniCities.Definitions.Blueprint
{
    public enum BlueprintCategory
    {
        Residential,
        Commercial,
        Industrial,
        Civic,
        Initiative,
        Disaster
    }

    public static class BlueprintCategoryExtensions
    {
        public static Color32 ToColor(this BlueprintCategory category) => category switch
        {
            BlueprintCategory.Residential => new Color32(0, 217, 124, 255),
            BlueprintCategory.Commercial => new Color32(0, 147, 255, 255),
            BlueprintCategory.Industrial => new Color32(243, 135, 0, 255),
            BlueprintCategory.Civic => new Color32(121, 136, 151, 255),
            BlueprintCategory.Initiative => new Color32(217, 56, 157, 255),
            BlueprintCategory.Disaster => new Color32(255, 77, 75, 255),
            _ => throw new ArgumentException($"{category} not handled!")
        };
    }
}