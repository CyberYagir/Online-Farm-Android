using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool rus;
    public Camera camera;
    public GameObject drop;
    public List<Vector2> seedPoses = new List<Vector2>();
    public World world;
    [Space]
    public List<WorldPrefab> worldPrefabs;
    public List<Grow> grows;
    public float time;
    private void Start()
    {

        if (PlayerPrefs.HasKey("FarmTestLang"))
        {
            rus = PlayerPrefs.GetInt("FarmTestLang") == 1 ? true : false;
        }

        if (FindObjectOfType<WebManager>().saveOnReginstration)
        {
            FindObjectOfType<Stats>().nickname = FindObjectOfType<WebManager>().lg;
            StartCoroutine(FindObjectOfType<WebManager>().Save());
        }
        else
        {
            LoadFromJson(FindObjectOfType<WebManager>().userData.json);
        }
    }
    private void OnApplicationPause(bool pause)
    {
        FindObjectOfType<WebManager>().Save();
    }
    private void OnApplicationQuit()
    {
        FindObjectOfType<WebManager>().Save();
    }

    private void Update()
    {
        time += 1 * Time.deltaTime;
        if (time > 2)
        {
            StartCoroutine(saveLoop());
            time = 0;
        }
    }

    IEnumerator saveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(FindObjectOfType<WebManager>().Save());
        }
    }


    public string SaveWorldJson()
    {
        var stats = FindObjectOfType<Stats>();
        var player = FindObjectOfType<Player>();

        world = new World();
        world.wp = new WorldPlayer() { name = stats.nickname, xp = stats.exp, maxxp = stats.endXp, lvl = stats.lvl, money = stats.money, inv = World.ToInv(stats.inv) };

        world.objects = new List<WorldObject>();
        var objects = FindObjectsOfType<WorldPrefab>();
        for (int i = 0; i < objects.Length; i++)
        {
            world.objects.Add(new WorldObject() { name = objects[i].naming, pos = WorldVector.FromVector(objects[i].transform.position) });
        }

        world.grow = new List<WorldGrow>();
        var grows = FindObjectsOfType<Grow>();

        for (int i = 0; i < grows.Length; i++)
        {
            world.grow.Add(new WorldGrow() { end = grows[i].end, name = grows[i].name, start = grows[i].start, pos = WorldVector.FromVector(grows[i].transform.position) });
        }
        world.dirtPoses = new List<WorldVector>();
        for (int i = 0; i < player.dirtPoses.Count; i++)
        {
            world.dirtPoses.Add(WorldVector.FromVector(player.dirtPoses[i]));
        }
        return JsonUtility.ToJson(world);
    }

    public void LoadFromJson(string json)
    {
        var objects = FindObjectsOfType<WorldPrefab>();
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i].gameObject);
        }

        world = JsonUtility.FromJson<World>(json);

        for (int i = 0; i < world.objects.Count; i++)
        {
            for (int j = 0; j < worldPrefabs.Count; j++)
            {
                if (worldPrefabs[j].naming == world.objects[i].name)
                {
                    Instantiate(worldPrefabs[j].gameObject, WorldVector.ToVector2(world.objects[i].pos), Quaternion.identity);
                }
            }
        }
        for (int i = 0; i < world.grow.Count; i++)
        {
            for (int j = 0; j < grows.Count; j++)
            {
                if (grows[j].name == world.grow[i].name)
                {
                    var gm = Instantiate(grows[j].gameObject, WorldVector.ToVector2(world.grow[i].pos), Quaternion.identity);
                    var grow = gm.GetComponent<Grow>();
                    grow.setTime = true;
                    grow.start = world.grow[i].start;
                    grow.end = world.grow[i].end;
                }
            }
        }
        var player = FindObjectOfType<Player>();
        player.dirtPoses = new List<Vector3>();
        for (int i = 0; i < world.dirtPoses.Count; i++)
        {
            player.tilemap.SetTile(new Vector3Int((int)world.dirtPoses[i].x, (int)world.dirtPoses[i].y, 0), player.dirt);
            player.dirtPoses.Add(WorldVector.ToVector2(world.dirtPoses[i]));
        }

        var stats = FindObjectOfType<Stats>();
        stats.exp = world.wp.xp;
        stats.endXp = world.wp.maxxp;
        stats.money = world.wp.money;
        stats.nickname = world.wp.name;
        stats.inv = World.ToItems(world.wp.inv, stats);
    }


    public void Active(GameObject gm)
    {
        gm.SetActive(!gm.active);
    }



    public void Zoom(float speed)
    {
        camera.orthographicSize += speed * Time.deltaTime;
     
    }
    
}

[System.Serializable]
public class World { 
    public WorldPlayer  wp = new WorldPlayer();
    public List<WorldObject> objects = new List<WorldObject>();
    public List<WorldGrow> grow = new List<WorldGrow>();
    public List<WorldVector> dirtPoses = new List<WorldVector>();

    public static List<WorldItem> ToInv(List<Item> items)
    {
        var list = new List<WorldItem>();
        for (int i = 0; i < items.Count; i++)
        {
            list.Add(WorldItem.toWorldItem(items[i]));
        }
        return list;
    }

    public static List<Item> ToItems(List<WorldItem> items, Stats stats)
    {
        var list = new List<Item>();
        for (int i = 0; i < items.Count; i++)
        {
            var it = stats.GI(items[i].enname);
            it.val = items[i].val;
            list.Add(it);
        }
        return list;
    }
}

[System.Serializable]
public class WorldGrow
{
    public string name;
    public double start, end;
    public WorldVector pos;
}

[System.Serializable]
public class WorldObject
{
    public string name;
    public WorldVector pos;
}

[System.Serializable]
public class WorldVector {
    public float x, y;

    public static WorldVector FromVector(Vector2 vect) => new WorldVector() { x = vect.x, y = vect.y };

    public static Vector2 ToVector2(WorldVector v) => new Vector2(v.x, v.y);
}

[System.Serializable]
public class WorldPlayer {
    public int lvl, xp,maxxp, money;
    public string name;
    public List<WorldItem> inv = new List<WorldItem>();
}

[System.Serializable]
public class WorldItem {
    public string enname;
    public int val;

    public static WorldItem toWorldItem(Item item) => new WorldItem() { enname = item.enname, val = item.val };
    public Item toItem(Stats player) { var it = player.GI(enname); it.val = val; return it; }

}


