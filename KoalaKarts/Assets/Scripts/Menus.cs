// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class Menus : MonoBehaviour {
	
	
	public Transform selected;
	public GameObject menuCamera;
	public AudioSource clickAudio;
	
	void  Start (){
		
	}
	
	void  Update (){
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				selected = hit.transform;

				if(selected.name == "Play")
				{
					menuCamera.transform.Rotate(0, 90, 0);
					clickAudio.Play();
				}
				
				if(selected.name == "Koala")
				{
					menuCamera.transform.Rotate(0, 90, 0);
					clickAudio.Play();
				}
				
				if(selected.name == "FinishedKart")
				{
					menuCamera.transform.Rotate(0, 90, 0);
					clickAudio.Play();
				}
				
				if(selected.name == "Stage1")
				{
					Application.LoadLevel("Level 1");
					clickAudio.Play();
				}
				
				if(selected.name == "Quit")
				{
					clickAudio.Play();
					Application.Quit();
				}
			}
		}
		
		if(Input.GetKey("escape"))
		{
			Application.Quit();
		}
	}
}