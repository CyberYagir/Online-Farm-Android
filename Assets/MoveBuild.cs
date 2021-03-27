using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBuild : MonoBehaviour
{
    public Vector2 poss;

    public List<GameObject> collisions = new List<GameObject>();
    public SpriteRenderer spriteRenderer;
    void Start()
    {
        poss = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var mb = FindObjectsOfType<MoveBuild>();
        for (int i = 0; i < mb.Length; i++)
        {
            if (mb[i] != this)
            {
                Destroy(mb[i]);
            }
        }

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
        transform.position = pl.tilemap.CellToWorld(pl.tilemap.WorldToCell(new Vector3(pos.x, pos.y, 0))) + new Vector3(0, 0.25f);
    
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (collisions.Count == 0)
            {
                Destroy(this);
            }
            else
            {
                spriteRenderer.color = Color.white;
                transform.position = new Vector3(poss.x, poss.y, 0);
                Destroy(this);
            }
        }
    
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
