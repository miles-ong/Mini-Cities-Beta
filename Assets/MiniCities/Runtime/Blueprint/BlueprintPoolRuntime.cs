using MiniCities.Core.Pool;
using MiniCities.Definitions.Blueprint;

namespace MiniCities.Runtime.Blueprint
{
    public class BlueprintPoolRuntime
    {
        private Pool<BlueprintDefinition> _blueprints;

        public BlueprintPoolRuntime(BlueprintPoolDefinition definition) => _blueprints = definition.blueprints.Clone();
    }
}