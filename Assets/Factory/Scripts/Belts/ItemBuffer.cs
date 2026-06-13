namespace Factory
{
    public interface IItemInput
    {
        bool CanAccept(ushort item, float progress);
        bool TryAccept(ushort item, float progress);
    }

    public interface IItemOutput
    {
        bool CanExtract(int count);
        bool Extract(int count, out ushort item);
    }

    public class ItemBuffer : IItemInput, IItemOutput
    {
        private int count;
        private readonly int size;
        private ushort itemID;
        
        public ItemBuffer(int size)
        {
            itemID = 0;
            count = 0;
            this.size = size;
        }

        public bool CanAccept(ushort item, float progress)
        {
            if (item == 0)
                return false;

            if(itemID == 0)
                return true;
            
            if(itemID != item)
                return false;
            if(count >= size)
                return false;
            return true;
        }

        public bool TryAccept(ushort item, float progress)
        {
            if (CanAccept(item, progress))
            {
                itemID = item;
                count++;
                return true;
            }
            return false;
        }
        
        public bool CanExtract(int count) => this.count >= count;

        public bool Extract(int count, out ushort item)
        {
            if(CanExtract(count))
            {
                item = itemID;
                this.count -= count;
                if (this.count == 0)
                    itemID = 0;
                return true;
            }

            item = 0;
            return false;
        }
    }
}