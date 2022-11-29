using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int width = 1;
    public int height = 1;

    public Sprite itemIcon;
    public ItemType itemType;
}

public enum ItemType
{
    none,
    weapons,
    weapons_addons,
    ammo,
    grenades,
    mounted_weapons,
    equipments,
    detectors,
    medkit,
    suits,
    food,
    artefacts,
    moster_parts

}