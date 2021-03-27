using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyBuild : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UIWindow uiWindow;
    public bool down;
    public GameObject spawned;
    public GameObject prefab;

    public Image image;
    public TextTranslator naming;
    public TMP_Text costT;
    public bool displayVal;
    [Space]
    public int cost;
    public string runame, enname;
    public Sprite sprite;
    public enum type {Build,Seed};
    public type buildType;
    public void OnPointerDown(PointerEventData eventData)
    {
        down = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    private void Start()
    {
        uiWindow = GetComponentInParent<UIWindow>();
        naming.rusText = runame;
        naming.engText = enname;
        costT.text = cost.ToString();
        image.sprite = sprite;
    }


    private void Update()
    {
        if (displayVal)
        {
            var it = FindObjectOfType<Stats>().inv.Find(x => x.enname == enname);
            if (it != null)
            {
                naming.rusText = runame + " [На складе: " + it.val.ToString("000") + "]";
                naming.engText = enname + " [In stock: " + it.val.ToString("000") + "]";
            }
            else
            {
                naming.rusText = runame;
                naming.engText = enname;
            }
        }
        if (down && uiWindow.over == false)
        {
            if (spawned == null)
            {
                spawned = Instantiate(prefab.gameObject);
                if (buildType == type.Build)
                {
                    spawned.AddComponent<Place>().cost = cost;
                }
                if (buildType == type.Seed)
                {
                    spawned.AddComponent<Plant>().cost = cost;
                }
            }
        }
        if (down)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                spawned = null;
                down = false;
            }
        }
    }


}
