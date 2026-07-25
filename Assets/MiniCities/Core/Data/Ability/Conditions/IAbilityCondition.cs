using System;
using MiniCities.Core.Data.Blueprint.Enums;
using MiniCities.Core.Grid.Enums;

namespace MiniCities.Core.Data.Ability.Conditions
{
    public interface IAbilityCondition
    {
        public bool Evaluate(AbilityContext ctx);
        public IAbilityCondition Clone();
    }
    
    [Serializable]
    public sealed class BlueprintCategoryInRange : IAbilityCondition
    {
        public BlueprintCategory category;
        public GridScope scope;
        public bool includeSelf;

        public bool Evaluate(AbilityContext ctx)
        {
            throw new NotImplementedException();
        }

        public IAbilityCondition Clone() => new BlueprintCategoryInRange()
        {
            category = category,
            scope = scope,
            includeSelf = includeSelf,
        };
    }
}