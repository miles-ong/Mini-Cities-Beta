using MiniCities.Core.Pool;
using MiniCities.Definitions.Map;

namespace MiniCities.Runtime.Map
{
    public class MapPoolRuntime
    {
        private Pool<MapDefinition> _maps;

        public MapPoolRuntime(MapPoolDefinition definition) => _maps = definition.maps.Clone();
    }
}