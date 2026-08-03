namespace MiniCities.Core.Random
{
    public class SeededRandom
    {
        private readonly System.Random _random;

        public SeededRandom(int seed) => _random = new System.Random(seed);

        public int Range(int min, int max) => _random.Next(min, max);
        public float Value() => (float)_random.NextDouble();
        public bool Chance(float probability) => Value() < probability;
    }
}