using System;
using MiniCities.Definitions.Ability.Trigger;
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