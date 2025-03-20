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

        public void Process()
        {
            int totalProcessed = 0;

            while (true)
            {
                List<int> batch = new List<int>();

                while (batch.Count < 1000 && _queue.TryDequeue(out int number))
                {
                    batch.Add(number);
                }

                if (batch.Count == 0)
                {
                    Console.WriteLine("Sorter: Queue is empty, stopping.");
                    break;
                }

                batch.Sort();
                totalProcessed += batch.Count;

                foreach (var num in batch)
                {
                    _sortedQueue.Enqueue(num);
                }

                Console.WriteLine($"Sorter processed batch: {batch.Count} numbers, sortedQueue count: {_sortedQueue.Count}");
            }

            Console.WriteLine($"Sorter finished processing {totalProcessed} numbers.");
        }
    }
}
