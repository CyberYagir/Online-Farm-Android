using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grow : MonoBehaviour
{
    public double start;
    public double end;
    public string name;
    public int growTime;
    public SpriteRenderer renderer;
    public Sprite t1, t2, t3;
    public int unixtime;
    public bool down;
    public int xp;
    public bool setTime;
    public bool timeSeted;
    private void Start()
    {
        unixtime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        if (setTime == false)
        {
            start = unixtime;
            end = unixtime + growTime;
        }
        renderer.sprite = t1;
    }



    private void FixedUpdate()
    {

        unixtime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;


        if (unixtime >= start + (growTime / 4f))
        {
            renderer.sprite = t1;
        }


        if (unixtime >= start + (growTime / 2f))
        {
            renderer.sprite = t2;
        }

        if (unixtime >= start + growTime)
        {
            renderer.sprite = t3;
        }
    }


    private void OnMouseDown()
    {
        if (renderer.sprite == t3)
        {
            down = true;
            FindObjectOfType<Player>().type = Player.interactType.Seed;
        }
    }
    private void OnMouseUp()
    {
        down = false;
    }
    private void OnMouseExit()
    {
        down = false;
    }
    private void OnMouseOver()
    {
        if (renderer.sprite == t3)
        {
            var stats = FindObjectOfType<Stats>();
            if (FindObjectOfType<Player>().type == Player.interactType.Seed)
            {
                var it = stats.GI(name);
                it.val = Random.Range(1, 3);
                stats.AddItem(it);
                Instantiate(FindObjectOfType<GameManager>().drop, transform.position, Quaternion.identity).GetComponent<Drop>().item = it;
                stats.exp += xp;
                FindObjectOfType<GameManager>().seedPoses.Remove(transform.position);
                Destroy(gameObject);
            }
        }
    }
}
