using MiniCities.Core.Definitions.Ability;
using MiniCities.Core.Definitions.Block;
using UnityEngine;

namespace MiniCities.Core.Definitions.Blueprint
{
    [CreateAssetMenu(fileName = "NewBlueprintDefinition", menuName = "Mini Cities/Blueprint/Normal")]
    public class BlueprintDefinition : ScriptableObject
    {
        public BlueprintFile file;
        public BlueprintCategory category;
        [SerializeReference] public IPlacementRule[] placementRules;
        public AbilityDefinition[] abilities;
    }

    [CreateAssetMenu(fileName = "NewBuildingBlueprintDefinition", menuName = "Mini Cities/Blueprint/Building")]
    public class BuildingBlueprintDefinition : BlueprintDefinition
    {
        public BlockDefinition building;
    }
}