using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public List<Item> items;
    public List<Craft> crafts;
}

[System.Serializable]
public class Item
{
    public string runame, enname;
    public Sprite icon;
    public int val = 1;

    public Item (Sprite icon, string runame, string enname)
    {
        this.runame = runame; this.enname = enname;  this.icon = icon;
    }

    public static Item Clone(Item item)
    {
        return new Item(item.icon, item.runame, item.enname) {val = item.val};
    }
}

[System.Serializable]
public class Craft {
    public string craftEnName;
    public int val;
    public List<CraftItem> items;
}

[System.Serializable]
public class CraftItem {

    public string enname;
    public int val;
}



