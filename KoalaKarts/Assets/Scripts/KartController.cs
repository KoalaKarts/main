using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KartController : MonoBehaviour
{
    public int playerNumber = 1;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    public Transform wheelFLTransform;
    public Transform wheelFRTransform;
    public Transform wheelRLTransform;
    public Transform wheelRRTransform;

    public int lives = 3;
    public int maxLives = 5;
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
        forwardFriction = wheelRL.forwardFriction.stiffness;
        sidewaysFriction = wheelRL.sidewaysFriction.stiffness;
        slipForwardFriction = 0.04f;
        slipSidewaysFriction = 0.08f;
    }

	void FixedUpdate ()
    {
        transform.rotation.x = Mathf.Clamp(transform.rotation.x, -45.0f, 45.0f);


        if (Input.GetButtonUp(buttonHover) && currentSpeed == 0.0f)
        {
            ToggleHoverMode();
        }

        if (!hoverEnabled)
        {
            currentSpeed = Mathf.Round(2 * Mathf.PI * wheelRL.radius * wheelRL.rpm * 60 / 1000);
            power = Input.GetAxis(axisVertical) * enginePower * Time.deltaTime * speedModifier;

            float speedFactor = rigidbody.velocity.magnitude / maxSteerSpeed;
            steer = Mathf.Lerp(lowSpeedSteerAngle, highSpeedSteerAngle, speedFactor);
            steer *= Input.GetAxis(axisHorizontal);
            //steer = Input.GetAxis("Horizontal") * maxSteer;

            brake = Input.GetButton(buttonBrake) ? rigidbody.mass * 0.1f : 0.0f;

            wheelFL.steerAngle = steer;
            wheelFR.steerAngle = steer;

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
            wheelFL.brakeTorque = brake;
            wheelFR.brakeTorque = brake;
            //wheelRL.brakeTorque = brake;
            //wheelRR.brakeTorque = brake;
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
        }
        else
        {
            //SetSlip(forwardFriction, sidewaysFriction);
            //axisValue = Input.GetAxis("Vertical");
            if (Input.GetAxis(axisVertical) == 0)
            {
                wheelRL.brakeTorque = decelerationSpeed;
                wheelRR.brakeTorque = decelerationSpeed;
                wheelRL.motorTorque = 0;
                wheelRR.motorTorque = 0;
            }
            else
            {
                SetSlip(forwardFriction, sidewaysFriction);
                wheelFL.brakeTorque = 0;
                wheelFR.brakeTorque = 0;
                wheelRL.brakeTorque = 0;
                wheelRR.brakeTorque = 0;
                if (currentSpeed < topSpeed && currentSpeed > (-topSpeed / 2))
                {
                    wheelRL.motorTorque = power;
                    wheelRR.motorTorque = power;
                }
                else
                {
                    wheelRL.brakeTorque = decelerationSpeed;
                    wheelRR.brakeTorque = decelerationSpeed;
                    wheelRL.motorTorque = 0;
                    wheelRR.motorTorque = 0;
                }
            }
        }
    }

    void Update()
    {
		if (Input.GetButtonDown (buttonFire))
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
            wheelFLTransform.Rotate(0, wheelFL.rpm / 60 * 360 * Time.deltaTime, 0);
            wheelFRTransform.Rotate(0, wheelFR.rpm / 60 * 360 * Time.deltaTime, 0);
            wheelRLTransform.Rotate(0, wheelRL.rpm / 60 * -360 * Time.deltaTime, 0);
            wheelRRTransform.Rotate(0, wheelRR.rpm / 60 * -360 * Time.deltaTime, 0);

            Vector3 wheelFLTransAngle = wheelFLTransform.localEulerAngles;
            Vector3 wheelFRTransAngle = wheelFRTransform.localEulerAngles;
            wheelFLTransAngle.y = wheelFL.steerAngle + 90 - wheelFLTransform.localEulerAngles.z;
            wheelFRTransAngle.y = wheelFR.steerAngle + 90 - wheelFRTransform.localEulerAngles.z;
            wheelFLTransform.localEulerAngles = wheelFLTransAngle;
            wheelFRTransform.localEulerAngles = wheelFRTransAngle;
        }

        if (rigidbody.velocity == Vector3.zero)
            rigidbody.angularVelocity = Vector3.zero;
    }

    void SetSlip(float currentForwardFriction, float currentSidewaysFriction)
    {
        var curve = wheelRL.forwardFriction;
        curve.stiffness = currentForwardFriction;
        wheelRL.forwardFriction = curve;

        curve = wheelRR.forwardFriction;
        curve.stiffness = currentForwardFriction;
        wheelRR.forwardFriction = curve;

        curve = wheelRL.sidewaysFriction;
        curve.stiffness = currentSidewaysFriction;
        wheelRL.sidewaysFriction = curve;

        curve = wheelRR.sidewaysFriction;
        curve.stiffness = currentSidewaysFriction;
        wheelRR.sidewaysFriction = curve;
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
            wheelFLTransform.localEulerAngles = newLAngle;
            wheelFRTransform.localEulerAngles = newRAngle;
            wheelRLTransform.localEulerAngles = newLAngle;
            wheelRRTransform.localEulerAngles = newRAngle;
        }
        else
        {
            rigidbody.useGravity = true;
            wheelFLTransform.localEulerAngles = origAngle;
            wheelFRTransform.localEulerAngles = origAngle;
            wheelRLTransform.localEulerAngles = origAngle;
            wheelRRTransform.localEulerAngles = origAngle;
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

    public void SubtractLife()
    {
        if (lives > 1)
        {
            lives--;

            if (lives == 2)
            {
                Destroy(transform.Find("LifeLeaf2").gameObject);
            }

            if (lives == 1)
            {
                Destroy(transform.Find("LifeLeaf1").gameObject);
            }
        }
        else
        {
            Die();
        }
    }

    void AddLife()
    {
        if (lives < maxLives)
        {
            lives++;

            if (lives == 2)
            {
                GameObject leaf;
                leaf = (GameObject)Instantiate(LifeLeaf1, transform.position, transform.rotation);
                leaf.transform.parent = transform;
                leaf.name = "LifeLeaf1";
            }

            if (lives == 3)
            {
                GameObject leaf;
                leaf = (GameObject)Instantiate(LifeLeaf2, transform.position, transform.rotation);
                leaf.transform.parent = transform;
                leaf.name = "LifeLeaf2";
            }
        }
    }

    void Die()
    {
        Application.LoadLevel("Menus");
    }

    void AddItem(Item item)
    {
        if (item == Item.Leaf)
        {
            AddLife();
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
                SubtractLife();
                Instantiate(Explosion, other.transform.position, other.transform.rotation);
                rigidbody.AddExplosionForce(500000.0f, other.transform.position, 30.0f, 5.0f);
                rigidbody.AddTorque(Vector3.up * 10000000.0f);
                Destroy(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Health Leaf")
        {
            Destroy(other.gameObject);
            AddLife();
            healthPickupAudio.Play();
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
