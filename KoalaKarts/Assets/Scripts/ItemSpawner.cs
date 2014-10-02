using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    public enum Item
    {
        NULL,
        Leaf,
        Rocket,
        RangARang,
        SpeedBoost,
        Mine,
        Shield,
    }

    public bool healthSpawner = false;

    public float spawnTime = 10;
	public float spawnTimer;
    public Item currentItem = Item.NULL;

    public GameObject itemRocket;
    public GameObject itemRangARang;
    public GameObject itemSpeedBoost;
    public GameObject itemMine;
    public GameObject itemShield;
    public GameObject itemLeaf;

	// Use this for initialization
	void Start ()
    {
		SpawnItem();
		spawnTimer = spawnTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (currentItem == Item.NULL)
		{
			if (spawnTimer > 0)
			{
				spawnTimer -= Time.deltaTime;
			}
			else
			{
				SpawnItem();
				spawnTimer = spawnTime;
			}
		}
	}

	public Item GetItem()
	{
		return currentItem;
	}

    void SpawnItem()
    {
        Item item;
        if (healthSpawner)
        {
            item = Item.Leaf;
        }
        else
        {
            item = GetRandomItem<Item>();
        }

		GameObject obj;
        switch (item)
        {
            case Item.NULL:
                currentItem = Item.NULL;
                break;
            case Item.Leaf:
                currentItem = Item.Leaf;
                obj = (GameObject)Instantiate(itemLeaf, new Vector3(transform.position.x, transform.position.y + 3.0f, transform.position.z), transform.rotation);
                obj.transform.parent = transform;
                obj.name = "LeafItem";
                break;
            case Item.Rocket:
                currentItem = Item.Rocket;
				obj = (GameObject) Instantiate(itemRocket, transform.position, transform.rotation);
				obj.transform.parent = transform;
				obj.name = "RocketItem";
                break;
            case Item.RangARang:
                currentItem = Item.RangARang;
                obj = (GameObject)Instantiate(itemRangARang, transform.position, transform.rotation);
                obj.transform.parent = transform;
                obj.name = "RangARangItem";
                break;
            case Item.SpeedBoost:
                currentItem = Item.SpeedBoost;
				obj = (GameObject) Instantiate(itemSpeedBoost, transform.position, transform.rotation);
				obj.transform.parent = transform;
				obj.name = "SpeedBoostItem";
                break;
            case Item.Mine:
                currentItem = Item.Mine;
                obj = (GameObject) Instantiate(itemMine, transform.position, transform.rotation);
				obj.transform.parent = transform;
				obj.name = "MineItem";
                break;
            case Item.Shield:
                currentItem = Item.Shield;
				obj = (GameObject) Instantiate(itemShield, transform.position, transform.rotation);
				obj.transform.parent = transform;
				obj.name = "ShieldItem";
                break;
            
            default:
                currentItem = Item.NULL;
                break;
        }
    }

	public void PickUp()
	{
		currentItem = Item.NULL;
		if (transform.childCount > 0)
		{
			Destroy (transform.GetChild (0).gameObject);
		}
	}

    void onTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "PlayerOneKart" || col.gameObject.name == "PlayerTwoKart")
        {
            currentItem = Item.NULL;
        }
    }

    static T GetRandomItem<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(1, A.Length));
        return V;
    }
}
