using System.Linq;
using MiniCities.Core.Grid;
using MiniCities.Definitions.Ability;
using MiniCities.Definitions.Block;
using MiniCities.Definitions.Blueprint.Placement;
using MiniCities.Definitions.Blueprint.Stat;
using UnityEngine;

namespace MiniCities.Definitions.Blueprint
{
    [CreateAssetMenu(fileName = "BP_", menuName = "Mini Cities/Blueprint")]
    public class BlueprintDefinition : ScriptableObject
    {
        public string displayName = "Unnamed";
        public BlueprintEra era;
        public BlueprintCategory category;
        public BlueprintRarity rarity;
        public BuildingStats stats = new();
        public BlockDefinition building;
        [SerializeReference] public IPlacementRule[] placementRules;
        public AbilityDefinition[] abilities;

        private void Reset()
        {
            placementRules = new IPlacementRule[]
            {
                new BlockGroupPlacementRule { scope = GridScope.On, group = BlockGroup.Land }
            };
        }

        public string GenerateDescription()
        {
            var abilityText = abilities == null || abilities.Length == 0
                ? "No abilities."
                : string.Join("\n", abilities.Select(AbilityDescriptionUtility.Describe));

            if (placementRules == null || placementRules.Length == 0) return abilityText;

            return $"{abilityText} Must be placed {string.Join(" and ", placementRules.Select(a => a.Describe()))}.";
        }
    }
}