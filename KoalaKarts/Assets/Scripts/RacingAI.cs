using UnityEngine;
using System.Collections;

public class RacingAI : MonoBehaviour {
	//where the ai goes to when finding enemy
	public GameObject node1;
	public GameObject node2;
	public GameObject node3;
	public GameObject node4;
	public int nodeNum;
	//public GameObject[] nodeArr = GameObject.FindGameObjectsWithTag("node");
	//lower the number = go to first
	public int nodePriority = 1; 
	public int nodeRange;

	//weapon(s) that this character is holding
	public GameObject weapon;
	//other players
	public GameObject enemy;
	//targetted enemy
	public Transform target; 

	private Vector3 dir;
	public float speed = 5.0f;
	public int maxDistance = 1;
	public int hp;
	public int leaves;
	
	private bool enemyFound = false;
	private bool hasWeapon = false;
	private bool enemyInRange = false;
	private int randNum;

	void Awake(){
	}

	// Use this for initialization
	void Start () {
		//creates the nodes, each with a different priority number (e.g 1 = go to first, 2 = go to second, etc.)

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

	//moves towards node to see if an enemy is within a certain range of the targetted node
	void findEnemy(){
		transform.position = Vector3.MoveTowards (transform.position, node1.transform.position, speed * Time.deltaTime);	
		if(transform.position == node1.transform.position){
			transform.position = Vector3.MoveTowards (transform.position, node2.transform.position, speed * Time.deltaTime);	
		}
	}

	void findWeapon(){

	}

	void useWeapon(){

	}
	/*MANDATORY AI:
	 * turn-might be its own function or not
	 * drifting-
	 * kart transition-
	 */
	/*AI ARCHETYPES AND FUNCTIONS (Highest Priority):
	 * chase- will chase an enemy in the shortest path possible
	 * find enemy-will go in a predetermined path to different areas of each level trying to find enemies
	 * run away-drive away as fast as possible if low on health
	 * wall follow-same as find enemy except it follows walls instead
	 * obstacle follow-same as find enemy except goes to nearby objects instead of a predetermined path
	 * combat-for trying to kill the enemy
	 * use weapon- just uses the weapon
	 * find weapon-searches around for a weapon
	 * move to next node-need to implement somehow
	 */

	/*
	 * MORE SOPHISTOCATED AI STUFF (only use if everything else is done):
	 *  sneak-take a longer way in order to try to kill an enemy as stealthily as possible (need to redefine possibly)
		bait???-(not sure if needed, need to ask if weapons being held can be seen by others)
		blocking-if a projectile is targetting self, then this character will try to go behind either an enemy or obstacle to avoid the damage from the projectile
		swarm-if an enemy is detected and can only get out in limited ways, then this character will 'trap' the target
	 */
}
