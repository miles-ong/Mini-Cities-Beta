using MiniCities.Definitions.Block;
using MiniCities.View;
using UnityEngine;

namespace MiniCities.Runtime.Block
{
    public class BlockRuntime
    {
        public BlockDefinition Definition { get; }
        public Vector3Int Location;
        
        public BlockView View { get; private set; }

        public BlockRuntime(BlockDefinition definition, Vector3Int location)
        {
            Definition = definition;
            Location = location;
        }

        public void SetView(BlockView view) => View = view;
    }
}