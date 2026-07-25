using MiniCities.Core.Data.Map;
using UnityEngine;

namespace MiniCities.Runtime.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MapManager mapManager;
        [SerializeField] private MapDefinition testMap;
        
        private void Start()
        {
            mapManager.LoadMap(testMap);
        }
    }
}