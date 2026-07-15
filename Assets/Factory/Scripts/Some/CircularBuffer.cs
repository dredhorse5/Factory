using System;

public class CircularBuffer<T>
{
    private readonly T[] buffer;

    private int head;
    private int count;

    public int Count => count;
    public int Capacity => buffer.Length;

    public bool IsFull => count == Capacity;
    public bool IsEmpty => count == 0;

    public CircularBuffer(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.");

        buffer = new T[capacity];
    }
    
    public T this[int index]
    {
        get
        {
            if ((uint)index >= (uint)count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return buffer[(head + index) % Capacity];
        }
        set
        {
            if ((uint)index >= (uint)count)
                throw new ArgumentOutOfRangeException(nameof(index));

            buffer[(head + index) % Capacity] = value;
        }
    }

    public bool Enqueue(T item)
    {
        if (IsFull)
            return false;

        int tail = (head + count) % Capacity;
        buffer[tail] = item;
        count++;

        return true;
    }

    public bool Dequeue(out T item)
    {
        if (IsEmpty)
        {
            item = default;
            return false;
        }

        item = buffer[head];
        buffer[head] = default;

        head = (head + 1) % Capacity;
        count--;

        return true;
    }

    public bool Peek(out T item)
    {
        if (count == 0)
        {
            item = default;
            return false;
        }

        item = buffer[head];
        return true;
    }
    
    public bool PeekLast(out T item) => PeekAt(count - 1, out item);

    public bool PeekAt(int index, out T item)
    {
        if ((uint)index >= (uint)count)
        {
            item = default;
            return false;
        }

        item = buffer[(head + index) % Capacity];
        return true;
    }

    public void Clear()
    {
        Array.Clear(buffer, 0, buffer.Length);
        head = 0;
        count = 0;
    }
}