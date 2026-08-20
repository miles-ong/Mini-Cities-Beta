using System;
using System.Collections.Generic;
using System.Linq;
using MiniCities.Core.Event;
using MiniCities.Definitions.Ability;
using MiniCities.Runtime.Ability;
using MiniCities.Runtime.Block;
using MiniCities.Systems.Game;
using UnityEngine;

namespace MiniCities.Systems.City.Ability
{
    public class AbilityDispatcher
    {
        private readonly struct Activation
        {
            public readonly IAbilityActivator Activator;
            public readonly int Index;
            public readonly AbilityRuntime Ability;

            public Activation(
                IAbilityActivator activator, 
                int index,
                AbilityRuntime ability)
            {
                Activator = activator;
                Index = index;
                Ability = ability;
            }
        }

        private GameSessionContext _context;

        public void SetContext(GameSessionContext context) => _context = context;

        public void QueueWhenPlaced(BuildingRuntime placed)
        {
            var activations = placed.GetAbilities(AbilityTriggerType.WhenPlaced)
                .Select(ia => new Activation(placed, ia.index, ia.ability))
                .ToList();
            
            Debug.Log($"Queueing {activations.Count} {AbilityTriggerType.WhenPlaced} abilities!");
            QueueAbilities(activations, (game, owner) => new AbilityContext(game, owner));
        }

        public void QueueWheneverBuilding(
            AbilityTriggerType type,
            BuildingRuntime placed,
            IEnumerable<IAbilityActivator> activeActivators)
        {
            var activations = new List<Activation>();

            foreach (var activator in activeActivators)
            {
                if (activator is BuildingRuntime building && building == placed) continue;
                
                foreach (var (index, ability) in activator.GetAbilities(type))
                    if (ability.Trigger.MatchesBuilding(placed)) 
                        activations.Add(new Activation(activator, index, ability));
            }
            
            Debug.Log($"Queueing {activations.Count} {type} abilities!");
            QueueAbilities(activations, (game, owner) => AbilityContext.TriggeredByBuilding(game, owner, placed));
        }

        private void QueueAbilities(
            List<Activation> activations,
            Func<GameSessionContext, IAbilityActivator, AbilityContext> contextBuilder)
        {
            if (activations.Count == 0) return;

            if (_context == null)
                throw new InvalidOperationException($"{nameof(QueueAbilities)} called before {nameof(SetContext)}!");

            var sortedActivations = activations
                .OrderBy(a => a.Activator.GetPriorityValue())
                .ThenBy(a => a.Index);

            foreach (var activation in sortedActivations)
            {
                var context = contextBuilder(_context, activation.Activator);
                GameEventQueue.Enqueue(() => activation.Ability.Activate(context));
            }
        }

        /*public void QueueCityStatReached(CityStat stat, float value, IEnumerable<BuildingRuntime> activeBuildings)
        {
            var batch = new List<Activation>();

            foreach (var o in activeBuildings)
            foreach (var (i, a) in o.GetAbilities(AbilityTriggerType.WheneverCityStatReached))
                if (a.Trigger.MatchesStat(stat, value)) batch.Add(new Activation(o, i, a));

            QueueAbilities(batch, (game, owner) => AbilityContext.ForCityStat(game, owner, stat, value));
        }*/
    }
}