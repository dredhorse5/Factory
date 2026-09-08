using Factory;
using UnityEngine;
using VContainer;

public class BeltSimulation : ISimulation
{
    private readonly BeltSystem beltSystem;

    [Inject]
    public BeltSimulation(BeltSystem beltSystem)
    {
        this.beltSystem = beltSystem;
    }

    public void Tick(float dt)
    {
        var belts = beltSystem.Belts;
        foreach (var belt in belts)
            MoveItems(belt, dt);
    }

    private void MoveItems(BeltBuild belt, float dt)
    {
        bool movingIsFree = false;
        for (var i = 0; i < belt.ItemBuffer.items.Count; i++)
        {
            var item = belt.ItemBuffer.items[i];
            item.Progress += dt * BeltSettings.Speed;
            if (i == 0)
            {
                if (item.Progress > 1) item.Progress = 1;
            }
            else if (!movingIsFree)
            {
                var min = belt.ItemBuffer.items[i - 1].Progress - BeltSettings.MinDistanceBetweenItems;
                if (belt.ItemBuffer.items[i - 1].Progress - item.Progress < BeltSettings.MinDistanceBetweenItems)
                    item.Progress = min;
                else
                    movingIsFree = true;
            }

            belt.ItemBuffer.items[i] = item;
        }
    }
}