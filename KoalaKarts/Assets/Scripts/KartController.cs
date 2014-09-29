using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KartController : MonoBehaviour
{
    public int playerNumber = 1;

    // Michael: bundled wheelColliders and wheel Transforms into their own classes so they can be grouped
    //          (and folded) in the unity inspector, and to add setters that affect all wheels at once
    [System.Serializable]
    public class WheelColliders
    {
        public WheelCollider FL;
        public WheelCollider FR;
        public WheelCollider RL;
        public WheelCollider RR;
    }
    public WheelColliders wheelColliders = new WheelColliders();

    [System.Serializable]
    public class WheelTransforms
    {
        public Transform FL; 
        public Transform FR;
        public Transform RL;
        public Transform RR;

        public void SetLocalEulerAnglesLeft(Vector3 eulerAngles)
        {
            FL.localEulerAngles = eulerAngles;
            RL.localEulerAngles = eulerAngles;
        }

        public void SetLocalEulerAnglesRight(Vector3 eulerAngles)
        {
            FR.localEulerAngles = eulerAngles;
            RR.localEulerAngles = eulerAngles;
        }

        public void SetLocalEulerAnglesAll(Vector3 eulerAngles)
        {
            SetLocalEulerAnglesLeft(eulerAngles);
            SetLocalEulerAnglesRight(eulerAngles);
        }

        public void Rotate(WheelColliders colliders, float deltaTime)
        {
            FL.Rotate(0, colliders.FL.rpm / 60 * 360 * deltaTime, 0);
            FR.Rotate(0, colliders.FR.rpm / 60 * 360 * deltaTime, 0);
            RL.Rotate(0, colliders.RL.rpm / 60 * -360 * deltaTime, 0);
            RR.Rotate(0, colliders.RR.rpm / 60 * -360 * deltaTime, 0);
        }
    }
    public WheelTransforms wheelTransforms = new WheelTransforms();

    //public int lives = 3;
    //public int maxLives = 5;
    public Item currentItem = Item.Rocket;

    public float enginePower = 80.0f;
    public float maxSteer = 20.0f;

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

    public bool hoverEnabled = false;
    public float hoverHeight = 1.0f;
    public float hoverRotateSpeed = 35.0f;

    private string axisHorizontal;
    private string axisVertical;
    private string buttonBrake;
    private string buttonFire;
    private string buttonHover;

    [Tooltip("Here is some text describing this")]
	public Transform SpawnPoint;
    public Transform MineSpawnPoint;
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
        SpeedBoost,
        Rocket,
        Mine,
        Shield,
        Leaf
    }

	// Use this for initialization
	void Start ()
    {
        SetPlayer();
        rigidbody.centerOfMass = new Vector3(0, -1f, 0);
        SetValues();
        kartStatus = GetComponent<KartStatus>();
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
        forwardFriction = wheelColliders.RL.forwardFriction.stiffness;
        sidewaysFriction = wheelColliders.RL.sidewaysFriction.stiffness;
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
            currentSpeed = Mathf.Round(2 * Mathf.PI * wheelColliders.RL.radius * wheelColliders.RL.rpm * 60 / 1000);
            float power = Input.GetAxis(axisVertical) * enginePower * Time.deltaTime * speedModifier;

            float speedFactor = rigidbody.velocity.magnitude / maxSteerSpeed;
            float steer = Mathf.Lerp(lowSpeedSteerAngle, highSpeedSteerAngle, speedFactor);
            steer *= Input.GetAxis(axisHorizontal);
            //steer = Input.GetAxis("Horizontal") * maxSteer;

            float brake = Input.GetButton(buttonBrake) ? rigidbody.mass * 0.1f : 0.0f;

            wheelColliders.FL.steerAngle = steer;
            wheelColliders.FR.steerAngle = steer;

            if (brake > 0.0)
            {
                if (rigidbody.velocity.magnitude > 1)
                    SetSlip(slipForwardFriction, slipSidewaysFriction);
                else
                    SetSlip(1, 1);
                wheelColliders.FL.brakeTorque = brake;
                wheelColliders.FR.brakeTorque = brake;
                //wheelRL.brakeTorque = brake;
                //wheelRR.brakeTorque = brake;
                wheelColliders.RL.motorTorque = 0;
                wheelColliders.RR.motorTorque = 0;
            }
            else
            {
                //SetSlip(forwardFriction, sidewaysFriction);
                //axisValue = Input.GetAxis("Vertical");
                if (Input.GetAxis(axisVertical) == 0)
                {
                    wheelColliders.RL.brakeTorque = decelerationSpeed;
                    wheelColliders.RR.brakeTorque = decelerationSpeed;
                    wheelColliders.RL.motorTorque = 0;
                    wheelColliders.RR.motorTorque = 0;
                }
                else
                {
                    SetSlip(forwardFriction, sidewaysFriction);
                    wheelColliders.FL.brakeTorque = 0;
                    wheelColliders.FR.brakeTorque = 0;
                    wheelColliders.RL.brakeTorque = 0;
                    wheelColliders.RR.brakeTorque = 0;
                    
                    if (currentSpeed < topSpeed && currentSpeed > (-topSpeed / 2))
                    {
                        wheelColliders.RL.motorTorque = power;
                        wheelColliders.RR.motorTorque = power;
                    }
                    else
                    {
                        wheelColliders.RL.brakeTorque = decelerationSpeed;
                        wheelColliders.RR.brakeTorque = decelerationSpeed;
                        wheelColliders.RL.motorTorque = 0;
                        wheelColliders.RR.motorTorque = 0;
                    }
                }
            }
        }
        else
        {
            HoverMode();
        }
	}

    void Update()
    {
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
            wheelTransforms.Rotate(wheelColliders, Time.deltaTime);

            Vector3 wheelFLTransAngle = wheelTransforms.FL.localEulerAngles;
            Vector3 wheelFRTransAngle = wheelTransforms.FR.localEulerAngles;
            wheelFLTransAngle.y = wheelColliders.FL.steerAngle + 90 - wheelTransforms.FL.localEulerAngles.z;
            wheelFRTransAngle.y = wheelColliders.FR.steerAngle + 90 - wheelTransforms.FR.localEulerAngles.z;
            
            wheelTransforms.FL.localEulerAngles = wheelFLTransAngle;
            wheelTransforms.FR.localEulerAngles = wheelFRTransAngle;
        }

        if (rigidbody.velocity == Vector3.zero)
            rigidbody.angularVelocity = Vector3.zero;
    }

    void SetSlip(float currentForwardFriction, float currentSidewaysFriction)
    {
        var curve = wheelColliders.RL.forwardFriction;
        curve.stiffness = currentForwardFriction;
        wheelColliders.RL.forwardFriction = curve;

        curve = wheelColliders.RR.forwardFriction;
        curve.stiffness = currentForwardFriction;
        wheelColliders.RR.forwardFriction = curve;

        curve = wheelColliders.RL.sidewaysFriction;
        curve.stiffness = currentSidewaysFriction;
        wheelColliders.RL.sidewaysFriction = curve;

        curve = wheelColliders.RR.sidewaysFriction;
        curve.stiffness = currentSidewaysFriction;
        wheelColliders.RR.sidewaysFriction = curve;
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
            wheelTransforms.SetLocalEulerAnglesLeft(newLAngle);
            wheelTransforms.SetLocalEulerAnglesRight(newRAngle);
        }
        else
        {
            rigidbody.useGravity = true;
            wheelTransforms.SetLocalEulerAnglesAll(origAngle);
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

        float power = Input.GetAxis(axisVertical) * 40 * Time.deltaTime;
        float hpower = 0.0f;

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
            case Item.SpeedBoost:
				EnableSpeedBoost();
                break;
            case Item.Rocket:
				ShootRocket();
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

	void ShootRocket()
	{
		Object proj = Instantiate(Rocket, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
		proj.name = "Rocket";
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
            kartStatus.BankLeaves();
            Instantiate(Explosion, col.transform.position, col.transform.rotation);
            rigidbody.AddForce(new Vector3(0, 0, rigidbody.velocity.z * 5000 * -1), ForceMode.Impulse);
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
