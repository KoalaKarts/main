using UnityEngine;
using System.Collections;

public class RacingAI : MonoBehaviour {
	public KartController kc;
	public KartStatus kh;
	public ItemSpawner its;

	/*NODE STUFF*/
	//holds all of the nodes
	public Transform[] nodeArr;
	//node the ai will go to, testing with this for now
	private Transform currNode;
	private int nodeNum;
	public int atNode;//debugging purposes with the nodes

	/*WEAPON/ITEM STUFF*/
	public bool hasWeapon = false;
	public GameObject[] itemSpawns;
	public GameObject closestItem;

	/*COMBAT STUFF*/
	public bool enemyFound = false;
	public bool enemyInRange = false;
	//other AI
	public GameObject[] aiEnemies;
	//players
	public GameObject[] playerEnemies;
	public GameObject[] enemies;
	//targetted enemy, closest enemy, or favorable enemy to take out
	public Transform[] target;
	private GameObject enemy;

	/*AI ATTRIBUTES*/
	private Vector3 dir;
	public float speed = 0.2f;
	public float fovRange = 10.0f;//field of vision/view
	public Quaternion startAngle = Quaternion.AngleAxis (-60, Vector3.up);
	public Quaternion endAngle = Quaternion.AngleAxis (5, Vector3.up);

	//public float speed = kc.GetComponent(speed);
	//public int hp = kh.GetComponent (leaves);
	//public int lives = kh.GetComponent();

	/*FIELD OF VISION*/
	float totalFOV = 70.0f;
	float rayRange = 100.0f;
	
	void Start () {
		currNode = nodeArr [0];
		nodeNum = 0;

		//combines both enemy arrays into one....hopefully
		aiEnemies = GameObject.FindGameObjectsWithTag ("AI");
		playerEnemies = GameObject.FindGameObjectsWithTag ("Kart");
		enemies = new GameObject[aiEnemies.Length + playerEnemies.Length];
		aiEnemies.CopyTo (enemies, 0);
		playerEnemies.CopyTo (enemies, aiEnemies.Length);
	}

	void Update () {
		aiVision ();

		//searches for a nearby weapon
		if(!enemyFound && !hasWeapon){
			findWeapon();
		}

		//chases an enemy that is in range so that the  AI can use the weapon
		if(enemyFound && !enemyInRange && hasWeapon){
			//chase();
		}

		//uses weapon on target
		if(enemyFound && enemyInRange && hasWeapon){
			//useWeapon();
		}

		if(!enemyFound && hasWeapon){// keep hasWeapon false for now, weapons aren't in yet/doesn't work or something
			findEnemy ();
			if(Vector3.Distance (currNode.transform.position, transform.position) < nodeArr.Length){
				nodeNum++;
				if(nodeNum > nodeArr.Length - 1){
					nodeNum = 0;
				}
				currNode = nodeArr[nodeNum];
				atNode = nodeNum;
			}
		}
	}

	//moves towards node
	void findEnemy(){
		dir = currNode.transform.position - transform.position;
		Vector3 mov = dir.normalized * speed / 3;
		transform.position += mov;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (dir), 4 * Time.deltaTime);
	}

	void findWeapon(){
		findClosestItemSpawn();
		dir = closestItem.transform.position - transform.position;
		Vector3 mov = dir.normalized * speed / 2;
		transform.position += mov;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (dir), 4 * Time.deltaTime);
		if(transform.position.y - closestItem.transform.position.y < 1){//MIGHT NEED TO FIX SINCE IT ONLY CHECKS FOR THE Y POSITION FOR BOTH
			hasWeapon = true;
		}
	}

	void useWeapon(){
		hasWeapon = false;
	}

	void chase(){

	}

	void runAway(){

	}

	void kartTransition(){

	}

	//ai's field of vision...not sure if this works or not yet, will probably need to modify
	void aiVision(){
		//OnDrawGizmosSelected ();

		RaycastHit hit;
		Quaternion angle = transform.rotation * startAngle;
		Vector3 dire = angle * Vector3.forward;
		Vector3 pos = transform.position;
		for(int i = 0; i < 24; i++){
			//Debug.DrawLine (enemy.position, transform.position, Color.red);

			if(Physics.Raycast (pos, dire, out hit, 500)){
				Enemy enemy = hit.collider.GetComponent<Enemy>();
				if(enemy){
					enemyFound = true;
				}
			}
		}
	}

	//shows the field of vision???
	void OnDrawGizmosSelected(){
		float halfFOV = totalFOV / 2.0f;
		float quarterFOV = totalFOV / 4.0f; //for enemyInRange

		Quaternion leftRayRot = Quaternion.AngleAxis (-halfFOV, Vector3.up);
		Quaternion quarLeftRayRot = Quaternion.AngleAxis (-quarterFOV, Vector3.up);
		Quaternion rightRayRot = Quaternion.AngleAxis (halfFOV, Vector3.up);
		Quaternion quarRightRayRot = Quaternion.AngleAxis (quarterFOV, Vector3.up);

		Vector3 leftRayDir = leftRayRot * transform.forward;
		Vector3 quarLeftRayDir = quarLeftRayRot * transform.forward;
		Vector3 rightRayDir = rightRayRot * transform.forward;
		Vector3 quarRightRayDir = quarRightRayRot * transform.forward;

		Gizmos.color = Color.blue;
		Gizmos.DrawRay (transform.position, leftRayDir * rayRange);
		Gizmos.DrawRay (transform.position, rightRayDir * rayRange);

		Gizmos.color = Color.cyan;
		Gizmos.DrawRay (transform.position, quarLeftRayDir * rayRange / 1.5f);
		Gizmos.DrawRay (transform.position, quarRightRayDir * rayRange / 1.5f);
	}

	GameObject findClosestItemSpawn(){
		float distance = Mathf.Infinity;

		foreach (GameObject iSpawn in itemSpawns) {
			Vector3 dif = iSpawn.transform.position - transform.position;
			float cDis = dif.sqrMagnitude;
			if(cDis < distance){
				closestItem = iSpawn;
				distance = cDis;
			}
		}
		return closestItem;
	}

	/*MANDATORY AI NEEDED:
	 * chase- will chase an enemy in the shortest path possible
	 * find enemy:need to work on turning and stuff
	 * run away-drive away as fast as possible if low on health
	 * use weapon- just uses the weapon
	 * find weapon:need to adjust the way it checks to see if it has collided/gotten an item
	 * move to next node-need to implement somehow
	 * turn-might be its own function or not
	 * drifting-
	 * kart transition-
	 * item spawn respawner-if an item spawn is destroyed, after a short amount of time respawn the item spawner
	 */

	/*
	 * MORE SOPHISTOCATED AI STUFF (only use if everything else is done):
	 *  sneak-take a longer way in order to try to kill an enemy as stealthily as possible (need to redefine possibly)
		bait???-(not sure if needed, need to ask if weapons being held can be seen by others)
		blocking-if a projectile is targetting self, then this character will try to go behind either an enemy or obstacle to avoid the damage from the projectile
		swarm-if an enemy is detected and can only get out in limited ways, then this character will 'trap' the target
	 */
}
