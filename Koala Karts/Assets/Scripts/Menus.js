#pragma strict

var selected : Transform;
var menuCamera : GameObject;
var clickAudio : AudioSource;

function Start ()
{
	
}

function Update ()
{
	if(Input.GetMouseButtonDown(0))
	{
		var ray = camera.ScreenPointToRay(Input.mousePosition);
		var hit : RaycastHit;
		
		if(Physics.Raycast(ray, hit))
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