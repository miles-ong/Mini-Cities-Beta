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

        private BlueprintFileRuntime _file;
        private readonly List<BlueprintRuntime> _hand = new();

        private void Start()
        {
            GameEventBus<BlueprintUsedEvent>.OnEvent += OnBlueprintUsed;
        }

        public void LoadFile(BlueprintFileRuntime file) => _file = file;

        public bool DrawBlueprint() => TryDrawBlueprint(() => _file.Draw());

        public void DrawMultipleBlueprints(int amount) =>
            StartCoroutine(TryDrawMultipleBlueprints(amount, DrawBlueprint));

        private bool TryDrawBlueprint(Func<BlueprintRuntime> drawFunction)
        {
            if (_hand.Count >= maxHandSize) return false;

            try
            {
                var blueprint = drawFunction();
                _hand.Add(blueprint);
                GameEventBus<BlueprintDrawnEvent>.Fire(new BlueprintDrawnEvent(blueprint));

                return true;
            }
            catch (EmptyDrawPileException)
            {
                return false;
            }
        }

        private IEnumerator TryDrawMultipleBlueprints(int amount, Func<bool> drawFunction)
        {
            while (amount > 0)
            {
                if (!drawFunction()) yield break;

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