using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Place : MonoBehaviour
{
    public List<GameObject> collisions = new List<GameObject>();
    public SpriteRenderer spriteRenderer;
    public int cost;
    public bool overOld = true;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        FindObjectOfType<Player>().type = Player.interactType.Build;
    }
    private void Update()
    {
        if (collisions.Count != 0)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }


        var pl = FindObjectOfType<Player>();

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = pl.tilemap.CellToWorld(pl.tilemap.WorldToCell(new Vector3(pos.x, pos.y, 0)));

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (overOld)
            {
                Destroy(gameObject);
                return;
            }

            if (collisions.Count == 0)
            {
                spriteRenderer.sortingLayerID = 0;
                if (FindObjectOfType<Stats>().money >= cost)
                {
                    ///BUY
                    FindObjectOfType<Stats>().money -= cost;
                    Destroy(this);
                }
                else
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }


        var p = FindObjectOfType<UI>().shopAnimator.gameObject.GetComponent<UIWindow>();
        overOld = p.over;
    }

    private void OnDestroy()
    {
        FindObjectOfType<Player>().type = Player.interactType.Move;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var c = collisions.Find(x => x.gameObject == collision.gameObject);
        if (c == null)
        {
            collisions.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collisions.Remove(collision.gameObject);
    }



}
