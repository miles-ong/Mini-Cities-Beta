using System;
using UnityEngine;

namespace MiniCities.Core.Definitions.Ability
{
    [Serializable]
    public class AbilityDefinition
    {
        public AbilityTrigger trigger;
        [SerializeReference] public IAbilityCondition[] conditions;
        [SerializeReference] public IAbilityEffect[] effects;
    }
}