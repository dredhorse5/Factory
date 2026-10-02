namespace Factory.Factory
{
    public class ItemsCell
    {
        public ItemCount ItemCount;
        
        public bool CanInsert(ItemCount items)
        {
            if (ItemCount.CanAdd(items))
                return true;
            return false;
        }

        public bool TryInsert(ItemCount items)
        {
            if (ItemCount.CanAdd(items))
            {
                ItemCount += items;
                return true;
            }
            return false;
        }

        public bool CanExtractAnyItem(out ItemCount items, int count)
        {
            if (count > 0 && ItemCount.Count >= 0)
            {
                items = new ItemCount(ItemCount.Item, count);
                return true;
            }
            items = ItemCount.None;
            return false;
        }

        /// Если хотим взять хоть какой-нибудь предмет с конкретным количеством
        public bool TryExtractAnyItem(out ItemCount items, int count)
        {
            if (count > 0 && ItemCount.Count >= 0)
            {
                items = new ItemCount(ItemCount.Item, count);
                ItemCount -= items;
                return true;
            }
            items = ItemCount.None;
            return false;
        }

        public bool CanExtract(ItemCount items)
        {
            if(ItemCount.CanSubtract(items))
                return true;
            return false;
        }

        /// Если хотим взять конкретный предмет с конкретным количеством
        public bool TryExtract(ItemCount items)
        {
            if (ItemCount.CanSubtract(items))
            {
                ItemCount -= items;
                return true;
            }
            return false;   
        }
    }
}