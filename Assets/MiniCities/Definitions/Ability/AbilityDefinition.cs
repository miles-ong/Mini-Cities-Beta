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

        public string Describe()
        {
            var triggerPrefix = trigger switch
            {
                AbilityTrigger.WhenPlaced => "When placed",
                AbilityTrigger.Whenever => "Whenever",
                AbilityTrigger.While => "While",
                _ => trigger.ToString()
            };

            var effectsText = effects?.Length > 0
                ? string.Join(", ", effects.Select(e => e.Describe()))
                : "nothing happens";

            if (conditions == null || conditions.Length == 0)
                return $"{triggerPrefix}, {effectsText}.";

            var conditionsText = string.Join(" and ", conditions.Select(c => c.Describe()));

            return $"{triggerPrefix} {conditionsText}, {effectsText}.";
        }
    }
}