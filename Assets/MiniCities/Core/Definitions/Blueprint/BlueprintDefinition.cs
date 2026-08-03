using MiniCities.Core.Definitions.Ability;
using MiniCities.Core.Definitions.Block;
using UnityEngine;

namespace MiniCities.Core.Definitions.Blueprint
{
    [CreateAssetMenu(fileName = "BLUEPRINT_", menuName = "Mini Cities/Blueprint")]
    public class BlueprintDefinition : ScriptableObject
    {
        public string displayName = "Unnamed";
        public BlueprintFile file;
        public BlueprintCategory category;
        [SerializeReference] public IPlacementRule[] placementRules;
        public AbilityDefinition[] abilities;
        public BlockDefinition building;
    }
}