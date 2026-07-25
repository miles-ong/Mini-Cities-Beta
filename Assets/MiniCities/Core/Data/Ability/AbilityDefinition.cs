using System;
using MiniCities.Core.Data.Ability.Conditions;
using MiniCities.Core.Data.Ability.Effects;
using MiniCities.Core.Data.Ability.Enums;
using UnityEngine;

namespace MiniCities.Core.Data.Ability
{
    [Serializable]
    public class AbilityDefinition
    {
        public AbilityTrigger trigger;
        [SerializeReference] public IAbilityCondition[] conditions;
        [SerializeReference] public IAbilityEffect[] effects;
    }
}