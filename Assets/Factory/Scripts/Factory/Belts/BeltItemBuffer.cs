namespace Factory
{
    public class BeltItemBuffer : IItemEndpoint
    {
        public readonly CircularBuffer<ItemProgress> items;
        
        public BeltItemBuffer(int capacity) => items = new CircularBuffer<ItemProgress>(capacity);

        public bool CanInsert(ItemProgress item)
        {
            if (items.IsFull)
                return false;
            if (items.IsEmpty)
                return true;

            items.PeekLast(out var lastItem);

            return lastItem.Progress - BeltSettings.MinDistanceBetweenItems >= 0;
        }

        public bool TryInsert(ItemProgress item)
        {
            if (CanInsert(item))
            {
                if(items.PeekLast(out var lastItem))
                {
                    var maxPrg = lastItem.Progress - BeltSettings.MinDistanceBetweenItems;
                    if (item.Progress > maxPrg)
                        item.Progress = maxPrg;
                }
                items.Enqueue(item);
                return true;
            }

            return false;
        }

        public bool CanExtract() => items.Peek(out ItemProgress item) && item.Progress > 0.9999f;
        

        public bool TryExtract(out ItemProgress item)
        {
            if (!CanExtract())
            {
                item = default;
                return false;
            }

            return items.Dequeue(out item);
        }
    }
}