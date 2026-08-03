namespace MiniCities.Systems.BuffSys
{
    public interface IBuffReceiver
    {
        public void MarkDirty();
        public void MarkClean();
    }
}