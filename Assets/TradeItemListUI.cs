using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeItemListUI : MonoBehaviour
{
    [HideInInspector]
    public ListTradesItem item;
    public List<TradeItemListUIITEM> tradeItems;

    public TMP_Text lvl, nick;
    [System.Serializable]
    public class TradeItemListUIITEM
    {
        public GameObject obj;
        public Image image;
        public TMP_Text val, cost;

    }

}


