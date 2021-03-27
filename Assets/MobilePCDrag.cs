using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MobilePCDrag : MonoBehaviour
{
    public MobilePC mobilePC;
    public List<UIWindow> windows;
    private void Start()
    {
        mobilePC.startPos = Camera.main.transform.position;
    }
    void Update()
    {
        windows = FindObjectsOfType<UIWindow>().ToList();
        var uidrag = windows.FindAll(x => x.over == true).Count == 0;

        mobilePC.enabled = uidrag;

        if (Input.GetMouseButtonDown(0))
        {
            if (uidrag)
            {
                mobilePC.startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }
}
