using System.Collections.Generic;
using MiniCities.Definitions.Blueprint;

namespace MiniCities.Runtime.Blueprint
{
    public class BlueprintPoolRuntime
    {
        private List<BlueprintDefinition> _blueprints;

        public BlueprintPoolRuntime(BlueprintPoolDefinition definition) =>
            _blueprints = new List<BlueprintDefinition>(definition.blueprints);
    }
}