using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MiniCities.Runtime.Ability;

namespace MiniCities.Definitions.Ability
{
    public static class AbilityDescriptionUtility
    {
        public static string Describe(AbilityRuntime runtime) =>
            Describe(runtime.Trigger, runtime.Conditions, runtime.Effects);
        public static string Describe(AbilityDefinition definition) =>
            Describe(definition.trigger, definition.conditions.ToList(), definition.effects.ToList());
        
        private static string Describe(
            AbilityTrigger trigger,
            [CanBeNull] List<IAbilityCondition> conditions,
            [CanBeNull] List<IAbilityEffect> effects)
        {
            var triggerPrefix = trigger switch
            {
                AbilityTrigger.WhenPlaced => "When placed",
                AbilityTrigger.Whenever => "Whenever",
                AbilityTrigger.While => "While",
                _ => trigger.ToString()
            };
            
            var effectsText = effects?.Count > 0
                ? string.Join(", ", effects.Select(e => e.Describe()))
                : "nothing happens";
            
            if (conditions == null || conditions.Count == 0)
                return $"{triggerPrefix}, {effectsText}.";
            
            var conditionsText = string.Join(" and ", conditions.Select(c => c.Describe()));
            
            return $"{triggerPrefix} {conditionsText}, {effectsText}.";
        }
    }
}