using System.Linq;
using MiniCities.Definitions.Ability;
using MiniCities.Definitions.Block;
using UnityEngine;

namespace MiniCities.Definitions.Blueprint
{
    [CreateAssetMenu(fileName = "BLUEPRINT_", menuName = "Mini Cities/Blueprint")]
    public class BlueprintDefinition : ScriptableObject
    {
        public string displayName = "Unnamed";
        public BlueprintFile file;
        public BlueprintCategory category;
        public int population;
        public int culture;
        public int pollution;
        public BlockDefinition building;
        [SerializeReference] public IPlacementRule[] placementRules;
        public AbilityDefinition[] abilities;

        public string GenerateDescription()
        {
            var abilityText = abilities == null || abilities.Length == 0
                ? "No abilities."
                : string.Join("\n", abilities.Select(a => a.Describe()));

            if (placementRules == null || placementRules.Length == 0) return abilityText;

            return $"{abilityText} Must be placed {string.Join(" and ", placementRules.Select(a => a.Describe()))}.";
        }
    }
}