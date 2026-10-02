using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Receipt", menuName = "Factory/Receipt")]
public class Recipe : ScriptableObject
{
    public string id => name;
    
    [SerializeField] private float productionTime;
    [SerializeField] private ItemCount[] inputItems;
    [SerializeField] private ItemCount[] outputItems;
    
    
    public float ProductionTime => productionTime;
    public IReadOnlyList<ItemCount> InputItems => inputItems;
    public IReadOnlyList<ItemCount> OutputItems => outputItems;
}
