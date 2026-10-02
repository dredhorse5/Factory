
using Factory.Factory;

public class ItemsStorage : IItemEndpoint
{
    public ItemsCell[] Cells;

    public ItemsStorage(int capacity)
    {
        Cells = new ItemsCell[capacity];
    }

    public bool CanInsert(ItemProgress item)
    {
        var itemCount = new ItemCount(item.Item, 1);
        for (var i = 0; i < Cells.Length; i++)
            if (Cells[i].CanInsert(itemCount))
                return true;
        return false;
    }

    public bool TryInsert(ItemProgress item)
    {
        var itemCount = new ItemCount(item.Item, 1);
        for (var i = 0; i < Cells.Length; i++)
            if (Cells[i].TryInsert(itemCount))
                return true;
        return false;
    }

    public bool CanExtract()
    {
        for (var i = 0; i < Cells.Length; i++)
            if (Cells[i].CanExtractAnyItem(out var item, 1))
                return true;
        return false;
    }

    public bool TryExtract(out ItemProgress item)
    {
        for (var i = 0; i < Cells.Length; i++)
            if (Cells[i].CanExtractAnyItem(out var item1, 1))
            {
                item = new ItemProgress(item1.Item);
                return true;
            }
        item = default;
        return false;
    }
}
