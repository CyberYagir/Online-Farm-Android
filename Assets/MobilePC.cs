using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MobilePC : MonoBehaviour
{
    public Vector3 startPos;
    public float min = 1;
    public float max = 200;
    public bool uidrag = false;
    public List<UIWindow> windows;
    public bool canGetPos = true;

    int two;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delay());
    }


    IEnumerator delay()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!uidrag)
            {
                if (Input.touchCount == 2)
                {
                    if (two != 2)
                    {
                        if (canGetPos)
                        {
                            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        }
                    }

                    two = 2;
                }
                else if (Input.GetMouseButton(0))
                {
                    if (two != 1)
                    {
                        if (canGetPos)
                        {
                            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        }
                    }
                    two = 1;
                }
                else
                {
                    two = 0;
                }
            }
            else
            {
                startPos = Camera.main.transform.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        windows = FindObjectsOfType<UIWindow>().ToList();
        uidrag = windows.FindAll(x=>x.over == true).Count != 0;

        if (!uidrag)
        {
            
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.05f);
            }
            else if (Input.touchCount == 1)
            {
                Vector3 dir = startPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + dir, 0.1f);
            }
        }
        else
        {
            startPos = Camera.main.transform.position;
        }
        zoom(Input.GetAxis("Mouse ScrollWheel") * 2);
    }

    void zoom(float inc)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - inc, min, max);
    }
}

