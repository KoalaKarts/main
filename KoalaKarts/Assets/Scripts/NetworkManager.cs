using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	//Used for ID & Server-building purposes
	private const string typeName = "KoalaKartsTheOneAndOnly";
	private const string gameName = "KoalaRoom";
	public GameObject GenericPlayer;

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

	private void SpawnPlayer(Vector3 startpoint)
	{
		Network.Instantiate (GenericPlayer, startpoint, Quaternion.identity, 0);
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
		Vector3 startPoint = new Vector3(1282.335f, 0.9999638f, 1604.21f);
		SpawnPlayer(startPoint);
	}

	//For CLIENTS. When you've connected to a server, this happens
	void OnConnectedToServer()
	{
		Vector3 startPoint = new Vector3(1130.081f, 1f, 250.0062f);
		SpawnPlayer(startPoint);
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
