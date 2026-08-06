using MiniCities.Core.Math;
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
        [SerializeField] private BlueprintHand blueprintHand;
        [SerializeField] private BlueprintHandUI blueprintHandUI;
        
        [SerializeField] private BlueprintDeckDefinition testDeck;

        [SerializeField] private int seed;
        
        private SeededRandom _random;
        
        private GameContext _context;
        
        private void Start()
        {
            _random = new SeededRandom(seed);
            var deck = new BlueprintDeckRuntime(testDeck, _random);
            var cityManager = new CityManager(mapManager);
            
            _context = new GameContext(_random, mapManager, cityManager, deck, blueprintHand);
            
            mapManager.LoadMap(testMap);
            blueprintHand.LoadDeck(deck);
            deck.FillDrawPile();
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasReleasedThisFrame) blueprintHand.DrawBlueprint();
        }
    }
}