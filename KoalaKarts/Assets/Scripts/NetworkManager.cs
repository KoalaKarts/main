using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	private const string typeName = "KoalaKartsTheOneAndOnly";
	private const string gameName = "KoalaRoom";

	private void StartServer()
	{
		Network.InitializeServer(6, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		MasterServer.ipAddress = "127.0.0.1";
	}
	
	void OnServerInitialized()
	{
		Debug.Log("Server Initialized");
	}

	void OnGUI()
	{
		if(!Network.isClient && !Network.isServer)
		{
			if(GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
