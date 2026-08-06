using System;
using System.Collections.Generic;
using System.Linq;
using MiniCities.Core.Event;
using MiniCities.Definitions.Ability;
using MiniCities.Runtime.Ability;
using MiniCities.Runtime.Block;
using MiniCities.Systems.City.Stat;
using MiniCities.Systems.Game;
using UnityEngine;

namespace MiniCities.Systems.City.Ability
{
    public class AbilityDispatcher
    {
        private readonly struct Activation
        {
            public readonly BuildingRuntime Owner;
            public readonly int AbilityIndex;
            public readonly AbilityRuntime Ability;

            public Activation(BuildingRuntime owner, int abilityIndex, AbilityRuntime ability)
            {
                Owner = owner;
                AbilityIndex = abilityIndex;
                Ability = ability;
            }
        }

        private GameContext? _context;

        public void SetContext(GameContext context) => _context = context;

        public void QueueWhenPlaced(BuildingRuntime placed)
        {
            var batch = placed.GetAbilities(AbilityTriggerType.WhenPlaced)
                .Select(ia => new Activation(placed, ia.index, ia.ability))
                .ToList();

            Debug.Log($"Queueing {batch.Count} {AbilityTriggerType.WhenPlaced} abilities!");
            Queue(batch, (game, owner) => new AbilityContext(game, owner));
        }

        public void QueueWhenever(
            AbilityTriggerType type, 
            BuildingRuntime triggeringBuilding,
            IEnumerable<BuildingRuntime> activeBuildings)
        {
            var batch = new List<Activation>();

            foreach (var o in activeBuildings)
            foreach (var (i, a) in o.GetAbilities(type))
                if (a.Trigger.MatchesBuilding(triggeringBuilding)) batch.Add(new Activation(o, i, a));

            Debug.Log($"Queueing {batch.Count} {type} abilities!");
            Queue(batch, (game, owner) => AbilityContext.ForBuilding(game, owner, triggeringBuilding));
        }

        public void QueueCityStatReached(CityStat stat, float value, IEnumerable<BuildingRuntime> activeBuildings)
        {
            var batch = new List<Activation>();
            
            foreach (var o in activeBuildings)
            foreach (var (i, a) in o.GetAbilities(AbilityTriggerType.WheneverCityStatReached))
                if (a.Trigger.MatchesStat(stat, value)) batch.Add(new Activation(o, i, a));
            
            Queue(batch, (game, owner) => AbilityContext.ForCityStat(game, owner, stat, value));
        }

        private void Queue(List<Activation> batch, Func<GameContext, BuildingRuntime, AbilityContext> buildContext)
        {
            if (batch.Count == 0) return;

            if (_context == null) throw new InvalidOperationException($"{nameof(Queue)} called before {nameof(SetContext)}.");
            
            batch.Sort((a, b) =>
            {
                var byCategory = a.Owner.Blueprint.Category.CompareTo(b.Owner.Blueprint.Category);

                if (byCategory != 0) return byCategory;

                var byPlacement = a.Owner.PlacementOrder.CompareTo(b.Owner.PlacementOrder);

                return byPlacement != 0 ? byPlacement : a.AbilityIndex.CompareTo(b.AbilityIndex);
            });

            var game = _context.Value;

            foreach (var a in batch)
            {
                var ctx = buildContext(game, a.Owner);
                EventQueue.Enqueue(() => a.Ability.Activate(ctx));
            }
        }
    }
}