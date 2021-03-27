using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public List<string> crafts;
    public Transform craftContent, craftItem;
    public GameObject canvas;
    public int selected = -1;
    [Space]
    public Transform craftCraftsContent, craftCraftsItem;
    public Transform craftInfo;
    public TextTranslator craftInfoText;
    public List<CraftItemUI> craftItems = new List<CraftItemUI>();


    bool openMenu;
    private void Start()
    {
        for (int i = 0; i < crafts.Count; i++)
        {
            var obj = Instantiate(craftItem, craftContent);
            obj.gameObject.GetComponent<CraftItemUI>().factory = this;
            obj.gameObject.GetComponent<CraftItemUI>().craftObject = crafts[i];
            obj.gameObject.GetComponent<CraftItemUI>().id = i;
            craftItems.Add(obj.gameObject.GetComponent<CraftItemUI>());
            obj.gameObject.SetActive(true);
        }
    }

    public void Create()
    {
        if (selected != -1)
        {
            var itinfo = FindObjectOfType<Items>().crafts.Find(x => x.craftEnName == craftItems[selected].craftObject);
            var stats = FindObjectOfType<Stats>();

            int plus = 0;
            for (int i = 0; i < itinfo.items.Count; i++)
            {
                for (int k = 0; k < stats.inv.Count; k++)
                {
                    if (itinfo.items[i].enname == stats.inv[k].enname)
                    {
                        if (stats.inv[k].val >= itinfo.items[i].val)
                        {
                            plus++;
                        }
                    }
                }
            }
            if (plus >= itinfo.items.Count)
            {
                for (int i = 0; i < itinfo.items.Count; i++)
                {
                    stats.inv.Find(x => x.enname == itinfo.items[i].enname).val -= itinfo.items[i].val;
                }


                var item = FindObjectOfType<Items>().items.Find(x => x.enname == craftItems[selected].craftObject);
                item = Item.Clone(item);
                item.val = itinfo.val;
                stats.AddItem(item);
            }
        }
    }
    public void DrawCraft(string craftObject, int id)
    {
        var it =FindObjectOfType<Items>().crafts.Find(x => x.craftEnName == craftObject);

        var itinfo = FindObjectOfType<Items>().items.Find(x => x.enname == craftObject);


        if (selected == id)
        {
            Create();
        }
        selected = id;
        craftInfoText.engText = itinfo.enname;
        craftInfoText.rusText = itinfo.runame;

        foreach (Transform item in craftCraftsContent)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < it.items.Count; i++)
        {
            var obj = Instantiate(craftCraftsItem, craftCraftsContent);
            obj.gameObject.GetComponent<CraftInfoUI>().enItemName = it.items[i].enname;
            obj.gameObject.GetComponent<CraftInfoUI>().val = it.items[i].val;
            obj.gameObject.SetActive(true);
        }


        craftInfo.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        openMenu = false;
    }
    private void OnMouseDown()
    {
        openMenu = true;
    }
    private void OnMouseUp()
    {
        if (FindObjectOfType<Player>().type == Player.interactType.Move)
        {
            if (!Player.isOver())
            {
                if (openMenu)
                {
                    canvas.SetActive(true);
                }
            }
        }
    }
}