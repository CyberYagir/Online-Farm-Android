using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Select : MonoBehaviour
{

    public Tilemap tileMap;
    public int z = 10;
    void Start()
    {
        
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TileBase tileBase;

            for (int i = 10; i >= 0; i--)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tileBase = tileMap.GetTile(Vector3Int.CeilToInt(new Vector3(pos.x, pos.y, i)));
                if (tileBase != null)
                {
                    print(tileBase.name + "|" + i);
                }
            }
           
        }
    }
}
