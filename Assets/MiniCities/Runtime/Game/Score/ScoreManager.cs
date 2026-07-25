using System;
using MiniCities.Core.Score.Enums;

namespace MiniCities.Runtime.Game.Score
{
    public class ScoreManager
    {
        public int TargetPopulation { get; private set; }
        public int TargetCulture { get; private set; }
        public int TargetPollution { get; private set; }

        public int Population { get; private set; }
        public int Culture { get; private set; }
        public int Pollution { get; private set; }
        public int Money { get; private set; }

        public void ModifyScore(ScoreGroup score, Operation operation, float amount)
        {
            switch (score)
            {
                case ScoreGroup.Population: ModifyPopulation(operation, amount); break;
                case ScoreGroup.Culture:
                case ScoreGroup.Pollution:
                case ScoreGroup.Money:
                default:
                    throw new ArgumentOutOfRangeException(nameof(score),
                        $"{score} is not handled within {nameof(ModifyScore)}");
            }
            
            // fire event?
        }

        private void ModifyPopulation(Operation operation, float amount)
        {
            Population = (int)operation.Apply(Population, amount);
            
            // current test iteration of function
        }
    }
}