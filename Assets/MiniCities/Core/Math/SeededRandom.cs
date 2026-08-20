namespace MiniCities.Core.Math
{
    public class SeededRandom
    {
        public int Seed { get; private set; }
        
        private readonly System.Random _random;

        public SeededRandom(int seed)
        {
            Seed = seed;
            _random = new System.Random(seed);
        }

        public int Range(int min, int max) => _random.Next(min, max);
        public float Value() => (float)_random.NextDouble();
        public bool Chance(float probability) => Value() < probability;
    }
}