using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniCities.Definitions.Blueprint
{
    [CreateAssetMenu(fileName = "NewBlueprintDeck", menuName = "Mini Cities/Deck/Blueprint")]
    public class BlueprintDeckDefinition : ScriptableObject
    {
        public BlueprintPool pool;
        public List<BlueprintCount> blueprintCounts = new();
    }

    [Serializable]
    public class BlueprintCount
    {
        public BlueprintDefinition blueprint;
        public int count;
    }
}