using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeItemListBuy : MonoBehaviour
{
    public int userid;
    public int slotNum;
    public int spawnid;



    public void Click()
    {
        var cost = GetComponentInParent<Trade>().tradesItems[spawnid].slots.items[slotNum].cost;
        var stats = FindObjectOfType<Stats>();
        if (cost <= stats.money)
        {
            if (GetComponentInParent<Trade>().tradesItems[spawnid].slots.items[slotNum].selled == false)
            {
                stats.money -= cost;
                var it = stats.GI(GetComponentInParent<Trade>().tradesItems[spawnid].slots.items[slotNum].enname);
                it.val = GetComponentInParent<Trade>().tradesItems[spawnid].slots.items[slotNum].val;
                stats.AddItem(it);
                GetComponentInParent<Trade>().tradesItems[spawnid].slots.items[slotNum].selled = true;
                GetComponentInParent<Trade>().time = 0;

                StartCoroutine(FindObjectOfType<WebManager>().buyList(userid, slotNum));
            }
        }
    }
}
