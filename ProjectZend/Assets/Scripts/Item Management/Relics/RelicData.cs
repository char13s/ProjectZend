using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class RelicData
{
    [SerializeField] private RelicType type;
    [SerializeField] private int iD;
    [SerializeField] private string relicName;
    [SerializeField] private int level;
    [SerializeField] private int cost;
    [TextArea(3, 10)]
    [SerializeField] private string description;
    private int quantity;

    public int Quantity { get => quantity; set => quantity = value; }
    public string RelicName { get => relicName; set => relicName = value; }
    public int Level { get => level; set => level = value; }
    public int Cost { get => cost; set => cost = value; }
    public string Description { get => description; set => description = value; }
    public int ID { get => iD; set => iD = value; }
    public RelicType Type { get => type; set => type = value; }

    public abstract void Use();
    public abstract void Unequipped();
}
