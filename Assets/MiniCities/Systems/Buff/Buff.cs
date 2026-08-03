using JetBrains.Annotations;
using MiniCities.Core;
using MiniCities.Definitions.Blueprint;
using UnityEngine;

namespace MiniCities.Systems.BuffSys
{
    public sealed class Buff
    {
        public IBuffSource Source { get; }
        [CanBeNull] public IBuffReceiver Receiver { get; }
        public Vector3Int? Location { get; }
        public BuildingStat Stat { get; }
        public Operation Operation { get; }
        public float Amount { get; }

        public Buff(
            IBuffSource source,
            BuildingStat stat,
            Operation operation,
            float amount,
            [CanBeNull] IBuffReceiver receiver = null,
            Vector3Int? location = null)
        {
            Source = source;
            Receiver = receiver;
            Location = location;
            Stat = stat;
            Operation = operation;
            Amount = amount;
        }
    }
}