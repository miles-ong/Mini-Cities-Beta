using MiniCities.Core.Random;
using MiniCities.Definitions.Blueprint;
using MiniCities.Definitions.Map;
using MiniCities.Runtime.Blueprint;
using MiniCities.Systems.City;
using MiniCities.Systems.Map;
using MiniCities.UI.Blueprint;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniCities.Systems.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MapManager mapManager;
        [SerializeField] private MapDefinition testMap;
        [SerializeField] private BlueprintDeckDefinition testDeck;
        [SerializeField] private BlueprintHandUI blueprintHandUI;

        [SerializeField] private int seed;
        
        private readonly CityManager _cityManager = new();
        
        private SeededRandom _random;
        private GameContext _context;
        private BlueprintDeckRuntime _deck;
        
        private void Start()
        {
            _random = new SeededRandom(seed);
            mapManager.LoadMap(testMap);
            _context = new GameContext(_random, mapManager, _cityManager);
            _deck = new BlueprintDeckRuntime(testDeck, _context);
            _deck.FillDrawPile();
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                blueprintHandUI.Add(_deck.Draw());
            }
        }
    }
}