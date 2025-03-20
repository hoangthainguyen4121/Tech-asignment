using System.Collections.Concurrent;

namespace TechnicalAssignment.Utilities
{
    public class QueueSorter
    {
        private readonly ConcurrentQueue<int> _queue;
        private readonly ConcurrentQueue<int> _sortedQueue;
        public QueueSorter(ConcurrentQueue<int> queue, ConcurrentQueue<int> sortedQueue)
        {
            _queue = queue;
            _sortedQueue = sortedQueue;
        }

        public async Task ProcessAsync()
        {
            int totalProcessed = 0;

            var numbers = new List<int>();
            while (_queue.TryDequeue(out int number))
            {
                numbers.Add(number);
            }

            if (numbers.Count == 0)
            {
                Console.WriteLine("Sorter: Queue is empty, stopping.");
                return;
            }

            int batchSize = 1000;
            int numberOfBatches = (numbers.Count + batchSize - 1) / batchSize;
            var batches = new List<List<int>>();

            for (int i = 0; i < numberOfBatches; i++)
            {
                int startIndex = i * batchSize;
                int count = Math.Min(batchSize, numbers.Count - startIndex);
                var batch = numbers.GetRange(startIndex, count);
                batches.Add(batch);
            }

            var sortTasks = new List<Task>();
            foreach (var batch in batches)
            {
                sortTasks.Add(Task.Run(() =>
                {
                    batch.Sort();
                    foreach (var num in batch)
                    {
                        _sortedQueue.Enqueue(num);
                    }
                    Console.WriteLine($"Sorter processed batch: {batch.Count} numbers, sortedQueue count: {_sortedQueue.Count}");
                }));
            }

            await Task.WhenAll(sortTasks);

            totalProcessed = numbers.Count;
            Console.WriteLine($"Sorter finished processing {totalProcessed} numbers.");
        }
    }
}
