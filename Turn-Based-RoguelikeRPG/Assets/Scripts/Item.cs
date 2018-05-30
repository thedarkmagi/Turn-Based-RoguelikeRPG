using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : ScriptableObject
{
    public string ItemName;
    public string ItemDesc;

    public enum ItemType
    {
        Consumable,
        Gear,
        KeyItem
    }
	
}

[CreateAssetMenu]
public class Consumable : Item
{
    public float HP_Gain;
    public float MP_Gain;
}

[CreateAssetMenu]
public class Gear : Item
{
    public enum GearType
    {
        Head,
        Shoulders,
        Gloves,
        Waist,
        Chest,
        Legs,
        Feet,
        Back,
        RingA,
        RingB,
        HandA,
        HandB
    }

    public GearType MyType;

    public float Fortify_STR;
    public float Fortify_DEX;
    public float Fortify_INT;
    public float Fortify_WIS;
    public float Fortify_CON;
    public float Fortify_CHA;

    public float Fortify_DMG;
    public float Fortify_SPEED;
    public float Fortify_AC;

}

[CreateAssetMenu]
public class KeyItem : Item
{

}
