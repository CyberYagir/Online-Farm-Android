using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftInfoUI : MonoBehaviour
{
    public string enItemName;
    public int val;
    [Space]
    public TextTranslator text;
    public Image image;

    [Space]
    public Items items;
    public Stats stats;
    private void Start()
    {
        items = FindObjectOfType<Items>();
        stats = FindObjectOfType<Stats>();

        var invIt = stats.inv.Find(x => x.enname == enItemName);

        var it = items.items.Find(x => x.enname == enItemName);

        image.sprite = it.icon;

        text.engText = "" + (invIt == null ? 0 : invIt.val) + "/" + val;
        text.rusText = "" + (invIt == null ? 0 : invIt.val) + "/" + val;

    }

    private void FixedUpdate()
    {
        var invIt = stats.inv.Find(x => x.enname == enItemName);
        text.engText = "" + (invIt == null ? 0 : invIt.val) + "/" + val;
        text.rusText = "" + (invIt == null ? 0 : invIt.val) + "/" + val;
    }
}
