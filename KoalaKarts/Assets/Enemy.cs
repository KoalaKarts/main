using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
	public static List<Collider> allEnemyColldiers = new List<Collider>();

	void OnEnable(){
		if (collider) {
				allEnemyColldiers.Add (collider);
			}
		}

	void OnDisable(){
		if (collider) {
			allEnemyColldiers.Remove (collider);
		}
	}

	void Update(){
		//need to use foreach loop over allEnemyColldiers and use Vector3.Angle w/ transform.forward to get whether or not its in the cone
	}
}