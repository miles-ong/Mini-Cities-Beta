using MiniCities.Definitions.Game;

namespace MiniCities.Systems.Game
{
    public class GameSettingsRuntime
    {
        public int MaxHandSize;
        public int InitialDrawAmount;
        
        public GameSettingsRuntime(GameSettingsDefinition definition)
        {
            MaxHandSize = definition.maxHandSize;
            InitialDrawAmount = definition.initialDrawAmount;
        }
    }
}