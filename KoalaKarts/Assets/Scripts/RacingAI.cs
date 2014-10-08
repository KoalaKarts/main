using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RacingAI : MonoBehaviour {
	public KartStatus respawn;
	public KartController itemAI;

	private CharacterController character;

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
	//players + other AI
	public List<GameObject> enemies;
	//targetted enemy, closest enemy, or favorable enemy to take out
	public GameObject closestEnemy;
	public GameObject target;

	public GameObject self;

	/*AI ATTRIBUTES*/
	private Vector3 dir;
	public float speed = 0.2f;
	public bool hoverMode = false;
	public bool respawning = false;
	public float respawnTimer;
	public float respawnTime = 2;

	//public float speed = kc.GetComponent(speed);
	//public int hp = kh.GetComponent (leaves);
	//public int lives = kh.GetComponent();

	/*FIELD OF VISION*/
	float totalFOV = 70.0f;
	float rayRange = 100.0f;
	public float fovRange = 10.0f;//field of vision/view
	public Quaternion startAngle = Quaternion.AngleAxis (-60, Vector3.up);
	public Quaternion endAngle = Quaternion.AngleAxis (5, Vector3.up);
	
	void Start () {
		//character = GetComponent (CharacterController);

		currNode = nodeArr [0];
		nodeNum = 0;

		//combines both arrays into one list
		aiEnemies = GameObject.FindGameObjectsWithTag("AI");
		playerEnemies = GameObject.FindGameObjectsWithTag ("Kart");
		enemies = new List<GameObject>();
		for(int i = 0; i < aiEnemies.Length; i++){
			enemies.Add (aiEnemies[i]);
		}

		for(int i = 0; i < playerEnemies.Length; i++){
			enemies.Add (playerEnemies[i]);
		}
	}

	void Update () {
		if(transform.position.y < -200){
			respawn.Respawn ();
		}

		if(enemyFound == false){
			enemyInRange = false;
		}
		aiVision ();

		/*
		need hovermode somewhere here
		 */

		//searches for a nearby weapon
		if(!enemyFound && !hasWeapon){
			findWeapon();
		}

		//chases an enemy that is in range so that the  AI can use the weapon
		if(enemyFound && !enemyInRange && hasWeapon){
			chase();
		}

		//uses weapon on target
		if(enemyFound && enemyInRange && hasWeapon){
			//useWeapon();
		}

		if(!enemyFound && hasWeapon){
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

	//moves towards node, NEED TO FIX
	void findEnemy(){
		Vector3 newRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position).eulerAngles;
		newRotation.x = 0;
		newRotation.z = 0;

		dir = currNode.transform.position - transform.position;
		float dist = dir.magnitude;
		dir = dir.normalized;

		Vector3 mov = dir.normalized * speed / 5;
		float angleToTarget = Vector3.Angle (dir.normalized * speed, transform.forward);

		transform.Translate(mov);
		//transform.position += mov + transform.forward;

		/*same as below*/
		//Quaternion turn = Quaternion.LookRotation (dir);
		//transform.rotation = Quaternion.Slerp (transform.rotation, turn, 4 * Time.deltaTime);

		/*causes ai to try to bounce up at intended position*/
		//transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (dir), 4 * Time.deltaTime);
	}

	//NEED TO FIX?
	void findWeapon(){
		findClosestItemSpawn();
		dir = closestItem.transform.position - transform.position;
		Vector3 mov = dir.normalized * speed / 10;
		transform.position += mov;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (dir), 4 * Time.deltaTime);
		if(transform.position.x - closestItem.transform.position.x < 1 || 
		   transform.position.y - closestItem.transform.position.y < 1 ||
		   transform.position.z - closestItem.transform.position.z < 1){
			hasWeapon = true;
		}
	}

	void useWeapon(){
		itemAI.GetComponent<KartItemsController> ().UseItem ();
		hasWeapon = false;
	}

	//chases the found enemy
	void chase(){
		dir = closestEnemy.transform.position - transform.position;
		Vector3 mov = dir.normalized * speed / 2;
		transform.position += mov;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (dir), 4 * Time.deltaTime);

		/*
		need to add some other stuff to end the chase, NEED TO MODIFY/FIX/ETC.
		 */
	}

	//use if this character has less health then the target
	void runAway(){

	}

	void kartTransition(){

	}

	//ai's field of vision stuff
	//can find enemy, won't do anything though
	void aiVision(){
		detectEnemy ();
		inRange ();

		RaycastHit hit;
		Quaternion angle = transform.rotation * startAngle;
		Vector3 dire = angle * transform.forward;
		Vector3 pos = transform.position;

		//not working, but its supposed to show 24 red lines showing the supposed field of vision
		for(int i = 0; i < 24; i++){

			//NEED TO FIX/MODIFY MAYBE
			if(Physics.Raycast (pos, dire, 100, 150)){
				Debug.DrawLine (pos, transform.position, Color.red);
				if(hit.transform.tag == "Kart" || hit.transform.tag == "AI"){
					enemyFound = true;
				}

				else{
					enemyFound = false;
				}
			}
			dire = endAngle * dire;
		}
	}

	int ByDistance(GameObject a, GameObject b){
		float aDist = Vector3.Distance (transform.position, a.transform.position);
		float bDist = Vector3.Distance (transform.position, b.transform.position);
		return (int) aDist.CompareTo (bDist);
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

	GameObject detectEnemy(){
		float distance = Mathf.Infinity;
		enemies.Sort (ByDistance);

		foreach(GameObject cEnemy in enemies.ToArray ()){
			Vector3 dif = cEnemy.transform.position - transform.position;
			float cDis = dif.sqrMagnitude;
			if(cDis < distance){
				closestEnemy = cEnemy;
				distance = cDis;
				if(cEnemy == gameObject){
					enemies.Remove (cEnemy);
				}
			}
		}
		return closestEnemy;
	}

	//checks to see if an enemy is in range for ai to use weapon
	bool inRange(){
		float dist = Vector3.Distance (closestEnemy.transform.position, transform.position);
		if (dist < 70) {
			enemyInRange = true;
		} 
		else {
			enemyInRange = false;
		}
		return enemyInRange;
	}

	/*void stayOnGround(){
		Ray downRay = new Ray (transform.position, Vector3.down);
	}*/

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
