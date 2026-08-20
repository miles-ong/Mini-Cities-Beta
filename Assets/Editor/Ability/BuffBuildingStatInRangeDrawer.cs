using MiniCities.Definitions.Ability;
using UnityEditor;

[CustomPropertyDrawer(typeof(BuffBuildingStatInRange))]
public class BuffBuildingStatInRangeDrawer : ExclusiveFilterPropertyDrawer
{
    protected override string[] AlwaysDrawnFields => new[]
    {
        "stat", "operation", "amount", "scope", "level", "includeSelf"
    };

    protected override (string, string[])[] ExclusiveOptions => new[]
    {
        ("buffByCategory", new[] { "targetCategory" }),
        ("buffByBuilding", new[] { "targetBuilding" })
    };
}