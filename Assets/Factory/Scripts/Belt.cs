using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : ITickable
{
    private float speed;
    private byte[] items;
    private float progress;

    public Belt(int size, float speed)
    {
        items = new byte[size];
        this.speed = speed;
    }

    public void Tick(float tickTime)
    {
        MoveItems(tickTime);
    }

    private void MoveItems(float tickTime)
    {
        progress += tickTime * speed;
        while(progress >= 1f)
        {
            progress--;
            for(int i = items.Length - 1; i > 0; i--)
            {
                if(items[i] == 0)
                {
                    items[i] = items[i - 1];
                    items[i - 1] = 0;
                }
            }
        }
    }
}
