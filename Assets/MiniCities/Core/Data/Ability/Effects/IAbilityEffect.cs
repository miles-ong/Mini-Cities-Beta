using System;
using MiniCities.Core.Score.Enums;

namespace MiniCities.Core.Data.Ability.Effects
{
    public interface IAbilityEffect
    {
        public void Activate(AbilityContext ctx);
        public IAbilityEffect Clone();
    }

    [Serializable]
    public class ModifyScoreEffect : IAbilityEffect
    {
        public ScoreGroup targetScore;
        public Operation operation;
        public float amount;
        
        public void Activate(AbilityContext ctx)
        {
            ctx.Game.Score.ModifyScore(targetScore, operation, amount);
        }

        public IAbilityEffect Clone() => new ModifyScoreEffect()
        {
            targetScore = targetScore,
            operation = operation,
            amount = amount
        };
    }
}