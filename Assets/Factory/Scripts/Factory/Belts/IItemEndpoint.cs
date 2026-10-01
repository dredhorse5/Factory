public interface IItemEndpoint
{
    bool CanInsert(ItemProgress item);
    bool TryInsert(ItemProgress item);

    bool CanExtract();
    bool TryExtract(out ItemProgress item);
}