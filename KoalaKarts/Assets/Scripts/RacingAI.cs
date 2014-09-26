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

	/*WEAPON STUFF*/
	private bool hasWeapon = false;

	/*COMBAT STUFF*/

	/*AI ATTRIBUTES*/
	private Vector3 dir;
	public float speed = 0.5f;
	//public float speed = kc.GetComponent(speed);
	public float minDistance = 2.0f;
	//public int hp = kh.GetComponent (leaves);


	//other players
	public GameObject enemy;
	//targetted enemy
	public Transform target;

	private bool enemyFound = false;
	private bool enemyInRange = false;
	
	void Start () {

		currNode = nodeArr [0];
		nodeNum = 0;
	}

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
			if(Vector3.Distance (currNode.transform.position, transform.position) < minDistance){
				nodeNum++;
				if(nodeNum > nodeArr.Length - 1){
					nodeNum = 0;
				}
				currNode = nodeArr[nodeNum];
			}
		}
	}

	//moves towards node
	void findEnemy(){
		dir = currNode.transform.position - transform.position;
		Vector3 mov = dir.normalized * speed * Time.deltaTime;
		transform.position += mov;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (dir), 4 * Time.deltaTime);
	}

	void findWeapon(){

	}

	void useWeapon(){

	}

	void runAway(){

	}

	void kartTransition(){

	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.name == "ItemSpawner"){
			Destroy(col.gameObject);
			hasWeapon = true;
		}
	}

	void aiVision(){
	
	}

	void foundEnemy(){

	}

	/*MANDATORY AI NEEDED:
	 * chase- will chase an enemy in the shortest path possible
	 * find enemy:need to work on turning and stuff
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
