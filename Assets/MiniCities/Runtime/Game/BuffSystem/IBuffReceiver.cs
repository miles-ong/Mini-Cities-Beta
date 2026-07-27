namespace MiniCities.Runtime.Game.BuffSystem
{
    public interface IBuffReceiver
    {
        public void MarkDirty();
        public void MarkClean();
    }
}