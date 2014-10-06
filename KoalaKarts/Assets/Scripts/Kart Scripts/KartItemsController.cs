using UnityEngine;
using System.Collections;

/// <summary>
/// Hold all the item GameObjects that can instantiated by karts
/// Responsible for collecting objects off the course
/// Knows which item the koala has
/// </summary>
public class KartItemsController : MonoBehaviour 
{
	public Transform SpawnPoint;
    public Transform MineSpawnPoint;
    public GameObject RangARang;
	public GameObject Rocket;
    public GameObject Mine;
    public GameObject Shield;
    public GameObject Explosion;
    public GameObject LifeLeaf1;
    public GameObject LifeLeaf2;

    public bool shieldEnabled = false;
    private float shieldTimer = 5.0f;

    public AudioSource healthPickupAudio;
    public AudioSource shieldPickupAudio;
    public AudioSource speedBoostAudio;

    public KartStatus kartStatus;
    private KartMotionController kartMotion; 

    public Item CurrentItem = Item.Rocket;

	void Start () 
    {
        kartStatus = GetComponent<KartStatus>();
	}
	
	void Update () 
    {
        if (shieldEnabled)
        {
            if (shieldTimer > 0)
            {
                shieldTimer -= Time.deltaTime;
            }
            else
            {
                DisableShield();
            }
        }
	}

    public void UseItem()
    {
        switch (CurrentItem)
        {
            case Item.NULL:
                break;
            case Item.RangARang:
                ShootItem();
                break;
            case Item.Rocket:
				ShootItem();
                break;
            case Item.SpeedBoost:
				EnableSpeedBoost();
                break;
            case Item.Mine:
                PlaceMine();
                break;
            case Item.Shield:
                DeployShield();
                break;
        }
		CurrentItem = Item.NULL;
    }

    void EnableSpeedBoost ()
    {
    //    boostEnabled = true;
    //    speedModifier = 250.0f;
    //    topSpeed = 400.0f;
        speedBoostAudio.Play();
        kartMotion.Boost();
    }

    //void DisableSpeedBoost()
    //{
    //    boostEnabled = false;
    //    speedBoostTimer = 5.0f;
    //    speedModifier = 150.0f;
    //    topSpeed = 250.0f;
    //}

	void ShootItem()
	{
        Object proj;
        switch (CurrentItem)
        {
            case Item.Rocket:
                proj = Instantiate(Rocket, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
                proj.name = "Rocket";
                break;
            case Item.RangARang:
                proj = Instantiate(RangARang, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
                proj.name = "RangARang";
                break;
        }
	}

    void PlaceMine()
    {
        Object mine = Instantiate(Mine, MineSpawnPoint.transform.position, MineSpawnPoint.transform.rotation);
        mine.name = "Mine";
    }

    void DeployShield()
    {
        GameObject shield;
        shield = (GameObject)Instantiate(Shield, transform.position, transform.rotation);
        shield.transform.parent = transform;
        shield.name = "Shield";
        shieldEnabled = true;
    }

    void DisableShield()
    {
        shieldEnabled = false;
        shieldTimer = 5.0f;
        Destroy(transform.Find("Shield").gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Mine")
        {
            if (!shieldEnabled && kartStatus.GetKartMode() == KartMode.Kart)
            {
                Instantiate(Explosion, other.transform.position, other.transform.rotation);
                rigidbody.AddExplosionForce(500000.0f, other.transform.position, 30.0f, 5.0f);
                rigidbody.AddTorque(Vector3.up * 10000000.0f);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Health Leaf")
        {
            Destroy(other.gameObject);
            kartStatus.AddLeaf();
            healthPickupAudio.Play();
        }

        if (other.gameObject.name == "Mine")
        {
            Destroy(other.gameObject);
            kartStatus.Hit();
        }

        /*if (other.gameObject.name == "SpeedBoost")
        {
            Destroy(other.gameObject);
            AddItem(Item.SpeedBoost);
        }*/

		if (other.GetComponent<ItemSpawner>())
		{
			Item item = (Item) other.GetComponent<ItemSpawner>().GetItem();
			other.GetComponent<ItemSpawner>().PickUp();
            shieldPickupAudio.Play();
			AddItem(item);
		}
    }

    void AddItem(Item item)
    {
        if (item == Item.Leaf)
        {
            kartStatus.AddLeaf();
        }
        else if (CurrentItem == Item.NULL)
        {
            CurrentItem = item;
        }
    }
}
