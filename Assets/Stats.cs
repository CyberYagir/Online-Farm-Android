using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public string nickname;
    public int money;
    [Space]
    public int exp;
    public int endXp;
    public int lvl;
    public List<Item> inv = new List<Item>();


    private void Update()
    {
        if (exp >= endXp)
        {
            lvl++;
            exp = 0;
            endXp += (int)(endXp * 0.25f);
        }
        for (int i = 0; i < inv.Count; i++)
        {
            if (inv[i].val == 0)
            {
                inv.RemoveAt(i);
                return;
            }
        }
    }


    public void AddItem(Item item)
    {
        var exitem = inv.Find(x => x.enname == item.enname);

        if (exitem != null)
        {
            exitem.val += item.val;
        }
        else
        {
            inv.Add(item);
        }
    }
    public Item GI(string enname) => Item.Clone(FindObjectOfType<Items>().items.Find(x => x.enname == enname));

    public Item GIInv(string enname) => inv.Find(x => x.enname == enname);
}
