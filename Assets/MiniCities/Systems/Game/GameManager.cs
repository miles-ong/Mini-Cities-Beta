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
        [Header("Context")]
        [SerializeField] private GameSessionContext gameSessionContext;
        
        [Header("Managers")]
        [SerializeField] private BlueprintHand blueprintHand;
        [SerializeField] private CityManager cityManager;
        [SerializeField] private MapManager mapManager;
        
        [Header("UI Controllers")]
        [SerializeField] private BlueprintHandUI blueprintHandUI;
        
        private void Start()
        {
            gameSessionContext.Initialize(blueprintHand, cityManager, mapManager);
            
            mapManager.LoadMap(gameSessionContext.MapPool.GetRandomMap());
            gameSessionContext.File.FillDrawPile();
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasReleasedThisFrame) blueprintHand.DrawBlueprint();
        }
    }
}