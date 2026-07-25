using System;
using MiniCities.Core.Data.Block;
using MiniCities.Core.Data.Blueprint.Enums;
using MiniCities.Core.Event;
using MiniCities.Core.Grid.Enums;
using MiniCities.Runtime.Game;

namespace _.Core.Data.Ability.Conditions
{
    public abstract class AbilityConditionData
    {
        public abstract bool Evaluate(GameContext ctx, IEvent evt, BlockInstance owner);
    }
    
    [Serializable]
    public sealed class BlueprintCategoryInRange : AbilityConditionData
    {
        public BlueprintCategory category;
        public GridScope scope;
        public bool includeSelf;

        public override bool Evaluate(GameContext ctx, IEvent evt, BlockInstance owner)
        {
            throw new NotImplementedException();
        }
    }
}