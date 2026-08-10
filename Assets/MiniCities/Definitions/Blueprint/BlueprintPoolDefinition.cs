using System.Collections.Generic;
using System.Linq;
using MiniCities.Core.Pool;
using UnityEngine;

namespace MiniCities.Definitions.Blueprint
{
    [CreateAssetMenu(fileName = "BPP_", menuName = "Mini Cities/Pool/Blueprint")]
    public class BlueprintPoolDefinition : ScriptableObject
    {
        public Pool<BlueprintDefinition> blueprints;

        public IReadOnlyList<BlueprintDefinition> GetByCategory(BlueprintCategory category) =>
            blueprints.Items.Where(b => b.category == category).ToList();
    }
}