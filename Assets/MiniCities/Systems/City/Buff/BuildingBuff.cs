using System;
using JetBrains.Annotations;
using MiniCities.Core.Math;
using MiniCities.Definitions.Blueprint.Stat;
using MiniCities.Runtime.Block;
using UnityEngine;

namespace MiniCities.Systems.City.Buff
{
    public sealed class BuildingBuff
    {
        public IBuffSource Source { get; }
        [CanBeNull] public BuildingRuntime Receiver { get; }
        public Vector3Int? Location { get; }
        public BuildingStat Stat { get; }
        public Operation Operation { get; }
        public float Amount { get; }
        [CanBeNull] public Predicate<BuildingRuntime> Condition { get; }

        public BuildingBuff(
            IBuffSource source,
            BuildingStat stat,
            Operation operation,
            float amount,
            Vector3Int location,
            [CanBeNull] Predicate<BuildingRuntime> condition = null)
        {
            Source = source;
            Location = location;
            Stat = stat;
            Operation = operation;
            Amount = amount;
            Condition = condition;
        }

        public BuildingBuff(
            IBuffSource source,
            BuildingStat stat,
            Operation operation,
            float amount,
            BuildingRuntime receiver,
            [CanBeNull] Predicate<BuildingRuntime> condition = null)
        {
            Source = source;
            Receiver = receiver;
            Stat = stat;
            Operation = operation;
            Amount = amount;
            Condition = condition;
        }
    }
}