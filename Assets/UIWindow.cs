using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIWindow : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool over;
    public bool onenter;
    public void OnPointerDown(PointerEventData eventData)
    {
        over = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        over = false;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onenter)
        {
            over = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onenter)
        {
            over = false;
        }
    }
}
