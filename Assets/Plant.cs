using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public List<GameObject> collisions = new List<GameObject>();
    public SpriteRenderer spriteRenderer;
    public int cost;
    public bool overOld = true;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        transform.position = pl.tilemap.CellToWorld(pl.tilemap.WorldToCell(new Vector3(pos.x, pos.y, 0))) + new Vector3(0,0.25f);

        var stats = FindObjectOfType<Stats>();

        if (collisions.Count == 0)
        {
            spriteRenderer.sortingLayerID = 0;

            if (pl.tilemap.GetTile(pl.tilemap.WorldToCell(transform.position)) == pl.dirt)
            {
                var poses = FindObjectOfType<GameManager>().seedPoses;
                if (!poses.Contains(transform.position))
                {
                    ///BUY
                    ///

                    var it = stats.GIInv(GetComponent<Grow>().name);
                    if (it == null || it.val == 0)
                    {
                        if (stats.money >= cost)
                        {
                            stats.money -= cost;
                            var gm = Instantiate(gameObject, transform.position, Quaternion.identity);
                            Destroy(gm.GetComponent<Plant>());
                            poses.Add(transform.position);
                            transform.position = Vector3.zero;
                            return;
                        }
                    }
                    else
                    {
                        it.val -= 1;
                        var gm = Instantiate(gameObject, transform.position, Quaternion.identity);
                        Destroy(gm.GetComponent<Plant>());
                        poses.Add(transform.position);
                        transform.position = Vector3.zero;
                        return;
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Destroy(gameObject);

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
