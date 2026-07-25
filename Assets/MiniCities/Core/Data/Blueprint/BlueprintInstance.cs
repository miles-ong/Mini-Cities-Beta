using MiniCities.Core.Data.Blueprint.Enums;

namespace MiniCities.Core.Data.Blueprint
{
    public class BlueprintInstance
    {
        public readonly BlueprintFile File;
        public BlueprintCategory Category;
        
        
        public BlueprintInstance(BlueprintData data)
        {
            File = data.file;
            Category = data.category;
            
            // do ability fill here
        }
    }
}