using System.Collections.Concurrent;
namespace TechnicalAssignment.Utilities
{
    public class QueuePrime
    {
        private readonly ConcurrentQueue<int> _sortedQueue;
        private readonly ConcurrentQueue<int> _primeQueue;

        public QueuePrime(ConcurrentQueue<int> sortedQueue, ConcurrentQueue<int> primeQueue)
        {
            _sortedQueue = sortedQueue;
            _primeQueue = primeQueue;
        }

        public void Process()
        {
            int primeCount = 0;

            List<int> numbersToKeep = new List<int>();

            while (_sortedQueue.TryDequeue(out int number))
            {
                if (IsPrime(number))
                {
                    _primeQueue.Enqueue(number);
                    primeCount++;
                }
                else
                {
                    numbersToKeep.Add(number);
                }
            }

            foreach (var num in numbersToKeep)
            {
                _sortedQueue.Enqueue(num);
            }

            Console.WriteLine($"PrimeFinder processed {primeCount} prime numbers.");
            Console.WriteLine($"After PrimeFinder, sortedQueue count: {_sortedQueue.Count}");
        }



        private bool IsPrime(int n)
        {
            if (n < 2) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;

            for (int i = 5; i * i <= n; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0) return false;
            }
            return true;
        }
    }
}
