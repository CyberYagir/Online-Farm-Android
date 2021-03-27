using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public Item item;

    public GameObject target;
    public SpriteRenderer renderer;
    float waitTime = 0;
    public float speed = 1;
    private void Start()
    {

        renderer.sprite = item.icon;
        var barns = FindObjectsOfType<Barn>();

        float min = 9999;
        int id = -1;
        for (int i = 0; i < barns.Length; i++)
        {
            var dst = Vector2.Distance(barns[i].transform.position, transform.position);
            if (dst < min)
            {
                id = i;
                min = dst; 
            }
        }

        target = barns[id].gameObject;
    }

    private void Update()
    {
        waitTime += 1 * Time.deltaTime;
        if (waitTime > 2)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, target.transform.position) < 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
