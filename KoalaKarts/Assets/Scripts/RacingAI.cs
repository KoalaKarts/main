using UnityEngine;
using System.Collections;

public class RacingAI : MonoBehaviour {
	public KartController kc;
	public KartHealth kh;

	//where the ai goes to when finding enemy, overall node stuff
	public GameObject node1;
	public GameObject node2;
	public GameObject node3;
	public GameObject node4;
	public GameObject node5;
	public GameObject node6;
	public GameObject node7;
	public GameObject node8;
	public GameObject node9;
	private Vector3 nodeStart;
	private Vector3 nodeEnd;
	public int nodeNum;
	public GameObject[] nodeArr;
	//lower the number = go to first
	public int nodePriority = 1; 
	//range of nodes which will allow the ai to do certain actions based on what is nearby, need to word better and need to look into more
	public int nodeRange;
	//node the ai will go to, testing with this for now
	public GameObject nodeTar;

	//weapon(s) that this character is holding
	public GameObject weapon;
	//other players
	public GameObject enemy;
	//targetted enemy
	public Transform target;

	private Vector3 dir;
	public float speed = 0.5f;
	//public float speed = kc.GetComponent(speed);
	public float maxDistance = 1.0f;
	//public int hp = kh.GetComponent (leaves);
	
	private bool enemyFound = false;
	private bool hasWeapon = false;
	private bool enemyInRange = false;
	private int randNum;

	void Awake(){
	}

	// Use this for initialization
	void Start () {
		//puts the nodes into the nodeArr, not sure if needed
		//nodeArr = GameObject.FindGameObjectsWithTag ("node");
		//for(int i = 0; i < nodeArr.Length; i++){

		//}
	}
	
	// Update is called once per frame
	void Update () {

		//searches for a nearby weapon
		if(!enemyFound && !hasWeapon){
			//findWeapon();
		}

		//uses weapon on target
		if(enemyFound && enemyInRange && hasWeapon){
			//useWeapon();
		}

		if(!enemyFound && !hasWeapon){// keep hasWeapon false for now, weapons aren't in yet
			findEnemy ();
		}
	}

	//moves towards node
	void findEnemy(){
		nodeTar = node1;
		transform.position = Vector3.MoveTowards (transform.position, nodeTar.transform.position, speed * Time.deltaTime);	
		if(transform.position.x == nodeTar.transform.position.x){
			nodeTar = node2;
			//Destroy (node1); //ai will still move towards node1 even if node1 is destroyed
			transform.position = Vector3.MoveTowards (transform.position, nodeTar.transform.position, speed * Time.deltaTime);	
		}
	}

	void findWeapon(){

	}

	void useWeapon(){

	}

	void runAway(){

	}

	void kartTransition(){

	}

	/*MANDATORY AI:
	 * chase- will chase an enemy in the shortest path possible
	 * find enemy-will go in a predetermined path to different areas of each level trying to find enemies
	 * run away-drive away as fast as possible if low on health
	 * use weapon- just uses the weapon
	 * find weapon-searches around for a weapon
	 * move to next node-need to implement somehow
	 * turn-might be its own function or not
	 * drifting-
	 * kart transition-
	 */

	/*
	 * MORE SOPHISTOCATED AI STUFF (only use if everything else is done):
	 *  sneak-take a longer way in order to try to kill an enemy as stealthily as possible (need to redefine possibly)
		bait???-(not sure if needed, need to ask if weapons being held can be seen by others)
		blocking-if a projectile is targetting self, then this character will try to go behind either an enemy or obstacle to avoid the damage from the projectile
		swarm-if an enemy is detected and can only get out in limited ways, then this character will 'trap' the target
	 */
}
