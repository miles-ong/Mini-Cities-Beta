using MiniCities.Definitions.Ability;
using UnityEditor;

[CustomPropertyDrawer(typeof(BlockInRange))]
public class BlockInRangeDrawer : ExclusiveFilterPropertyDrawer
{
    protected override string[] AlwaysDrawnFields => new[]
    {
        "amount", "scope", "level", "includeSelf", "queryTriggeringBuilding"
    };

    protected override (string, string[])[] ExclusiveOptions => new[]
    {
        ("checkByCategory", new[] { "targetCategory" }),
        ("checkByGroup", new[] { "targetGroup" }),
        ("checkByBlock", new[] { "targetBlock" })
    };
}