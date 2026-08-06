using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MiniCities.Definitions.Ability.Trigger;
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
            var triggerPrefix = trigger.type switch
            {
                AbilityTriggerType.WhenPlaced => "When placed",
                AbilityTriggerType.Continuous => conditions?.Count > 0 ? "While" : "While active",
                AbilityTriggerType.WheneverBuildingPlaced => DescribeBuildingWhenever("placed", trigger),
                AbilityTriggerType.WheneverBuildingRemoved => DescribeBuildingWhenever("destroyed", trigger),
                AbilityTriggerType.WheneverBuildingMoved => DescribeBuildingWhenever("moved", trigger),
                AbilityTriggerType.WheneverCityStatReached =>
                    $"Whenever {trigger.stat} {trigger.comparison} {trigger.threshold}",
                _ => trigger.type.ToString()
            };
            
            var effectsText = effects?.Count > 0
                ? string.Join(", ", effects.Select(e => e.Describe()))
                : "nothing happens";
            
            if (conditions == null || conditions.Count == 0)
                return $"{triggerPrefix}, {effectsText}.";
            
            var conditionsText = string.Join(" and ", conditions.Select(c => c.Describe()));
            
            return $"{triggerPrefix} {conditionsText}, {effectsText}.";
        }

        private static string DescribeBuildingWhenever(string action, AbilityTrigger trigger)
        {
            if (trigger.filterByCategory) return $"Whenever a {trigger.category} building is {action}";
            if (trigger.filterByBlock) return $"Whenever a {trigger.block?.displayName} is {action}";

            return $"Whenever a building is {action}";
        }
    }
}