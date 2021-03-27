using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
public class Player : MonoBehaviour
{

    public enum interactType {Shovel, Build, Move, MoveBuild, Seed};
    public interactType type = interactType.Move;
    public Tilemap tilemap;
    public TileBase grass, dirt;
    public Vector3Int pos;
    public TileBase currentTileBase;

    [Header("Shovel")]
    public TileBase downtilebase;
    [Header("Seed")]
    public RectTransform basket;

    [Space]
    public List<Vector3> dirtPoses = new List<Vector3>(); 

    private void FixedUpdate()
    {
    }
    public void Update()
    {
        Vector3 ps = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = tilemap.WorldToCell(ps - new Vector3(0, 2.8f, 0));
        pos = new Vector3Int(pos.x, pos.y, 0);
        currentTileBase = tilemap.GetTile(pos);
        if (type == interactType.Shovel)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                downtilebase = currentTileBase;
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (isOver() == false)
                {
                    if (tilemap.GetTile(pos) != null)
                    {
                        if (tilemap.GetTile(pos) == downtilebase)
                        {
                            if (downtilebase == dirt)
                            {
                                tilemap.SetTile(pos, grass);
                                dirtPoses.Remove(pos);
                            }
                            if (downtilebase == grass)
                            {
                                tilemap.SetTile(pos, dirt);
                                dirtPoses.Add(pos);
                            }
                        }
                    }
                }
            }
        }
        if (type == interactType.Seed)
        {
            basket.gameObject.SetActive(true);
            basket.position = Input.mousePosition;
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                type = interactType.Move;
            }
        }
        else
        {
            basket.gameObject.SetActive(false);
        }
        if (type == interactType.MoveBuild)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,-10), Vector3.forward);
                if (hit.collider != null)
                {
                    if (hit.transform.tag == "Build")
                    {
                        hit.transform.gameObject.AddComponent<MoveBuild>();
                    }
                }
            }
        }
    }

    public void SelectShovel()
    {
        if (type == interactType.Shovel)
        {
            type = interactType.Move;
        }
        else
            type = interactType.Shovel;
    }
    public void SelectMoveBuild()
    {
        if (type == interactType.MoveBuild)
        {
            type = interactType.Move;
        }
        else
            type = interactType.MoveBuild;
    }

    public static bool isOver() => FindObjectsOfType<UIWindow>().ToList().Find(x => x.over == true) != null;
}
