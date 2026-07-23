using _.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBlockData", menuName = "Mini Cities/Block")]
public class BlockData : ScriptableObject
{
    public BlockGroup group;
    public GameObject prefab;
}
