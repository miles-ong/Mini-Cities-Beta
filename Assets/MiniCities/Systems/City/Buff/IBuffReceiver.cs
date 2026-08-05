namespace MiniCities.Systems.City.Buff
{
    public interface IBuffReceiver
    {
        public void MarkDirty();
        public void MarkClean();
    }
}