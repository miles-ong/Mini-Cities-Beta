using System;
using System.Collections.Generic;
using System.Linq;
using MiniCities.Core.Math;
using MiniCities.Definitions.Blueprint;

namespace MiniCities.Runtime.Blueprint
{
    public class BlueprintDeckRuntime
    {
        private readonly List<BlueprintRuntime> _blueprints = new();
        private readonly List<BlueprintRuntime> _drawPile = new();
        private readonly List<BlueprintRuntime> _discardPile = new();

        public IReadOnlyList<BlueprintRuntime> Blueprints => _blueprints;
        public IReadOnlyList<BlueprintRuntime> DrawPile => _drawPile;
        public IReadOnlyList<BlueprintRuntime> DiscardPile => _discardPile;

        private readonly SeededRandom _random;
        
        public BlueprintDeckRuntime(BlueprintDeckDefinition definition, SeededRandom random)
        {
            _random = random;
            
            foreach (var bc in definition.blueprintCounts)
            {
                for (var i = 0; i < bc.count; i++)
                {
                    var bp = new BlueprintRuntime(bc.blueprint);
                    _blueprints.Add(bp);
                }
            }
        }

        public void Add(BlueprintRuntime blueprint) => _blueprints.Add(blueprint);
        public void Remove(BlueprintRuntime blueprint) => _blueprints.Remove(blueprint);

        public BlueprintRuntime Draw()
        {
            if (_drawPile.Count == 0) throw new EmptyDrawPileException();

            var bp = _drawPile.First();
            _drawPile.RemoveAt(0);

            return bp;
        }

        public void Discard(BlueprintRuntime blueprint) => _discardPile.Add(blueprint);

        public void FillDrawPile()
        {
            _drawPile.Clear();
            _discardPile.Clear();

            foreach (var b in _blueprints) _drawPile.Add(b);

            Shuffle(_drawPile);
        }

        private void Shuffle(List<BlueprintRuntime> blueprints)
        {
            for (var i = blueprints.Count - 1; i > 0; i--)
            {
                var j = _random.Range(0, i + 1);
                
                (blueprints[i], blueprints[j]) = (blueprints[j], blueprints[i]);
            }
        }
    }

    public class EmptyDrawPileException : Exception
    {
        public EmptyDrawPileException()
        {
        }
    }
}