using UnityEngine;
using UnityEngine.UI;

public class CraftItemUI : MonoBehaviour
{
    public string craftObject;
    public int id;
    public Image image;
    public Factory factory;

    Items items;
    private void Start()
    {
        items = FindObjectOfType<Items>();
        var craftC = items.crafts.Find(x => x.craftEnName == craftObject);

        image.sprite = items.items.Find(x => x.enname == craftC.craftEnName).icon;
    }



    public void Click()
    {
        factory.DrawCraft(craftObject, id);
    }
}
