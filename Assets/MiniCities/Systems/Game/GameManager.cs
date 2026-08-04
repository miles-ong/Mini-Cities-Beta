using MiniCities.Core.Random;
using MiniCities.Definitions.Block;
using MiniCities.Definitions.Blueprint;
using MiniCities.Definitions.Map;
using MiniCities.Runtime.Blueprint;
using MiniCities.Systems.BuffSys;
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

        private SeededRandom _random;
        private CityManager _cityManager;
        private BuffManager _buffManager = new();
        
        private GameContext _context;
        private BlueprintDeckRuntime _deck;
        
        private void Start()
        {
            Initialize();
            
            mapManager.LoadMap(testMap);
        }

        public void Initialize()
        {
            _random = new SeededRandom(5000);
            _cityManager = new CityManager(_buffManager);
            _context = new GameContext(new SeededRandom(5000), mapManager, _cityManager, _buffManager);
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