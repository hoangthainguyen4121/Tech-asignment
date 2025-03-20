namespace TechnicalAssignment.Extensions
{
    public static class GeneratorExtension
    {
        public static IEnumerable<int> GenerateRandomNumbers(this Random rng, 
            int count, int min, int max)
        {
            for (int i = 0; i < count; i++)
            {
                yield return rng.Next(min, max);
            }
        }
    }
}
