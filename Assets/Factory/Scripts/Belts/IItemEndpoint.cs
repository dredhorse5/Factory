public interface IItemEndpoint
{
    bool CanInsert(Item item);
    bool TryInsert(Item item);

    bool CanExtract();
    bool TryExtract(out Item item);
}