int batchSize = 500;
int numberOfBatches = (numbers.Count + batchSize - 1) / batchSize;
var batches = new List<List<int>>();

for (int i = 0; i < numberOfBatches; i++)
{
    int startIndex = i * batchSize;
    int count = Math.Min(batchSize, numbers.Count - startIndex);
    var batch = numbers.GetRange(startIndex, count);
    batches.Add(batch);
}
