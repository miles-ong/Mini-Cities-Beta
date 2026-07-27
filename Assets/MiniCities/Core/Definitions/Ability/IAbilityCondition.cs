using System;
using MiniCities.Core.Definitions.Blueprint;
using MiniCities.Core.Grid;

namespace MiniCities.Core.Definitions.Ability
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