using System;
using UnityEngine;

[Serializable]
public struct ItemCount
{
    public Item Item;
    public int Count;
    
    public static readonly ItemCount None = new(Item.None, 0);

    public ItemCount(Item item, int count)
    {
        Item = item;
        Count = count;
    }

    public bool CanAdd(ItemCount other)
    {
        return Item == other.Item;
    }

    public bool CanSubtract(ItemCount other)
    {
        return Item == other.Item && Count >= other.Count;
    }

    public static ItemCount operator +(ItemCount a, ItemCount b)
    {
        ValidateItems(a, b);

        return new ItemCount(
            a.Item,
            a.Count + b.Count
        );
    }

    public static ItemCount operator -(ItemCount a, ItemCount b)
    {
        ValidateItems(a, b);

        return new ItemCount(
            a.Item,
            a.Count - b.Count
        );
    }

    private static void ValidateItems(ItemCount a, ItemCount b)
    {
        if (a.Item != b.Item)
        {
            throw new InvalidOperationException(
                $"Cannot operate on different items: {a.Item} and {b.Item}");
        }
    }
}