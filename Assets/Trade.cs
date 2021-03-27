using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Trade : MonoBehaviour
{
    public List<TradeIt> placeItems = new List<TradeIt>(6);

    public List<BarnItem> itemsSlots = new List<BarnItem>(6);

    public GameObject canvas, second;
    public Image secondImage;
    public Slider secondSlider;
    public TMP_InputField priceText;
    public GameObject selectMenu;
    public TMP_Text secondText;

    public GameObject holder, item;

    public int selected = -1;

    public string selectedname;

    public bool openMenu;
    public Sprite moneySprite;

    public float time;
    [Space]
    public GameObject tradesWindow;
    public List<ListTradesItem> tradesItems;
    public Transform tradesHolder, tradesItem;
    [HideInInspector]
    public ListOfTrades rawTrades;

    private void Start()
    {
        StartCoroutine(FindObjectOfType<WebManager>().GetShop());
    }

    public void Publish()
    {
        time = 0;
        int count = 0;
        for (int i = 0; i < placeItems.Count; i++)
        {
            if (placeItems[i].enname != "" && placeItems[i].selled == false)
            {
                count += 1;
            }
        }
        if (count != 0)
        {
            StartCoroutine(FindObjectOfType<WebManager>().SetShop());
        }
        else
        {
            StartCoroutine(FindObjectOfType<WebManager>().RmShop());
        }
    }
    public void Place()
    {
        var stats = FindObjectOfType<Stats>();
        stats.GIInv(selectedname).val -= (int)secondSlider.value;
        var it = stats.GI(selectedname);
        it.val = (int)secondSlider.value;
        placeItems[selected] = new TradeIt() { cost = int.Parse("0" + priceText.text), enname = it.enname, val = it.val };
        second.SetActive(false);
        selectMenu.SetActive(false);    
    }
    public void OpenList()
    {
        tradesWindow.SetActive(true);
        StartCoroutine(FindObjectOfType<WebManager>().GetList());
    }
    public void DrawList()
    {
        foreach (Transform item in tradesHolder.transform)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < tradesItems.Count; i++)
        {
            var obj = Instantiate(tradesItem, tradesHolder);
            var tr = obj.GetComponent<TradeItemListUI>();
            tr.item = tradesItems[i];
            for (int k = 0; k < tr.tradeItems.Count; k++)
            {
                if (tr.item.slots.items[k].enname != "")
                {
                    tr.tradeItems[k].image.gameObject.SetActive(true);
                    var it = FindObjectOfType<Stats>().GI(tr.item.slots.items[k].enname);
                    tr.tradeItems[k].cost.text = tr.item.slots.items[k].cost.ToString("0000");
                    tr.tradeItems[k].val.text = tr.item.slots.items[k].val.ToString("000");
                    var buy = tr.tradeItems[k].obj.GetComponent<TradeItemListBuy>();
                    buy.userid = tr.item.id;
                    buy.slotNum = k;
                    buy.spawnid = i;



                    if (tr.item.slots.items[k].selled)
                    {
                        tr.tradeItems[k].image.sprite = moneySprite;
                        tr.tradeItems[k].cost.text = "";
                        tr.tradeItems[k].val.text = "";
                    }
                    else
                    {
                        tr.tradeItems[k].image.sprite = it.icon;
                    }
                }
                else
                {
                    tr.tradeItems[k].cost.text = "";
                    tr.tradeItems[k].val.text = "";
                    tr.tradeItems[k].image.gameObject.SetActive(false);
                }

                tr.lvl.text = tr.item.lvl.ToString();
                tr.nick.text = tr.item.nickname;
            }
            obj.gameObject.SetActive(true);
        }
    }
    public void ClickSlot(int id)
    {
        if (placeItems[id].enname == "" && placeItems[id].selled == false)
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
                gm.GetComponent<TradeItem>().enname = stats.inv[i].enname;
                gm.GetComponent<TradeItem>().val = stats.inv[i].val;
                gm.GetComponent<TradeItem>().image.sprite = it.icon;
                gm.SetActive(true);
            }
            selected = id;
            selectMenu.SetActive(true);
        }
        else
        {
            print("else");
            if (placeItems[id].selled)
            {
                FindObjectOfType<Stats>().money += placeItems[id].cost;
                placeItems[id].enname = "";
                placeItems[id].selled = false;
            }
            Publish();
        }
    }


    private void Update()
    {

        time += 1 * Time.deltaTime;
        
        if (time >= 5)
        {
            if (tradesWindow)
            {
                if (tradesItems.Count > 0)
                {
                    StartCoroutine(FindObjectOfType<WebManager>().upList(int.Parse(rawTrades.start)));
                }
            }

            StartCoroutine(FindObjectOfType<WebManager>().GetShop());
            int count = 0;
            for (int i = 0; i < placeItems.Count; i++)
            {
                if (placeItems[i].enname != "" && placeItems[i].selled == false)
                {
                    count += 1;
                }
            }
            if (count == 0)
            {
                StartCoroutine(FindObjectOfType<WebManager>().RmShop());
            }
            time = 0;
        }


        var stats = FindObjectOfType<Stats>();
        secondText.text = secondSlider.value.ToString() + "/" + secondSlider.maxValue;
        for (int i = 0; i < itemsSlots.Count; i++)
        {
            if (placeItems[i].enname != "")
            {
                var it = stats.GI(placeItems[i].enname);
                it.val = (int)placeItems[i].val;

                itemsSlots[i].img.enabled = true;
                itemsSlots[i].text.text = placeItems[i].val.ToString("000");
                if (placeItems[i].selled == false)
                {
                    itemsSlots[i].img.sprite = it.icon;
                }
                else
                {
                    itemsSlots[i].img.sprite = moneySprite;
                }
            }
            else
            {
                itemsSlots[i].text.text = "";
                itemsSlots[i].img.enabled = false;
                itemsSlots[i].img.sprite = null;
            }
        }
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

[System.Serializable]
public class TradeSlots {
     public List<TradeIt> items = new List<TradeIt>(6);   
}

[System.Serializable]
public class TradeIt
{
    public string enname;
    public int val;
    public int cost;
    public bool selled;
}
[System.Serializable]
public class ListOfTrades {
    public string[] usersids = new string[20];
    public string[] usersjsons = new string[20];
    public string[] list = new string[20];
    public string start = "0";
}

[System.Serializable]
public class ListTradesItem
{
    public int start = 0;
    public int id = -1;
    public int lvl = -1;
    public string nickname = "";
    public TradeSlots slots = new TradeSlots();
}



