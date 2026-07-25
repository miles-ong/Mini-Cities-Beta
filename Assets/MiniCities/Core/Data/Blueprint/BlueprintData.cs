using MiniCities.Core.Data.Ability;
using MiniCities.Core.Data.Blueprint.Enums;
using UnityEngine;

namespace MiniCities.Core.Data.Blueprint
{
    [CreateAssetMenu(fileName = "NewBlueprintData", menuName = "Mini Cities/Blueprint")]
    public class BlueprintData : ScriptableObject
    {
        public BlueprintFile file;
        public BlueprintCategory category;
        public AbilityDefinition[] abilities;
    }
}