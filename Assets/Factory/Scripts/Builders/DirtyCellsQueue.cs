using System.Collections.Generic;

public class DirtyCellsQueue
{
    private readonly Queue<Cell> queue = new();
    private readonly HashSet<Cell> queued = new();

    public void Enqueue(Cell cell)
    {
        if (queued.Add(cell))
            queue.Enqueue(cell);
    }

    public bool TryDequeue(out Cell cell)
    {
        if (queue.Count == 0)
        {
            cell = default;
            return false;
        }

        cell = queue.Dequeue();
        queued.Remove(cell);
        return true;
    }

    public int Count => queue.Count;
}