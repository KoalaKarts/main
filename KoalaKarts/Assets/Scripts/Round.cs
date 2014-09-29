using UnityEngine;
using System.Collections;

public class Round : MonoBehaviour {

    public enum STATE
    {
        BeforeGame,
        DuringGame,
        AfterGAme
    }
    STATE state = STATE.BeforeGame;

	void Update () 
    {
	    
	}
}
