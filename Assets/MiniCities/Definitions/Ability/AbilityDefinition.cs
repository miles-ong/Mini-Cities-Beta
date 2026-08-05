using System;
using System.Linq;
using UnityEngine;

namespace MiniCities.Definitions.Ability
{
    [Serializable]
    public class AbilityDefinition
    {
        public AbilityTrigger trigger;
        [SerializeReference] public IAbilityCondition[] conditions;
        [SerializeReference] public IAbilityEffect[] effects;
    }
}