using System;
using System.Collections.Generic;

namespace MiniCities.Core.Deck
{
    public abstract class Deck<T>
    {
        private readonly List<T> _items = new();
        private readonly List<T> _drawPile = new();
        private readonly List<T> _discardPile = new();

        public IReadOnlyList<T> Items => _items;
        public int Count => _drawPile.Count;

        public void Add(T item) => _items.Add(item);

        public T Draw()
        {
            if (_drawPile.Count == 0) throw new EmptyDrawPileException();

            var item = _drawPile[0];
            _drawPile.RemoveAt(0);

            return item;
        }

        public void Discard(T item) => _discardPile.Add(item);

        public void FillDrawPile()
        {
            _drawPile.Clear();
            _discardPile.Clear();
            
            foreach (var item in _items) _drawPile.Add(item);

            Shuffle(_drawPile);
        }

        private static void Shuffle(List<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = UnityEngine.Random.Range(0, i + 1);
                
                (list[i], list[j]) = (list[j], list[i]);
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