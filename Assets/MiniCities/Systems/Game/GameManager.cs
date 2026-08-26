using MiniCities.Core.Event;
using MiniCities.Runtime.Blueprint;
using MiniCities.Runtime.Map;
using MiniCities.Systems.City;
using MiniCities.Systems.City.Stat;
using MiniCities.Systems.Map;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniCities.Systems.Game
{
    public class GameManager : MonoBehaviour
    {
        [Header("Context")]
        [SerializeField] private GameSessionContext gameSessionContext;
        
        [Header("Managers")]
        [SerializeField] private BlueprintHand blueprintHand;
        [SerializeField] private CityManager cityManager;
        [SerializeField] private MapManager mapManager;
        
        private void Start()
        {
            gameSessionContext.Initialize(blueprintHand, cityManager, mapManager);

            GameEventBus<LevelStartedEvent>.OnEvent += OnLevelStarted;
            GameEventBus<LevelCompletedEvent>.OnEvent += OnLevelCompleted;
            GameEventBus<LevelFailedEvent>.OnEvent += OnLevelFailed;

            var map = new MapRuntime(gameSessionContext.MapPool.GetRandomMap());

            var targetStats = new CityStats()
            {
                Population = 500,
                Culture = 100,
                Pollution = 6
            };

            GameEventBus<LevelStartedEvent>.Fire(new LevelStartedEvent(new LevelContext(map, targetStats)));
        }

        private void OnDestroy()
        {
            GameEventBus<LevelStartedEvent>.OnEvent -= OnLevelStarted;
            GameEventBus<LevelCompletedEvent>.OnEvent -= OnLevelCompleted;
            GameEventBus<LevelFailedEvent>.OnEvent -= OnLevelFailed;
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasReleasedThisFrame) blueprintHand.DrawBlueprint();
        }

        private void OnLevelStarted(LevelStartedEvent evt)
        {
            gameSessionContext.File.FillDrawPile();
            
            mapManager.LoadMap(evt.Level.Map);
            cityManager.Initialize(evt.Level.TargetStats);
            
            gameSessionContext.Hand.DrawMultipleBlueprints(5);
        }

        private void OnLevelCompleted(LevelCompletedEvent evt)
        {
            GameEventBus<EnterShopEvent>.Fire(new EnterShopEvent());
        }

        private void OnLevelFailed(LevelFailedEvent evt)
        {
            
        }
    }
}