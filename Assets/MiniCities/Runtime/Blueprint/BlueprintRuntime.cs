using MiniCities.Definitions.Blueprint;

namespace MiniCities.Runtime.Blueprint
{
    public class BlueprintRuntime
    {
        public string DisplayName;
        public BlueprintFile File;
        public BlueprintCategory Category;
        public string Description = "Test";
        public BuildingStats Stats { get; private set; }
        
        public BlueprintRuntime(BlueprintDefinition definition)
        {
            DisplayName = definition.displayName;
            File = definition.file;
            Category = definition.category;
            
            Stats = new BuildingStats
            {
                Population = definition.population,
                Culture = definition.culture,
                Pollution = definition.pollution
            };
        }
    }
}