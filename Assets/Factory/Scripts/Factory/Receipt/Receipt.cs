using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Receipt", menuName = "Factory/Receipt")]
public class Receipt : ScriptableObject
{
    public string id => name;
    
    public float ProductionTime;
    public List<ItemCount> InputItems;
    public List<ItemCount> OutputItems;
}
