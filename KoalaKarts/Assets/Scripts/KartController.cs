using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KartController : MonoBehaviour
{
    public int playerNumber = 1;
    
    [System.Serializable]
    public class WheelColliders
    {
        public WheelCollider FL, FR, RL, RR;
    }

    [System.Serializable]
    public class WheelGeometry
    {
        public Transform FL, FR, RL, RR;
    }

    public WheelColliders Colliders;
    public WheelGeometry Geometry;

    //public int lives = 3;
    //public int maxLives = 5;
    public Item currentItem = Item.Rocket;

    public float enginePower = 80.0f;
    public float maxSteer = 20.0f;

    public float power = 0.0f;
    public float hpower = 0.0f;
    public float brake = 0.0f;
    public float steer = 0.0f;

    public float maxSteerSpeed = 50;
    public float lowSpeedSteerAngle = 20;
    public float highSpeedSteerAngle = 6;
    public float decelerationSpeed = 50;

    public float currentSpeed;
	public float speedModifier = 150.0f;
    public float topSpeed = 250;

    private float sidewaysFriction;
    private float forwardFriction;
    private float slipSidewaysFriction;
    private float slipForwardFriction;

    public float axisValue;

    public bool hoverEnabled = false;
    public float hoverHeight = 1.0f;
    public float hoverRotateSpeed = 35.0f;

    private string axisHorizontal;
    private string axisVertical;
    private string buttonBrake;
    private string buttonFire;
    private string buttonHover;

	public Transform SpawnPoint;
    public Transform MineSpawnPoint;
    public GameObject RangARang;
	public GameObject Rocket;
    public GameObject Mine;
    public GameObject Shield;
    public GameObject Explosion;
    public GameObject LifeLeaf1;
    public GameObject LifeLeaf2;

	private bool boostEnabled = false;
	private float speedBoostTimer = 5.0f;

    public bool shieldEnabled = false;
    private float shieldTimer = 5.0f;

    public AudioSource healthPickupAudio;
    public AudioSource shieldPickupAudio;
    public AudioSource speedBoostAudio;

    public KartStatus kartStatus;

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

	// Use this for initialization
	void Start ()
    {
        SetPlayer();
        rigidbody.centerOfMass = new Vector3(0, -1f, 0);
        SetValues();
        kartStatus = GetComponent<KartStatus>();
	}

    void OnGUI()
    {
        if (playerNumber == 1)
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Leaves: " + kartStatus.GetCurrentLeaves());
            GUI.Label(new Rect(10, 30, 100, 20), "Points: " + kartStatus.GetCurrentPoints());
            GUI.Label(new Rect(10, 50, 200, 20), "Item: " + currentItem.ToString());
        }
    }

    void SetPlayer()
    {
        if (playerNumber == 1)
        {
            axisHorizontal = "Horizontal";
            axisVertical = "Vertical";
            buttonBrake = "Brake";
            buttonFire = "Fire";
            buttonHover = "Hover";
        }
        else
        {
            axisHorizontal = "Horizontal2";
            axisVertical = "Vertical2";
            buttonBrake = "Brake2";
            buttonFire = "Fire2";
            buttonHover = "Hover2";
        }
    }

    void SetValues()
    {
        forwardFriction = Colliders.RL.forwardFriction.stiffness;
        sidewaysFriction = Colliders.RL.sidewaysFriction.stiffness;
        slipForwardFriction = 0.04f;
        slipSidewaysFriction = 0.08f;
    }

	void FixedUpdate ()
    {
        if (Input.GetButtonUp(buttonHover) && currentSpeed == 0.0f)
        {
            ToggleHoverMode();
        }

        if (!hoverEnabled)
        {
            currentSpeed = Mathf.Round(2 * Mathf.PI * Colliders.RL.radius * Colliders.RL.rpm * 60 / 1000);
            power = Input.GetAxis(axisVertical) * enginePower * Time.deltaTime * speedModifier;

            float speedFactor = rigidbody.velocity.magnitude / maxSteerSpeed;
            steer = Mathf.Lerp(lowSpeedSteerAngle, highSpeedSteerAngle, speedFactor);
            steer *= Input.GetAxis(axisHorizontal);
            //steer = Input.GetAxis("Horizontal") * maxSteer;

            brake = Input.GetButton(buttonBrake) ? rigidbody.mass * 0.1f : 0.0f;

            Colliders.FL.steerAngle = steer;
            Colliders.FR.steerAngle = steer;

            Brake();
        }
        else
        {
            HoverMode();
        }
	}

    void Brake()
    {
        if (brake > 0.0)
        {
            if (rigidbody.velocity.magnitude > 1)
                SetSlip(slipForwardFriction, slipSidewaysFriction);
            else
                SetSlip(1, 1);
            Colliders.FL.brakeTorque = brake;
            Colliders.FR.brakeTorque = brake;
            //wheelRL.brakeTorque = brake;
            //wheelRR.brakeTorque = brake;
            Colliders.RL.motorTorque = 0;
            Colliders.RR.motorTorque = 0;
        }
        else
        {
            //SetSlip(forwardFriction, sidewaysFriction);
            //axisValue = Input.GetAxis("Vertical");
            if (Input.GetAxis(axisVertical) == 0)
            {
                Colliders.RL.brakeTorque = decelerationSpeed;
                Colliders.RR.brakeTorque = decelerationSpeed;
                Colliders.RL.motorTorque = 0;
                Colliders.RR.motorTorque = 0;
            }
            else
            {
                SetSlip(forwardFriction, sidewaysFriction);
                Colliders.FL.brakeTorque = 0;
                Colliders.FR.brakeTorque = 0;
                Colliders.RL.brakeTorque = 0;
                Colliders.RR.brakeTorque = 0;
                if (currentSpeed < topSpeed && currentSpeed > (-topSpeed / 2))
                {
                    Colliders.RL.motorTorque = power;
                    Colliders.RR.motorTorque = power;
                }
                else
                {
                    Colliders.RL.brakeTorque = decelerationSpeed;
                    Colliders.RR.brakeTorque = decelerationSpeed;
                    Colliders.RL.motorTorque = 0;
                    Colliders.RR.motorTorque = 0;
                }
            }
        }
    }

    void Update()
    {
        if (transform.position.y < -200)
        {
            kartStatus.Respawn();
        }

        if (Input.GetButtonDown(buttonFire))
        {
            UseItem();
        }

        if (boostEnabled)
        {
            if (speedBoostTimer > 0)
            {
                speedBoostTimer -= Time.deltaTime;
            }
            else
            {
                DisableSpeedBoost();
            }
        }

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

        if (!hoverEnabled)
        {
            Geometry.FL.Rotate(0, Colliders.FL.rpm / 60 * 360 * Time.deltaTime, 0);
            Geometry.FR.Rotate(0, Colliders.FR.rpm / 60 * 360 * Time.deltaTime, 0);
            Geometry.RL.Rotate(0, Colliders.RL.rpm / 60 * -360 * Time.deltaTime, 0);
            Geometry.RR.Rotate(0, Colliders.RR.rpm / 60 * -360 * Time.deltaTime, 0);

            Vector3 wheelFLTransAngle = Geometry.FL.localEulerAngles;
            Vector3 wheelFRTransAngle = Geometry.FR.localEulerAngles;
            wheelFLTransAngle.y = Colliders.FL.steerAngle + 90 - Geometry.FL.localEulerAngles.z;
            wheelFRTransAngle.y = Colliders.FR.steerAngle + 90 - Geometry.FR.localEulerAngles.z;
            Geometry.FL.localEulerAngles = wheelFLTransAngle;
            Geometry.FR.localEulerAngles = wheelFRTransAngle;
        }

        if (rigidbody.velocity == Vector3.zero)
            rigidbody.angularVelocity = Vector3.zero;
    }

    void SetSlip(float currentForwardFriction, float currentSidewaysFriction)
    {
        var curve = Colliders.RL.forwardFriction;
        curve.stiffness = currentForwardFriction;
        Colliders.RL.forwardFriction = curve;

        curve = Colliders.RR.forwardFriction;
        curve.stiffness = currentForwardFriction;
        Colliders.RR.forwardFriction = curve;

        curve = Colliders.RL.sidewaysFriction;
        curve.stiffness = currentSidewaysFriction;
        Colliders.RL.sidewaysFriction = curve;

        curve = Colliders.RR.sidewaysFriction;
        curve.stiffness = currentSidewaysFriction;
        Colliders.RR.sidewaysFriction = curve;
    }

    void ToggleHoverMode()
    {
        Vector3 origAngle = new Vector3(90, 270, 0);
        Vector3 newLAngle = new Vector3(180, 270, 0);
        Vector3 newRAngle = new Vector3(0, 270, 0);

        hoverEnabled = !hoverEnabled;

        if (hoverEnabled)
        {
            rigidbody.useGravity = false;
            Geometry.FL.localEulerAngles = newLAngle;
            Geometry.FR.localEulerAngles = newRAngle;
            Geometry.RL.localEulerAngles = newLAngle;
            Geometry.RR.localEulerAngles = newRAngle;
        }
        else
        {
            rigidbody.useGravity = true;
            Geometry.FL.localEulerAngles = origAngle;
            Geometry.FR.localEulerAngles = origAngle;
            Geometry.RL.localEulerAngles = origAngle;
            Geometry.RR.localEulerAngles = origAngle;
        }
    }

    void HoverMode()
    {
        if (Physics.Raycast(transform.position, Vector3.down, hoverHeight))
        {
            rigidbody.useGravity = false;
            rigidbody.transform.position += Vector3.up * 5 * Time.deltaTime;
            rigidbody.drag = 20.0f;
        }
        else
        {
            if (!Physics.Raycast(transform.position, Vector3.down, hoverHeight + 1.0f))
            {
                rigidbody.useGravity = true;
            }
            rigidbody.drag = 1.0f;
        }

        power = Input.GetAxis(axisVertical) * 40 * Time.deltaTime;

        if (!Input.GetButton(buttonBrake))
        {
            hpower = Input.GetAxis(axisHorizontal) * 40 * Time.deltaTime;
            transform.Translate(new Vector3(hpower, 0, power));
        }
        else
        {
            float rotation = Input.GetAxis(axisHorizontal) * hoverRotateSpeed * Time.deltaTime;
            hpower = Input.GetAxis(axisHorizontal) * 40 * Time.deltaTime;
            transform.Translate(new Vector3(0, 0, power));
            transform.Rotate(0, rotation, 0);
        }
    }

    void UseItem()
    {
        switch (currentItem)
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
		currentItem = Item.NULL;
    }

	void EnableSpeedBoost ()
	{
		boostEnabled = true;
        speedBoostAudio.Play();
		speedModifier = 250.0f;
		topSpeed = 400.0f;
	}

	void DisableSpeedBoost()
	{
		boostEnabled = false;
        speedBoostTimer = 5.0f;
		speedModifier = 150.0f;
		topSpeed = 250.0f;
	}

	void ShootItem()
	{
        Object proj;
        switch (currentItem)
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

    void Die()
    {
        Application.LoadLevel("Menus");
    }

    void AddItem(Item item)
    {
        if (item == Item.Leaf)
        {
            kartStatus.AddLeaf();
        }
        else if(currentItem == Item.NULL)
        {
            currentItem = item;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Kart")
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (col.gameObject.name == "Bank")
        {
            if (kartStatus.GetCurrentLeaves() > 0)
            {
                kartStatus.BankLeaves();
                Instantiate(Explosion, col.transform.position, col.transform.rotation);
            }
            rigidbody.AddForce(new Vector3(0, 0, rigidbody.velocity.z * 2000 * -1), ForceMode.Impulse);
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Kart")
            rigidbody.constraints = RigidbodyConstraints.None;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Mine")
        {
            if (!shieldEnabled && !hoverEnabled)
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
}
