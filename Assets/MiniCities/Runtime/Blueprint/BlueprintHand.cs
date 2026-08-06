using System;
using System.Collections;
using System.Collections.Generic;
using MiniCities.Core.Event;
using UnityEngine;

namespace MiniCities.Runtime.Blueprint
{
    public class BlueprintHand : MonoBehaviour
    {
        [SerializeField] private int maxHandSize;
        [SerializeField] private float drawSpeed;

        private BlueprintDeckRuntime _deck;
        private readonly List<BlueprintRuntime> _hand = new();

        private void Start()
        {
            EventBus<BlueprintUsedEvent>.OnEvent += OnBlueprintUsed;
        }

        public void LoadDeck(BlueprintDeckRuntime deck) => _deck = deck;

        public bool DrawBlueprint() => TryDrawBlueprint(() => _deck.Draw());

        public void DrawMultipleBlueprints(int amount) =>
            StartCoroutine(TryDrawMultipleBlueprints(amount, DrawBlueprint));

        private bool TryDrawBlueprint(Func<BlueprintRuntime> drawFunc)
        {
            if (_hand.Count >= maxHandSize) return false;

            try
            {
                var blueprint = drawFunc();
                _hand.Add(blueprint);
                EventBus<BlueprintDrawnEvent>.Fire(new BlueprintDrawnEvent(blueprint));
                
                Debug.Log($"Hand Size: {_hand.Count}");

                return true;
            }
            catch (EmptyDrawPileException)
            {
                return false;
            }
        }

        private IEnumerator TryDrawMultipleBlueprints(int amount, Func<bool> drawFunc)
        {
            while (amount > 0)
            {
                if (!drawFunc()) yield break;

                amount--;

                yield return new WaitForSeconds(drawSpeed);
            }
        }

        private void OnBlueprintUsed(BlueprintUsedEvent e)
        {
            _hand.Remove(e.Blueprint);
        }
    }
}