using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public RectTransform shovel;
    public RectTransform movebuild;
    public Player player;
    public MobilePC control;
    public Stats stats;
    public Animator shopAnimator;
    public bool shopOpen = false;
    public List<GameObject> shopMarkersWindows;
    public List<GameObject> shopMarkers;
    public int selectedMarker = 0;
    [Space]
    public RectTransform expLine;
    public TMP_Text lvlT, expT, coinT;

    private void Update()
    {
        control.enabled = Player.isOver() == false && !shopOpen && player.type == Player.interactType.Move;

        lvlT.text = stats.lvl.ToString();
        expT.text = stats.exp + " / " + stats.endXp;
        coinT.text = stats.money.ToString();
        expLine.localScale = new Vector3((((float)stats.exp / (float)stats.endXp)), 1, 1);

        if (player.type == Player.interactType.Shovel)
        {
            shovel.sizeDelta = new Vector2(120, 120);
        }
        else
        {
            shovel.sizeDelta = new Vector2(100, 100);
        }

        if (player.type == Player.interactType.MoveBuild)
        {
            movebuild.sizeDelta = new Vector2(120, 120);
        }
        else
        {
            movebuild.sizeDelta = new Vector2(100, 100);
        }
    }
    public void ShopMarkerClick(int id)
    {
        for (int i = 0; i < shopMarkersWindows.Count; i++)
        {
            shopMarkersWindows[i].gameObject.SetActive(i == id);
        }
        for (int i = 0; i < shopMarkers.Count; i++)
        {
            var pos = shopMarkers[i].GetComponent<RectTransform>().localPosition;
            if (id != i)
            {
                shopMarkers[i].GetComponent<RectTransform>().localPosition = new Vector3(216f*2, pos.y);
            }
            else
            {
                shopMarkers[i].GetComponent<RectTransform>().localPosition = new Vector3(220f*2, pos.y);
            }
        }
        selectedMarker = id;
    }

    public void ActiveShop()
    {
        if (!shopOpen)
        {
            shopAnimator.Play("OpenShop");
        }
        else
        {
            shopAnimator.Play("CloseShop");
        }
        shopOpen = !shopOpen;
    }

}
