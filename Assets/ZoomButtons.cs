using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomButtons : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool over, down;
    public float speed;

    private void Update()
    {
        if (over && down)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                FindObjectOfType<GameManager>().Zoom(speed);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        down = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        down = false;
    }
}
