﻿using System.Collections.Concurrent;
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

            // ✅ Dictionary lưu nguồn RNG của từng số
            var sourceMap = new ConcurrentDictionary<int, string>();

            // Random generator 1,2,3:
            var rng1 = new Random();
            var rng2 = new Random();
            var rng3 = new Random();

            Task producer1 = Task.Run(() => GenerateNumbers(queue, rng1, "RNG1", sourceMap));
            Task producer2 = Task.Run(() => GenerateNumbers(queue, rng2, "RNG2", sourceMap));
            Task producer3 = Task.Run(() => GenerateNumbers(queue, rng3, "RNG3", sourceMap));

            await Task.WhenAll(producer1, producer2, producer3);

            Console.WriteLine($"Queue sizes before sorting -> Input: {queue.Count}");

            var sorter = new QueueSorter(queue, sortedQueue);
            await Task.Run(() => sorter.ProcessAsync());

            Console.WriteLine($"Queue sizes after sorting -> Sorted: {sortedQueue.Count}");

            var primeFinder = new QueuePrime(sortedQueue, primeQueue);
            primeFinder.Process();

            Console.WriteLine($"Queue sizes after prime filtering -> Primes: {primeQueue.Count}, Remaining Sorted: {sortedQueue.Count}");

            // ✅ Ghi số nguyên tố vào "primes.txt"
            var writerA = new Writer(primeQueue, "primes.txt");
            var writerATask = writerA.WriteToFile(sourceMap);

            // ✅ Ghi toàn bộ số đã sắp xếp vào "sorted.txt"
            var writerB = new Writer(sortedQueue, "sorted.txt");
            var writerBTask = writerB.WriteToFile(sourceMap);

            await Task.WhenAll(writerATask, writerBTask);            


            // ✅ Hiển thị Memory Usage
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
