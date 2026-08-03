using System.Collections.Generic;
using System.Linq;
using MiniCities.Core.Pool;
using UnityEngine;

namespace MiniCities.Core.Definitions.Blueprint
{
    [CreateAssetMenu(fileName = "NewBlueprintPool", menuName = "Mini Cities/Pool/Blueprint")]
    public class BlueprintPool : ScriptableObject
    {
        public Pool<BlueprintDefinition> blueprints = new();

        public IReadOnlyList<BlueprintDefinition> GetByCategory(BlueprintCategory category) =>
            blueprints.Items.Where(b => b.category == category).ToList();
    }
}