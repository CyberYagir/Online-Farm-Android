using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barn : MonoBehaviour
{
    public GameObject canvas;

    public GameObject holder, item;

    bool openMenu;
    private void OnMouseDown()
    {

        openMenu = true;
    }

    private void OnMouseExit()
    {
        openMenu = false;
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

                    foreach (Transform item in holder.transform)
                    {
                        Destroy(item.gameObject);
                    }

                    var stats = FindObjectOfType<Stats>();
                    for (int i = 0; i < stats.inv.Count; i++)
                    {
                        var gm = Instantiate(item, holder.transform);
                        var it = stats.GIInv(stats.inv[i].enname);
                        gm.GetComponent<BarnItem>().text.text = it.val.ToString("000");
                        gm.GetComponent<BarnItem>().img.sprite = it.icon;
                        gm.SetActive(true);
                    }
                }
            }
        }
    }
}
