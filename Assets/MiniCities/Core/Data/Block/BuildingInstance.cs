using UnityEngine;

namespace MiniCities.Core.Data.Block
{
    public class BuildingInstance : BlockInstance
    {
        public int Population;
        public int Culture;
        public int Pollution;
        
        public BuildingInstance(BlockDefinition definition, Vector3Int position) : base(definition, position)
        {
            
        }
    }
}