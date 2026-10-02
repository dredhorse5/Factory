using System;

[Serializable]
public struct ItemProgress
{
    public Item Item;
    public float Progress;

    public ItemProgress(Item item, float progress)
    {
        Item = item;
        Progress = progress;
    }
    
    public ItemProgress(Item item)
    {
        Item = item;
        Progress = 0;
    }
}
