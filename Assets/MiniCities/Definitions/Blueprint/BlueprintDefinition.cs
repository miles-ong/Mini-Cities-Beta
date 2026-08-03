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
        [SerializeReference] public IPlacementRule[] placementRules;
        public AbilityDefinition[] abilities;
        public BlockDefinition building;
    }
}