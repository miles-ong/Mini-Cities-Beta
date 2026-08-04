namespace MiniCities.Core.Grid
{
    public enum GridScope
    {
        Adjacent,
        Nearby,
        All,
        On
    }

    public static class GridScopeExtensions
    {
        public static string ToDescriptionText(this GridScope scope) => scope switch
        {
            GridScope.Adjacent => "adjacent to",
            GridScope.Nearby => "nearby",
            GridScope.All => "in a city with",
            GridScope.On => "on",
            _ => scope.ToString()
        };
    }
}