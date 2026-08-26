using System.Collections.Generic;
using System.Linq;
using MiniCities.Definitions.Ability;
using MiniCities.Definitions.Block;
using MiniCities.Definitions.Blueprint;
using MiniCities.Definitions.Blueprint.Placement;
using MiniCities.Definitions.Blueprint.Stat;
using MiniCities.Runtime.Ability;
using Unity.Properties;
using UnityEngine.UIElements;

namespace MiniCities.Runtime.Blueprint
{
    public class BlueprintRuntime
    {
        [CreateProperty] public readonly string DisplayName;
        public readonly BlueprintEra Era;
        public BlueprintCategory Category;
        public BlueprintRarity Rarity;
        public readonly BuildingStats Stats;
        public readonly BlockDefinition Building;
        public readonly List<IPlacementRule> PlacementRules;
        public readonly List<AbilityRuntime> Abilities;
        [CreateProperty] public string Description { get; private set; }
        [CreateProperty] public int Population => Stats.Get(BuildingStat.Population);
        [CreateProperty] public int Culture => Stats.Get(BuildingStat.Culture);
        [CreateProperty] public int Pollution => Stats.Get(BuildingStat.Pollution);
        [CreateProperty] public bool DisplayPopulation => Population > 0;
        [CreateProperty] public bool DisplayCulture => Culture > 0;
        [CreateProperty] public bool DisplayPollution => Pollution > 0;
        
        public BlueprintRuntime(BlueprintDefinition definition)
        {
            DisplayName = definition.displayName;
            Era = definition.era;
            Category = definition.category;
            Rarity = definition.rarity;
            Stats = definition.stats.Clone();
            Building = definition.building;
            PlacementRules = definition.placementRules.ToList();
            Abilities = definition.abilities.Select(a => new AbilityRuntime(a)).ToList();
            Description = GenerateDescription();
        }

        private string GenerateDescription()
        {
            var abilityText = Abilities == null || Abilities.Count == 0
                ? "No abilities."
                : string.Join("\n", Abilities.Select(AbilityUtility.Describe));

            if (PlacementRules == null || PlacementRules.Count == 0) return abilityText;

            return $"{abilityText} Must be placed {string.Join(" and ", PlacementRules.Select(a => a.Describe()))}.";
        }
    }
}