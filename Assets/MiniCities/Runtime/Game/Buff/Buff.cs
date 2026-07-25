using JetBrains.Annotations;
using MiniCities.Core.Score.Enums;
using UnityEngine;

namespace MiniCities.Runtime.Game.Buff
{
    public sealed class Buff
    {
        public IBuffSource Source { get; }
        [CanBeNull] public IBuffReceiver Receiver { get; }
        public Vector3Int? Location { get; }
        public ScoreGroup Score { get; }
        public Operation Operation { get; }
        public float Amount { get; }
        
        public bool IsAdditive => Operation is Operation.Add or Operation.Subtract;

        public Buff(
            IBuffSource source,
            ScoreGroup score,
            Operation operation,
            float amount,
            [CanBeNull] IBuffReceiver receiver = null,
            Vector3Int? location = null)
        {
            Source = source;
            Receiver = receiver;
            Location = location;
            Score = score;
            Operation = operation;
            Amount = amount;
        }
    }
}