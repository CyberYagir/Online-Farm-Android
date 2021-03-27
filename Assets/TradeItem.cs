using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeItem : MonoBehaviour
{
    public string enname;
    public Image image;
    public int val;
   
    public void Click()
    {
        var tr = GetComponentInParent<Trade>();
        tr.selectedname = enname;
        tr.second.SetActive(true);
        tr.secondImage.sprite = FindObjectOfType<Stats>().GIInv(enname).icon;
        tr.secondSlider.maxValue = FindObjectOfType<Stats>().GIInv(enname).val;
        tr.secondSlider.minValue = 1;

    }
}
