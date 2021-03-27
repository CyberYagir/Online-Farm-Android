using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WebManager : MonoBehaviour
{
    public bool rus;
    public string url;
    public TMP_InputField login, password;

    public bool saveOnReginstration;
    public int saveTry = 0;

    public User userData;
    public string lg, pw;
    public GameObject waitScreen;


    [Header("Operations:")]
    public bool loginst;
    public bool regst, savest, setshopst, getshopst, rmshopst, getlistst, uplistst, buylistst;
    private void Start()
    {
        if (PlayerPrefs.HasKey("FarmTestLang"))
        {
            rus = PlayerPrefs.GetInt("FarmTestLang") == 1 ? true : false;
        }
    }

    public void LoginStart()
    {
        StartCoroutine(Login());
    }
    public void RegStart()
    {
        StartCoroutine(Register());
    }


    public IEnumerator Login()
    {
        if (!loginst)
        {
            loginst = true;
            waitScreen.SetActive(true);
            WWWForm form = new WWWForm();
            form.AddField("lg_go", "1");
            form.AddField("login", login.text);
            form.AddField("password", password.text);
            lg = login.text;
            pw = password.text;

            WWW www = new WWW(url, form);

            yield return www;
            print(www.text);
            var user = JsonUtility.FromJson<User>(www.text);


            if (user.normal)
            {
                userData = user;
                DontDestroyOnLoad(gameObject);
                Application.LoadLevel(1);
            }
            else
            {
                waitScreen.SetActive(false);
                login.text = user.json;
            }
            loginst = false;
        }
    }
    public IEnumerator Register()
    {
        if (!regst)
        {
            regst = true;
            waitScreen.SetActive(true);
            WWWForm form = new WWWForm();
            form.AddField("reg_go", "1");
            form.AddField("login", login.text);
            form.AddField("password", password.text);

            lg = login.text;
            pw = password.text;


            WWW www = new WWW(url, form);

            yield return www;

            var user = JsonUtility.FromJson<User>(www.text);


            if (user.normal)
            {
                userData = user;
                DontDestroyOnLoad(gameObject);
                saveOnReginstration = true;
                Application.LoadLevel(1);
            }
            else
            {
                waitScreen.SetActive(false);
                login.text = user.json;
            }
            regst = false;
        }
    }
    public IEnumerator Save()
    {
        if (!savest)
        {
            savest = true;
            WWWForm form = new WWWForm();
            form.AddField("save", "1");
            form.AddField("login", lg);
            form.AddField("password", pw);



            form.AddField("json", FindObjectOfType<GameManager>().SaveWorldJson());


            WWW www = new WWW(url, form);


            yield return www;
            savest = false;
        }

    }

    public IEnumerator SetShop()
    {
        if (!setshopst)
        {
            setshopst = true;
            WWWForm form = new WWWForm();
            form.AddField("setShop", "1");
            form.AddField("login", lg);
            form.AddField("password", pw);
            form.AddField("json", JsonUtility.ToJson(new TradeSlots() { items = FindObjectOfType<Trade>().placeItems }));

            WWW www = new WWW(url, form);

            yield return www;
            setshopst = false;
        }
    }

    public IEnumerator GetShop(bool setShop = false)
    {
        if (!getlistst)
        {
            getlistst = true;
            WWWForm form = new WWWForm();
            form.AddField("getShop", "1");
            form.AddField("login", lg);
            form.AddField("password", pw);


            WWW www = new WWW(url, form);
            yield return www;
            var us = JsonUtility.FromJson<User>(www.text);

            if (us.normal == true)
            {
                FindObjectOfType<Trade>().placeItems = JsonUtility.FromJson<TradeSlots>(us.json).items;
            }
            if (setShop)
            {
                StartCoroutine(SetShop());
            }

            getlistst = false;
        }
    }

    public IEnumerator RmShop()
    {
        if (!rmshopst)
        {
            rmshopst = true;
            WWWForm form = new WWWForm();
            form.AddField("removeShop", "1");
            form.AddField("login", lg);
            form.AddField("password", pw);


            WWW www = new WWW(url, form);
            yield return www;

            rmshopst = false;
        }
    }

    public IEnumerator GetList()
    {
        if (!getlistst)
        {
            getlistst = true;
            WWWForm form = new WWWForm();
            form.AddField("getList", "1");

            form.AddField("login", lg);
            form.AddField("password", pw);

            WWW www = new WWW(url, form);
            yield return www;
            print(www.text);
            ListOfTrades p = JsonUtility.FromJson<ListOfTrades>(www.text);

            List<ListTradesItem> items = new List<ListTradesItem>();

            for (int i = 0; i < p.list.Length; i++)
            {
                World wrld = JsonUtility.FromJson<World>(p.usersjsons[i]);
                TradeSlots slots = JsonUtility.FromJson<TradeSlots>(p.list[i]);
                int id = int.Parse(p.usersids[i]);
                items.Add(new ListTradesItem() { start = int.Parse(p.start), id = id, slots = slots, lvl = wrld.wp.lvl, nickname = wrld.wp.name });
            }

            FindObjectOfType<Trade>().tradesItems = items;
            FindObjectOfType<Trade>().DrawList();

            getlistst = false;
        }
    }

    public IEnumerator upList(int start)
    {
        if (!uplistst)
        {
            uplistst = true;
            WWWForm form = new WWWForm();
            form.AddField("upList", "1");

            form.AddField("login", lg);
            form.AddField("password", pw);
            form.AddField("start", start);

            WWW www = new WWW(url, form);
            yield return www;
            print(www.text);
            ListOfTrades p = JsonUtility.FromJson<ListOfTrades>(www.text);

            List<ListTradesItem> items = new List<ListTradesItem>();

            for (int i = 0; i < p.list.Length; i++)
            {
                World wrld = JsonUtility.FromJson<World>(p.usersjsons[i]);
                TradeSlots slots = JsonUtility.FromJson<TradeSlots>(p.list[i]);
                int id = int.Parse(p.usersids[i]);
                items.Add(new ListTradesItem() { start = int.Parse(p.start), id = id, slots = slots, lvl = wrld.wp.lvl, nickname = wrld.wp.name });
            }

            FindObjectOfType<Trade>().tradesItems = items;
            FindObjectOfType<Trade>().rawTrades = p;
            FindObjectOfType<Trade>().DrawList();

            uplistst = false;
        }
    }


    public IEnumerator buyList(int user, int slot)
    {
        if (!buylistst)
        {
            buylistst = true;
            WWWForm form = new WWWForm();
            form.AddField("byInList", "1");
            form.AddField("userid", user);
            form.AddField("slot", slot);

            WWW www = new WWW(url, form);
            yield return www;
            print(www.text);
            FindObjectOfType<Trade>().time = 10;
            FindObjectOfType<Trade>().DrawList();
            buylistst = false;
        }
    }
    public void SetLanguage(bool n)
    {
        rus = n;
        PlayerPrefs.SetInt("FarmTestLang", rus == true ? 1 : 0);
        print(PlayerPrefs.GetInt("FarmTestLang"));
    }
}
[System.Serializable]
public class User {
    public bool normal = false;
    public string json = "";

}

