using MiniCities.Core.Random;
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
        [SerializeField] private BlueprintDefinition testBlueprint;
        [SerializeField] private BlueprintHandUI blueprintHandUI;

        private SeededRandom _random;
        private CityManager _cityManager;
        private BuffManager _buffManager = new();
        
        private GameContext _context;
        
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
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                blueprintHandUI.Add(new BlueprintRuntime(testBlueprint));
            }
        }
    }
}