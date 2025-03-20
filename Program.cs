using System.Collections.Concurrent;
using TechnicalAssignment.Extensions;
using TechnicalAssignment.Utilities;

namespace TechnicalAssignment
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var queue = new ConcurrentQueue<int>();
            var sortedQueue = new ConcurrentQueue<int>();
            var primeQueue = new ConcurrentQueue<int>();

            // Source RNG for numbers
            var sourceMap = new ConcurrentDictionary<int, string>();

            // Random generator 1,2,3:
            var rng1 = new Random();
            var rng2 = new Random();
            var rng3 = new Random();

            Task producer1 = Task.Run(() => GenerateNumbers(queue, rng1, "RNG1", sourceMap));
            Task producer2 = Task.Run(() => GenerateNumbers(queue, rng2, "RNG2", sourceMap));
            Task producer3 = Task.Run(() => GenerateNumbers(queue, rng3, "RNG3", sourceMap));

            await Task.WhenAll(producer1, producer2, producer3);

            //Console.WriteLine($"Check Queue sizes before sorting -> Input: {queue.Count}");

            var sorter = new QueueSorter(queue, sortedQueue);
            sorter.Process();

            Console.WriteLine($"Queue sizes after sorting -> Sorted: {sortedQueue.Count}");

            // ✅ Run PrimeFinder & Writer B in parallel!
            Task primeTask = Task.Run(() => {
                var primeFinder = new QueuePrime(sortedQueue, primeQueue);
                primeFinder.Process();
            });

            Task writerTask = Task.Run(async () => {
                var writerB = new Writer(sortedQueue, "sorted.txt");
                await writerB.WriteToFile(sourceMap);
            });

            // ✅ Wait for both to finish
            await Task.WhenAll(primeTask, writerTask);

            // ✅ Now write primes to file
            var writerA = new Writer(primeQueue, "primes.txt");
            await writerA.WriteToFile(sourceMap);


            // Memory Usage
            Console.WriteLine($"Memory Usage: {GC.GetTotalMemory(false) / 1024} KB");

            Console.WriteLine("Processing Completed!");
        }

        static void GenerateNumbers(ConcurrentQueue<int> queue, Random rng, string source, ConcurrentDictionary<int, string> sourceMap)
        {
            foreach (var num in rng.GenerateRandomNumbers(1000, 1, 10000))
            {
                queue.Enqueue(num);
                sourceMap[num] = source; // Add tag RNG
                Console.WriteLine($"{source}: {num}");
            }
        }
    }
}
