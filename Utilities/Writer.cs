using System.Collections.Concurrent;
using System.Diagnostics;

namespace TechnicalAssignment.Utilities
{
    public class Writer
    {
        private readonly ConcurrentQueue<int> _queue;
        private readonly string _filePath;

        public Writer(ConcurrentQueue<int> queue, string filePath)
        {
            _queue = queue;
            _filePath = filePath;
        }

        public async Task WriteToFile(ConcurrentDictionary<int, string> sourceMap)
        {
            using StreamWriter writer = new StreamWriter(_filePath, true);
            Stopwatch sw = Stopwatch.StartNew();

            if (_queue.IsEmpty)
            {
                Console.WriteLine($"Writer for {_filePath} found queue empty!");
                return;
            }

            int count = 0;
            while (_queue.TryDequeue(out int number))
            {
                string source = sourceMap.ContainsKey(number) ? sourceMap[number] : "Unknown";
                await writer.WriteLineAsync($"{number}, {source}");
                count++;
            }

            sw.Stop();

            double speed = count / (sw.ElapsedMilliseconds / 1000.0);
            Console.WriteLine($"{_filePath} has {count} numbers at {speed:F2} numbers/second.");
        }
    }
}
