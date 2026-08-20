using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniCities.Definitions.Blueprint
{
    [CreateAssetMenu(fileName = "BPP_", menuName = "Mini Cities/Pool/Blueprint")]
    public class BlueprintPoolDefinition : ScriptableObject
    {
        public List<BlueprintDefinition> blueprints = new();

        public void Add(BlueprintDefinition blueprint)
        {
            if (!blueprints.Contains(blueprint)) blueprints.Add(blueprint);
        }

        public void Remove(BlueprintDefinition blueprint) => blueprints.Remove(blueprint);
        public bool Contains(BlueprintDefinition blueprint)  => blueprints.Contains(blueprint);

        public IReadOnlyList<BlueprintDefinition> GetByCategory(BlueprintCategory category) =>
            blueprints.Where(b => b.category == category).ToList();
    }
}