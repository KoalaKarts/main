using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	//Used for ID & Server-building purposes
	private const string typeName = "KoalaKartsTheOneAndOnly";
	private const string gameName = "KoalaRoom";

	private HostData[] hostList;


	//Builds the server itself
	private void StartServer()
	{
		Network.InitializeServer(6, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		//MasterServer.ipAddress = "127.0.0.1";
	}

	private void RefreshHostList()
	{
		MasterServer.RequestHostList (typeName);
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect (hostData);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
						hostList = MasterServer.PollHostList ();
	}

	//CHANGE THESE. They just give pleasant messages now, but they should actually do something later on.

	//For HOSTS. When you've initialized a server, this happens
	void OnServerInitialized()
	{
		Debug.Log("Server Initialized");
	}

	//For CLIENTS. When you've connected to a server, this happens
	void OnConnectedToServer()
	{
		Debug.Log ("Server Joined");
	}

	//Here's where the magic happens, baby
	//We should probably use more sophisticated effects here
	void OnGUI()
	{
		if(!Network.isClient && !Network.isServer)
		{
			if(GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();

			if(GUI.Button (new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();

			if(hostList != null)
			{
				for(int i = 0; i < hostList.Length; i++)
				{
					if(GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

}
