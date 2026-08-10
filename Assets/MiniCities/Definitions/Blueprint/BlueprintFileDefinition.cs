using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniCities.Definitions.Blueprint
{
    [CreateAssetMenu(fileName = "BPF_", menuName = "Mini Cities/File/Blueprint")]
    public class BlueprintFileDefinition : ScriptableObject
    {
        public string displayName = "Unnamed";
        public BlueprintPoolDefinition poolDefinition;
        public List<BlueprintCount> blueprintCounts = new();
    }

    [Serializable]
    public class BlueprintCount
    {
        public BlueprintDefinition blueprint;
        public int count;
    }
}